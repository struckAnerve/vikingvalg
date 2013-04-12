using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vikingvalg
{
    abstract class AnimatedCharacter : AnimatedSprite, IPlaySound
    {
        public int CurrHp { get; set; } //Nåværende Hitpoints
        public int MaxHp { get; set; } //Maks hitpoints

        public IManageAudio _audioManager { get; set; }//Audiomanager
        private static Random _rand = new Random(); //Random objekt
        private static readonly object syncLock = new object(); //Objekt som brukes som lås for randomfunksjon

        public override String Directory { get; set; } //Navnet til karakteren, brukes for å navigere i mapper

        protected int _damage { get; set; } //Hvor mye skade karakteren gjør
        protected int _speed { get; set; } //Hastigheten til karakteren
        protected int _xSpeed { get; set; } // x-hastigheten til karakteren
        protected int _ySpeed { get; set; } // y-hastigheten til karakteren
        protected int _attackDamageFrame { get; set; } //Hvilken frame man må være på i animasjonen for å utføre skade i et angrep

        public Healthbar healthbar {get; set;} //Healthbar
        public AnimatedEnemy activeEnemy; //Hvilken fiende som er aktiv

        public AnimatedCharacter(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Game game)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale)
        {
            _audioManager = (IManageAudio)game.Services.GetService(typeof(IManageAudio)); //Henter ut audiomanager fra game
        }
        /// <summary>
        /// Setter posisjonen til healthbar (Litt annerledes for Wolfenemy enn alle andre)
        /// </summary>
        protected void setHpBar()
        {
            if (this is WolfEnemy) healthbar = new Healthbar(CurrHp, _destinationRectangle.Height - 60);
            else healthbar = new Healthbar(MaxHp, _destinationRectangle.Height);
        }
        //Setter hastigheten til karakteren, _ySpeed er halvparten av _xSpeed
        protected void setSpeed(int speed)
        {
            _speed = speed;
            _xSpeed = speed;
            _ySpeed = speed / 2;
        }
        /*Funksjon for å få tak i et tilfeldig tall, vi lagde en egen funksjon for å 
         * kunne få tak i flere tilfeldige tall innenfor en veldig kort periode, som beskrevet her:
         * http://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number */
        public static int rInt(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return _rand.Next(min, max);
            }
        }
        /// <summary>
        /// Tar skade, basert på hvor mye som blir sendt inn
        /// Player har litt annen funksjonalitet, og overrider denne
        /// </summary>
        /// <param name="damageTaken">Hvor mye skade som skal trekkes fra</param>
        public virtual void takeDamage(int damageTaken)
        {
            CurrHp -= damageTaken;
            healthbar.updateHealtBar(CurrHp, MaxHp);
            if (CurrHp <= 0) dead();
        }
        /// <summary>
        /// Funksjon som aktiveres når karakteren har 0 eller mindre hitpoints
        /// Fiender og spiller har helt forskjellig funksjonalitet, derfor er den abstrakt
        /// </summary>
        public abstract void dead();
    }
}
