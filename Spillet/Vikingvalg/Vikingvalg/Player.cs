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
        //Hvorvidt spilleren er blokkert fra høyre, venstre, oppe, eller nede
        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        private Rectangle TargetBox;
        List<string> playerTextureList = new List<string>(); //Liste over teksturer i animasjonen
        
        public int targetBoxXDif = 60; //Hvor mye større denne boksen er i bredden
        public int targetBoxYDif = -6; //Hvor mye større denne boksen er i høyden
        public int totalGold; //Hvor mye penger spilleren har
        public int totalXP; //Hvor mye xp spilleren har
        //Fighting
        public int combatLevel { get; set; } //Hvor "sterk" spilleren er/level
        private int statBonus { get; set; } //Hvor mye bonus spilleren får fra combatLevel
        //Mining
        public List<Stone> StonesToMine { get; set; } //Liste over steiner i området
        private InGameManager _inGameManager; //inGameManager(for å bytte ingamestate når spilleren dør)
        private IManageSprites _spritemanager; //Spritemanager(For å laste inn teksturer når spilleren går opp i level)
        public Player(Rectangle destinationRectangle, float layerDepth, float scale, Game game)
            : base(destinationRectangle, layerDepth, scale, game)
        {
            Directory = @"player"; 
            setSpeed(4);
            combatLevel = 1;
            setStats();
            setHpBar();

            //Henter ut inGameManager og spriteManager fra game
            _inGameManager = (InGameManager)game.Services.GetService(typeof(InGameManager));
            _spritemanager = (IManageSprites)game.Services.GetService(typeof(IManageSprites));

            //Skalerer spilleren med scale
            destinationRectangle.Width = (int)(destinationRectangle.Width * scale);
            destinationRectangle.Height = (int)(destinationRectangle.Height * scale);

            //Setter hitboxen til spilleren til 40px høy og bredden på spilleren / 2
            footBoxWidth = (int)destinationRectangle.Width;
            footBoxXOffset = (int)footBoxWidth / 2;
            footBoxYOffset = (int)(110 * scale);
            footBoxHeight = (int)(60 * scale);

            //Plasserer boksen midstilt nederst på spilleren.
            _footBox = new Rectangle(destinationRectangle.X - footBoxXOffset, destinationRectangle.Y + footBoxYOffset, footBoxWidth, (int)footBoxHeight);
            TargetBox = new Rectangle(_footBox.X - targetBoxXDif / 2 - 5, _footBox.Y, _footBox.Width + targetBoxXDif, _footBox.Height + targetBoxYDif);

            //Legger til alle navn på animasjoner som spilleren har, brukes for å laste inn riktige animasjoner.
            animationList.Add("block");
            animationList.Add("strikeSword");
            animationList.Add("battleBlockWalk");

            //Legger til alle navn på ben som hører til animasjonen, brukes for å endre på teksturer
            playerTextureList.Add("torso");
            playerTextureList.Add("bicep");
            playerTextureList.Add("shieldHand_Walking");
            playerTextureList.Add("hip");
            playerTextureList.Add("shin");
            playerTextureList.Add("foot");
            playerTextureList.Add("head");
            playerTextureList.Add("forearm");
            playerTextureList.Add("swordHand");
            playerTextureList.Add("helmet");
            playerTextureList.Add("shield_StudWood_Ready");
            playerTextureList.Add("shield_StudWood_Slashing");
            playerTextureList.Add("shieldHand_Blocking");
            
        }
        public Player(Rectangle destinationRectangle, float scale, Game game)
            : this(destinationRectangle, 0.6f, scale, game)
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
            setStats();
            //Flytter spilleren til samme sted som hitboksen
            _destinationRectangle.Y = ((int)(_footBox.Y - footBoxYOffset));
            _destinationRectangle.X = ((int)(_footBox.X - footBoxXOffset));
            healthbar.setPosition(_footBox);
        }
        public void Update(IManageInput inputService)
        {
            if(inputService.KeyWasPressedThisFrame(Keys.P)) levelUp();
            //setter layerdepth til bunnen av spilleren
            setLayerDepth(_footBox.Bottom);
            //Øker hastigheten til walk-animasjonen
            if (animationPlayer.CurrentAnimation == "battleBlockWalk")
                animationPlayer.PlaySpeedMultiplier = 1.4f;
            else
                animationPlayer.PlaySpeedMultiplier = 1f;
            //Hvis spilleren ikke går, stopp gålyden
            if (AnimationState != "walking") _audioManager.StopLoop("walk");
            //Hvis spilleren ikke angriper
            if (AnimationState != "slashing")
            {
                //SJekk hvilke knapper som blir trykket og aktiver funksjonene
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
                //Hvis ingenting blir trykket, stå stille
                else
                {
                    idle();
                }
            }
            //Hvis spilleren angriper og er på slutten av animasjonen
            else if (animationPlayer.Transitioning == false && animationPlayer.CurrentKeyframeIndex > 0 && animationPlayer.CurrentKeyframeIndex == (animationPlayer.currentPlayingAnimation.Keyframes.Count() - 1))
            {
                //Hvis det finnes en aktiv fiende
                if (activeEnemy != null)
                {
                    //Hvis targetboksen er innenfor fiendens footbox, og spilleren ser mot fienden
                    if (TargetBox.Intersects(activeEnemy.FootBox) && FacesTowards((float)activeEnemy.FootBox.Center.X))
                    {
                        //Aktiver tilfeldig angrepslyd og gjør skade på fienden
                        _audioManager.AddSound(Directory + "/attack" + rInt(1, 3));
                        activeEnemy.takeDamage(_damage);
                    }
                }
                //Hvis det finnes steiner som kan treffes
                else if (StonesToMine != null)
                {
                    //Sjekker om spillerens targetbox er innenfor en av steinene i listen, og om han står mot dem
                    foreach (Stone stone in StonesToMine)
                    {
                        if (TargetBox.Intersects(stone.FootBox) && stone.endurance > 0 && FacesTowards(stone.FootBox.Center.X))
                        {
                            //Aktiver tilfeldig "slå på stein"-lyd og gjør skade på steinen
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
            /* Hvis AnimationState er "walking"
             * (Man kan ikke bruke currentanimation, fordi han skal bevege seg mens han er i overgang mellom to animasjoner)*/
            if (AnimationState == "walking")
            {
                //Start gålyden
                _audioManager.PlayLoop("walk");
                //Hvis man både holder nede venstre og høyre, stå stille
                if (inputService.KeyIsDown(Keys.Right) && inputService.KeyIsDown(Keys.Left))
                {
                    idle();
                }
                //Hvis man holder nede høyreknappen, og man ikke er blokkert fra høyre
                else if (inputService.KeyIsDown(Keys.Right) && !BlockedRight)
                {
                    //Ikke snu karakteren, beveg han mot høyre
                    _destinationRectangle.X += _xSpeed;
                    Flipped = false;
                }
                //Hvis man holder nede venstreknappen, og man ikke er blokkert fra venstre
                else if (inputService.KeyIsDown(Keys.Left) && !BlockedLeft)
                {
                    //Snu karakteren og beveg han mot venstre
                    _destinationRectangle.X -= _xSpeed;
                    Flipped = true;
                }
                //Hvis man både holder nede opp og ned, stå stille
                if (inputService.KeyIsDown(Keys.Up) && inputService.KeyIsDown(Keys.Down))
                {
                    idle();
                }
                //Hvis man holder nede opp, og man ikke er blokkert fra venstre
                else if (inputService.KeyIsDown(Keys.Up) && !BlockedTop)
                {
                    _destinationRectangle.Y -= _xSpeed;
                }
                //Hvis man holder nede ned, og man ikke er blokkert fra venstre
                else if (inputService.KeyIsDown(Keys.Down) && !BlockedBottom)
                {
                    _destinationRectangle.Y += _xSpeed;
                }
                //Flytter hitboxen til samme sted som spilleren
                _footBox.Y = ((int)(_destinationRectangle.Y + footBoxYOffset));
                _footBox.X = (int)(_destinationRectangle.X - footBoxXOffset);
                TargetBox.X = (int)(_footBox.X - targetBoxXDif / 2);
                TargetBox.Y = (int)_footBox.Y - targetBoxYDif / 2;
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
        /// <summary>
        /// Tar skade, basert på hvor mye som blir sendt inn
        /// </summary>
        /// <param name="damageTaken">Hvor mye skade som skal trekkes fra</param>
        public override void takeDamage(int damageTaken)
        {
            //Hvis spilleren blokkerer, trekk fra 70% på skaden og spill av "blockHit" lyden
            if (AnimationState == "blocking")
            {
                damageTaken = (int)(damageTaken * 0.3);
                _audioManager.AddSound(Directory + "/blockHit");
            }
            base.takeDamage(damageTaken);
        }
        /// <summary>
        /// Når spilleren dør, bytt til Choosedirectionlevel, og hvis spilleren er over level 1, 
        /// trekk fra 10 * combatLevel i xp og penger
        /// </summary>
        public override void dead()
        {
            _inGameManager.ChangeInGameState("ChooseDirectionLevel", 100, 450);
            if (combatLevel > 1)
            {
                totalXP -= 10 * combatLevel;
                totalGold -= 10 * combatLevel;
                if (totalXP < 0) totalXP = 0;
                if (totalGold < 0) totalGold = 0;
            }
        }
        /// <summary>
        /// Legg til så mye xp som blir sendt inn
        /// </summary>
        /// <param name="xpToAdd">hvor mye XP som skal legges til</param>
        public void addXP(int xpToAdd)
        {
            totalXP += xpToAdd;
        }
        /// <summary>
        /// Legg til så mye penger som blir sendt inn
        /// </summary>
        /// <param name="xpToAdd">hvor mye penger som skal legges til</param>
        public void addMoney(int moneyToAdd)
        {
            totalGold += moneyToAdd;
        }
        /// <summary>
        /// Regner ut stats basert på combatLevel
        /// Statbonus øker med én annenhver combatLevel
        /// </summary>
        public void setStats()
        {
            statBonus = combatLevel;
            for (int i = 0; i <= 20; i += 2)
            {
                statBonus++;
            }
            if (combatLevel != 1) _damage = 10 * (combatLevel / 2);
            else _damage = 10;
            MaxHp = 50 * combatLevel;
            CurrHp = MaxHp;
            if (healthbar != null) 
            healthbar.reset();
        }
        /// <summary>
        /// Funksjon for å gå opp i level
        /// </summary>
        public void levelUp()
        {
            combatLevel += 1;
            Texture2D textureToLoad;
            /*Går gjennom hver streng i playerTextureList, og endrer teksturen som tilsvarer det navnet til den 
             *teksturen som ligger inne i mappen til levelet (E.g "level1" mappe som inneholder teksturer) */
            foreach (String textureName in playerTextureList)
            {
                textureToLoad = _spritemanager.LoadTexture2D(@"Animations/" + Directory + "Animation/level" + combatLevel + "/" + textureName);
                animationPlayer.setTexture(textureToLoad, playerTextureList.IndexOf(textureName));
            }
        }
        /// <summary>
        /// Sjekker om man er rettet mot det punktet som blir sendt inn
        /// </summary>
        /// <param name="point">Punkt som skal sjekkes imot</param>
        /// <returns>bool</returns>
        public bool FacesTowards(float point)
        {
            return (point < this.FootBox.Center.X && this.Flipped) || (point > this.FootBox.Center.X && !this.Flipped);
        }
    }
}
