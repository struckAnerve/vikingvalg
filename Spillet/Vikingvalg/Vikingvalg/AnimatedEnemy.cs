using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vikingvalg
{
    class AnimatedEnemy : AnimatedCharacter
    {
        public int mobIndex { get; set; } //Hvilket nummer fienden har fått
        protected int _xpWorth; // Hvor mye XP spilleren får for å drepe fienden
        public Player _player1 { private get; set; } //Spilleren
        protected float randomDifficulty; //Tilfeldig vanskelighetsgrad for fienden

        private Point _distance; //Avstand fra der fienden er, til der den skal
        private Point _target; // Målet som fienden skal til
        private int _targetSpan = 7; //Et slingringsmonn for hvor fienden kan ende opp
        private int[] _yPosArray; //En liste over posisjoner som fienden kan stelle seg ved
        private Rectangle _lastAllowedPosition; //Siste lovlige posisjon for fienden

        private bool _attackedAfterMoving; //Hvorvidt fienden har angrepet en gang etter å ha flyttet på seg eller ikke
        private bool _positionRight = true; //Om man er på høyresiden av spilleren
        private bool _attackRight = true; //Om man angriper fra høyresiden av spilleren
        private bool _attackOfOpportunity = false; //Om man skal angripe én gang, selv om man ikke er activeenemy
        private bool _firstAttack = true; //Om man har angrepet en gang før
        protected String _attackSound; //Hvilken lyd som skal spilles av når fienden angriper

        public AnimatedEnemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Player player1, Game game)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale, game)
        {
            _yPosArray = new int[] { 0, -150, -100, 0, 100, 150 };
            _player1 = player1;
            _lastAllowedPosition = _destinationRectangle;
            //Legger til alle navn på animasjoner som fienden har, brukes for å laste inn riktige animasjoner i AnimatedSprite.
            animationList.Add("attack1");
            animationList.Add("attack2");
            animationList.Add("walk");
            animationList.Add("run");
            animationList.Add("taunt");
        }
        public override void Update()
        {
            //Setter layerdepth til bunnen av fienden
            setLayerDepth(_footBox.Bottom );
            //Hvis fienden ikke angriper, aktiver walk()
            if (AnimationState != "attacking")
            {
                walk();
            }
            //Hvis fienden ikke er i overgang mellom animasjoner, og keyframen man er på tilsvarer den keyframen man skal være på for å utføre et angrep
            else if (animationPlayer.Transitioning == false && animationPlayer.CurrentKeyframeIndex > 0 && animationPlayer.CurrentKeyframeIndex >= _attackDamageFrame)
            {
                //Hvis man ikke har angrepet denne gangen
                if (!_attackedAfterMoving)
                {
                   setAttackTargetDistance(); //Regn ut hvor langt man er fra spilleren, i tilfelle spilleren har flyttet på seg
                   _targetSpan += 70; //Øk slingringsmonnet for hvor langt unna man kan være i det man angriper
                   if (withinRangeOfTarget(_footBox, _target))
                       _player1.takeDamage(_damage); //Hvis man er nærme nok, skad spilleren
                   _audioManager.AddSound(Directory+"/"+_attackSound); //Legg til angrepslyd i audiomanager
                   _targetSpan -= 70;
                   _attackOfOpportunity = false;
                   _attackedAfterMoving = true;
                }
                //Hvis man er på siste keyframe i angrepsanimasjonen, sett fienden til idle.
                if (animationPlayer.CurrentKeyframeIndex == (animationPlayer.currentPlayingAnimation.Keyframes.Count() - 1))
                {
                    idle();
                    _attackedAfterMoving = false;
                }
                
            }
        }
        /// <summary>
        /// Denne funksjonen ble ikke implementert, den skulle gjøre at fienden "terger" spilleren
        /// </summary>
        public void taunt()
        {
            if (animationPlayer.CurrentAnimation != "taunt" && AnimationState != "taunting")
            {
                animationPlayer.TransitionToAnimation("taunt", 0.2f);
                AnimationState = "taunting";
            }
        }
        /// <summary>
        /// Hvis fienden ikke allerede angriper
        /// Kjør overgang til angrepsanimasjonen
        /// Sett animasjonsstate til å si at han angriper
        /// Sett angrepslyden til "attack1"
        /// </summary>
        public virtual void attack1()
        {
            if (animationPlayer.CurrentAnimation != "attack1" && AnimationState != "attacking")
            {
                animationPlayer.TransitionToAnimation("attack1", 0.2f);
                AnimationState = "attacking";
                _attackSound = "attack1";
            }
        }
        /// <summary>
        /// Samme som attack1, bare for attack2
        /// </summary>
        public virtual void attack2()
        {
            if (animationPlayer.CurrentAnimation != "attack2" && AnimationState != "attacking")
            {
                animationPlayer.TransitionToAnimation("attack2", 0.2f);
                AnimationState = "attacking";
                _attackSound = "attack2";
            }
        }
        /// <summary>
        /// Regn ut avstanden til fra fienden til målet (spilleren)
        /// Hvis fienden er ved spilleren, og enten ikke har angrepet en gang, 
        /// eller fienden har fått tilbake over 95 fra rInt, angrip. 50% sjanse
        /// for attack1 eller attack2
        /// Hvis man ikke får over 95, stå stille.
        /// </summary>
        public void attackFormation()
        {
            setAttackTargetDistance();
            if (withinRangeOfTarget(_footBox, _target) && AnimationState != "attacking" && (_firstAttack || rInt(0, 100) > 95))
            {
                if (rInt(0,2) < 1) attack1();
                else attack2();
                _firstAttack = false;
            }
            else if (withinRangeOfTarget(_footBox, _target)) idle();
        }
        /// <summary>
        /// Regn ut avstand til målet, som er et sted rundt spilleren.
        /// 1 av 1000 ganger vil fienden flytte seg fra høyre for spilleren, til venstre.
        /// Hvis man er ved målet, stå stille.
        /// </summary>
        public void waitingFormation()
        {
            if (rInt(0, 1000) >= 999)
            {
                _positionRight = !_positionRight;
            }
            setWaitingTargetDistance();
            if (withinRangeOfTarget(_footBox, _target))
            {
                idle();
            }
        }
        public void walk()
        {
            //Hvis man er den aktive fienden, sett til angrepsformasjon, som bestemmer setter målet til fienden
            if (activeEnemy == this || _attackOfOpportunity)
            {
                attackFormation();
            }
            /*Hvis man ikke er det, sett målet til et sted rundt spilleren og vent.
             * 1 av 1000 ganger, vil fienden gå inn for et overraskelsesangrep */
            else
            {
                if (rInt(0, 1000) >= 999 && _attackOfOpportunity == false) _attackOfOpportunity = true;
                waitingFormation();
            } 
            //Hvis man er til høyre for fienden, så angriper man fra høre, hvis ikke angriper man fra venstre.
            if (_footBox.Center.X > _player1.FootBox.Center.X) _attackRight = true;
            else _attackRight = false;
            //Hvis man er ved målet sitt, stå stille og snu fienden mot spilleren
            if (withinRangeOfTarget(_footBox, _target))
            {
                if (_attackRight) Flipped = true;
                else Flipped = false;
            }
            //Hvis man ikke er ved målet sitt
            else
            {
                //Sett animasjonen til "walk", og animationstate til "walking" hvis den ikke er det
                if (animationPlayer.CurrentAnimation != "walk" && AnimationState != "walking")
                {
                    animationPlayer.TransitionToAnimation("walk", 0.2f);
                    AnimationState = "walking";
                }
                /*Hvis animationstate er "walking" 
                 * (Man kan ikke bruke currentanimation, fordi de skal bevege seg mens de er i overgang mellom to animasjoner)*/
                if (AnimationState == "walking")
                {
                    //Regn ut hvordan fienden skal bevege seg basert på rotasjon og avstand
                    // Kode fra http://www.berecursive.com/2008/c/rotating-a-sprite-towards-an-object-in-xna //
                    float rotation = (float)Math.Atan2(_distance.Y, _distance.X);
                    _xSpeed = (int)(_speed * Math.Cos(rotation));
                    _ySpeed = (int)(_speed * Math.Sin(rotation));
                    // Kode fra http://www.berecursive.com/2008/c/rotating-a-sprite-towards-an-object-in-xna //
                    //Flytt fienden etter _xSpeed og _ySpeed
                    _footBox.X -= _xSpeed;
                    _footBox.Y -= _ySpeed;
                    //Hvis fienden er på høyresiden og beveger seg mot venstre, pek mot venstre
                    if (_xSpeed > 0 && _footBox.Center.X > _target.X)
                    {
                        Flipped = true;
                    }
                    //Hvis man er på venstresiden av fienden og beveger seg mot høyre, pek mot høyre
                    else if (_xSpeed < 0 && _footBox.Center.X < _target.X)
                    {
                        Flipped = false;
                    }
                    //Når man beveger seg skal firstattack settes tilbake til true, sånn at fienden angriper med en gang han kommer fram
                    _firstAttack = true;
                    //Sett hvor spilleren skal tegnes i forhold til footbox
                    _destinationRectangle.X = _footBox.Center.X - footBoxXOffset;
                    _destinationRectangle.Y = _footBox.Y - footBoxXOffset;
                    //Flytt på healthbar
                    healthbar.setPosition(_footBox);
                    
                }
            }
        }
        /// <summary>
        /// Når fienden dør, gi xp til spilleren
        /// </summary>
        public override void dead()
        {
            _player1.addXP(_xpWorth);
        }
        /// <summary>
        /// Sier om fienden er i nærheten av målet sitt eller ikke
        /// Bruker slingringsmonnet for å definere området fienden kan være innenfor
        /// </summary>
        /// <param name="box">boksen som skal sjekkes</param>
        /// <param name="target">målet som skal sjekkes imot</param>
        /// <returns>bool</returns>
        private bool withinRangeOfTarget(Rectangle box, Point target){
            if (box.Bottom > target.Y + 6 - _targetSpan && box.Bottom < target.Y + _targetSpan)
            {
                //Hvis man angriper fra høyre, skal venstresiden av boksen sjekkes, og motsatt for venstre.
                if (_attackRight)
                    return (box.Left > target.X - _targetSpan && box.Left < target.X + _targetSpan);
                else 
                    return (box.Right > target.X - _targetSpan && box.Right < target.X + _targetSpan && box.Bottom > target.Y - _targetSpan && box.Bottom < target.Y + _targetSpan);
            }
            return false;
        }
        /// <summary>
        /// Regner ut hvor langt det er fra der fienden er, til der den skal være for å kunne angripe
        /// </summary>
        public void setAttackTargetDistance()
        {
            if (_footBox.Center.X > _player1.FootBox.Center.X)
            {
                _target.X = _player1.FootBox.X + _player1.FootBox.Width + 5;
                _distance.X = _footBox.X - _target.X;
            }
            else
            {
                _target.X = _player1.FootBox.X - 5;
                _distance.X = _footBox.X + _footBox.Width - _target.X;
            }
            _target.Y = _player1.FootBox.Y + _player1.FootBox.Height;
            _distance.Y = _footBox.Y + _footBox.Height - _target.Y;
        }
        /// <summary>
        /// Regner ut hvor langt det er fra der fienden er, til der den skal være mens den venter
        /// </summary>
        private void setWaitingTargetDistance()
        {
            //Hvis fienden plasserer seg på høyresiden av spilleren
            if (_positionRight)
            {
                //Sjekker om målet den prøver å sette til er utenfor skjermen, hvis den er det, flytt målet innenfor skjermen
                _target.Y = _player1.FootBox.Top + _yPosArray[mobIndex];
                while (_target.Y - _footBox.Height - 40 < 170) _target.Y += 1;
                while (_target.Y > 700) _target.Y -= 2;
                //X-posisjonen regnes ut fra Y-posisjonen slik at fienden plasserer seg i en "sirkel" rundt spilleren
                _target.X = _player1.FootBox.Right + 100 + (int)((Math.Sqrt(Math.Pow(200, 2) - Math.Pow(_yPosArray[mobIndex], 2)) * 1.2));
                while (_target.X + _footBox.Width > 1350) _target.X--;
                _distance.X = _footBox.Left - _target.X;
            }
            //Samme som over, bare for venstresiden av spilleren
            else if (!_positionRight)
            {
                _target.Y = _player1.FootBox.Top + (_yPosArray[mobIndex] * -1);
                while (_target.Y - _footBox.Height - 40 < 170) _target.Y += 2;
                while (_target.Y > 700) _target.Y -= 2;
                _target.X = _player1.FootBox.Left - 100 - (int)((Math.Sqrt(Math.Pow(200, 2) - Math.Pow((_yPosArray[mobIndex] * -1), 2)) * 1.2));
                while (_target.X - _footBox.Width < -100) _target.X += 2;
                _distance.X = _footBox.Right - _target.X;
            }
            _distance.Y = _footBox.Y + _footBox.Height - _target.Y;
        }
        /// <summary>
        /// Setter vanskelighetsgraden til fienden basert på combatlevel til spiller
        /// Definerer skade, hit points, og hvor mye xp fienden er verdt.
        /// </summary>
        /// <param name="combatLevel">combatLevel til spiller</param>
        /// <param name="baseHp">Hvor mye hp fienden har i lvl 1</param>
        /// <param name="baseDamage">Hvor mye skade fienden gjør i lvl 1</param>
        public void setDifficulty(int combatLevel, int baseHp, int baseDamage)
        {
            randomDifficulty = rInt(combatLevel - 2, combatLevel + 1);
            if (combatLevel == 1 || randomDifficulty <= 1) randomDifficulty = 0;
            _damage = baseDamage + (int)randomDifficulty * (int)randomDifficulty;
            MaxHp = baseHp + 10 * (int)randomDifficulty * (int)randomDifficulty;
            CurrHp = MaxHp;
            _xpWorth = 10 + 10 * (int)randomDifficulty * (int)randomDifficulty;
            
        }
    }
}
