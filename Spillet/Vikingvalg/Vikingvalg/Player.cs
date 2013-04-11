using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Demina;
namespace Vikingvalg
{
    class Player : AnimatedCharacter, IUseInput, ICanCollide
    {
        Dictionary<string, string> playerBoneList = new Dictionary<string, string>();
        public Rectangle targetBox;
        public int targetBoxXDif = 60;
        public int targetBoxYDif = -6;
        public int totalMoney;
        public int totalXP;
        private int _maxHitpoints;
        public int battleRating { get; set; }
        private int statBonus { get; set; }
        //Mining
        public List<Stone> StonesToMine { get; set; }
        private IManageStates _inGameManager;
        public Player(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Game game)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale, game)
        {
            Directory =  @"player";
            setSpeed(4);
            battleRating = 1;
            setStats();
            setHpBar();
            //kan flyttes til base?
            destinationRectangle.Width = (int)(destinationRectangle.Width*scale);
            destinationRectangle.Height= (int)(destinationRectangle.Height*scale);
            
            //Setter hitboxen til spilleren til 40px høy og bredden på spilleren / 2
            footBoxWidth = (int)destinationRectangle.Width;
            footBoxXOffset =(int)footBoxWidth / 2;
            footBoxYOffset = (int)(110 * scale);
            footBoxHeight = (int)(60 * scale);
            
            //Plasserer boksen midstilt nederst på spilleren.
            _footBox = new Rectangle(destinationRectangle.X - footBoxXOffset, destinationRectangle.Y + footBoxYOffset, footBoxWidth, (int)footBoxHeight);
            targetBox = new Rectangle(_footBox.X - targetBoxXDif / 2 - 5, _footBox.Y, _footBox.Width + targetBoxXDif, _footBox.Height + targetBoxYDif);
            
            //Legger til alle navn på animasjoner som spilleren har, brukes for å laste inn riktige animasjoner.
            animationList.Add("block");
            animationList.Add("strikeSword");
            animationList.Add("battleBlockWalk");
            animationList.Add("walkCycle");

            //Legger til alle navn på ben som hører til animasjonen, brukes for å endre på teksturer
            playerBoneList.Add("head", "head");
            playerBoneList.Add("torso", "torso");
            playerBoneList.Add("bicep", "bicepL");
            playerBoneList.Add("forearmR", "forearmR");
            playerBoneList.Add("idleShieldArm", "idleShieldArm");
            playerBoneList.Add("blockingShieldArm", "blockingShieldArm");
            playerBoneList.Add("shield", "shield");
            playerBoneList.Add("swordHand", "swordHand");
            playerBoneList.Add("thigh", "thighL");
            playerBoneList.Add("shin", "shinL");
            playerBoneList.Add("foot", "footL");

        }
        public Player(Rectangle destinationRectangle, float scale, Game game)
            : this("mm", destinationRectangle, new Rectangle(0, 0, 375, 485), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 0.6f, scale, game)
        { }
        public Player(Rectangle destinationRectangle, Game game)
            : this(destinationRectangle, 1f, game)
        { }
        public Player(Vector2 destinationPosition, Game game)
            : this(new Rectangle((int)destinationPosition.X, (int)destinationPosition.Y, 375, 485), game)
        { }
        public Player(Game game)
            : this(Vector2.Zero, game)
        { }

        public void Reset(int resetX, int resetY)
        {
            _footBox.X = resetX;
            _footBox.Y = resetY;
            _damage = 10 * battleRating;
            hp = _maxHitpoints;
            healthbar.reset();
            //Flytter hitboxen til samme sted som spilleren
            _destinationRectangle.Y = ((int)(_footBox.Y - footBoxYOffset));
            _destinationRectangle.X = ((int)(_footBox.X - footBoxXOffset));

            healthbar.setPosition(_footBox);
        }
        public void Update(IManageInput inputService)
        {
            setLayerDepth(_footBox.Bottom );
            if (animationPlayer.CurrentAnimation == "battleBlockWalk")
                animationPlayer.PlaySpeedMultiplier = 1.4f;
            else
                animationPlayer.PlaySpeedMultiplier = 1f;
            if(AnimationState != "walking") _audioManager.StopLoop("walk");
            if (AnimationState != "slashing")
            {
                if (inputService.KeyIsDown(Keys.Space))
                {
                    attackSlash();
                }
                else if (inputService.KeyIsDown(Keys.LeftShift))
                {
                    block();
                }
                //Hvis én av piltastene er nede, aktiver walk
                else if (inputService.KeyIsDown(Keys.Left) || inputService.KeyIsDown(Keys.Down) || inputService.KeyIsDown(Keys.Up) || inputService.KeyIsDown(Keys.Right))
                {
                    walk(inputService);
                }
                else
                {
                    idle();
                }
            }
            else if (animationPlayer.Transitioning == false && animationPlayer.CurrentKeyframeIndex > 0 && animationPlayer.CurrentKeyframeIndex == (animationPlayer.currentPlayingAnimation.Keyframes.Count() - 1))
            {
                if(activeEnemy != null)
                {
                    if (targetBox.Intersects(activeEnemy.FootBox) &&
                        ((activeEnemy.FootBox.Center.X < this.FootBox.Center.X && this.Flipped) || (activeEnemy.FootBox.Center.X > this.FootBox.Center.X && !this.Flipped)))
                    {

                        _audioManager.AddSound(Directory + "/attack" + rInt(1, 3));
                        AnimatedEnemy enemyColidedWith = (AnimatedEnemy)CollidingWith;
                        activeEnemy.takeDamage(_damage);
                    }
                }
                else if (StonesToMine != null)
                {
                    foreach (Stone stone in StonesToMine)
                    {
                        if (targetBox.Intersects(stone.FootBox) && stone.endurance > 0 && FacesTowards(stone.FootBox.Center.X))
                        {
                            _audioManager.AddSound(Directory + "/clang" + rInt(1, 4));
                            stone.IsHit();
                        }
                    }
                }
                idle();
            }
        }
        /// <summary>
        /// Gjør om animasjonen som blir spilt til "strikeSword" hvis den animasjonen ikke allerede er aktiv
        /// </summary>
        public void attackSlash()
        {
            if (AnimationState != "slashing")
            {
                animationPlayer.TransitionToAnimation("strikeSword", 0.2f);
                AnimationState = "slashing";
            }
        }
        /// <summary>
        /// Holder oversikt over hvilke knapper som er trykket, og hvorvidt spilleren er blokkert
        /// </summary>
        /// <param name="inputService">Inputservice som holder oversikt over input</param>
        public void walk(IManageInput inputService)
        {
            /* Hvis "walking" animasjonen ikke er aktiv, og AnimationState ikke er "walking"
             * aktiveres "walking" animasjonen, og bytter AnimationState til "walking" */
            if (animationPlayer.CurrentAnimation != "walking" && AnimationState != "walking")
            {
                animationPlayer.TransitionToAnimation("battleBlockWalk", 0.2f);
                AnimationState = "walking";
            }
            if (AnimationState == "walking")
            {
                //_audioManager.AddSound("walk");
                _audioManager.PlayLoop("walk");
                if (inputService.KeyIsDown(Keys.Right) && inputService.KeyIsDown(Keys.Left))
                {
                    idle();
                }
                else if (inputService.KeyIsDown(Keys.Right) && inputService.KeyIsUp(Keys.Left) && !BlockedRight)
                {
                    _destinationRectangle.X += _xSpeed;
                    Flipped = false;
                }
                else if (inputService.KeyIsDown(Keys.Left) && inputService.KeyIsUp(Keys.Right) && !BlockedLeft)
                {
                    _destinationRectangle.X -= _xSpeed;
                    Flipped = true;
                }
                else if (inputService.KeyIsUp(Keys.Up) && inputService.KeyIsUp(Keys.Down) || BlockedTop || BlockedBottom)
                {
                    idle();
                }
                if (inputService.KeyIsDown(Keys.Up) && inputService.KeyIsDown(Keys.Down))
                {
                    idle();
                }
                else if (inputService.KeyIsDown(Keys.Up) && !BlockedTop)
                {
                    _destinationRectangle.Y -= _xSpeed;
                }
                else if (inputService.KeyIsDown(Keys.Down) && !BlockedBottom)
                {
                    _destinationRectangle.Y += _xSpeed;
                }
                //Flytter hitboxen til samme sted som spilleren
                _footBox.Y = ((int)(_destinationRectangle.Y + footBoxYOffset));
                _footBox.X = (int)(_destinationRectangle.X - footBoxXOffset);
                targetBox.X = (int)(_footBox.X - targetBoxXDif / 2);
                targetBox.Y = (int)_footBox.Y - targetBoxYDif / 2;
                healthbar.setPosition(_footBox);
            }
        }
        /// <summary>
        /// Gjør om animasjonen som blir spilt til "strikeSword" hvis den animasjonen ikke allerede er aktiv
        /// </summary>
        public void block()
        {
            if (animationPlayer.CurrentAnimation != "blocking" && AnimationState != "blocking")
            {
                animationPlayer.TransitionToAnimation("block", 0.2f);
                AnimationState = "blocking";
            }
        }
        public void takeDamage(int damageTaken)
        {
            if (AnimationState == "blocking")
            {
                hp -= (int)(damageTaken * 0.3);
                _audioManager.AddSound(Directory + "/blockHit");
            }
            else
            {
                hp -= damageTaken;
            }
            healthbar.updateHealtBar(hp);
            if (hp <= 0) dead();
        }
        private void dead(){
            _footBox.X -= 2000;
            if (battleRating > 1)
            {
                totalXP -= 10 * battleRating;
                totalMoney -= 10 * battleRating;
            }
        }
        public void addXP(int xpToAdd)
        {
            totalXP += xpToAdd;
            Console.WriteLine("XP: " + totalXP);
        }
        public void addMoney(int moneyToAdd)
        {
            totalMoney += moneyToAdd;
            Console.WriteLine("Money: " + totalMoney);
        }
        public void setStats(){
            statBonus = battleRating;
            for (int i = 0; i <= 20; i += 2)
            {
                statBonus++;
            }
            hp = 50 * battleRating;
            _maxHitpoints = 50 * battleRating;
            _damage = 10 * battleRating;
        }
    }
}
