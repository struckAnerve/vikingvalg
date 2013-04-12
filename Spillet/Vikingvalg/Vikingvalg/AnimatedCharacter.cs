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
    class AnimatedCharacter : AnimatedSprite, IPlaySound
    {
        public AnimatedSprite CollidingWith { get; set; }
        public int currHp { get; set; }
        public int maxHp { get; set; }
        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        public IManageAudio _audioManager { get; set; }
        private static Random _rand = new Random();
        private static readonly object syncLock = new object();
        public override String Directory { get; set; }
        protected int _damage { get; set; }
        protected int _speed { get; set; }
        protected int _xSpeed { get; set; }
        protected int _ySpeed { get; set; }
        protected int _attackDamageFrame { get; set; }
        protected bool _attacked { get; set; }

        public Healthbar healthbar {get; set;}
        public AnimatedEnemy activeEnemy;
        public AnimatedCharacter(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Game game)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale)
        {
            _audioManager = (IManageAudio)game.Services.GetService(typeof(IManageAudio));
        }
        protected void setHpBar()
        {
            if (this is WolfEnemy) healthbar = new Healthbar(currHp, _destinationRectangle, _destinationRectangle.Height - 60);
            else healthbar = new Healthbar(maxHp, _destinationRectangle, _destinationRectangle.Height);
        }
        public bool FacesTowards(float point)
        {
            return (point < this.FootBox.Center.X && this.Flipped) || (point > this.FootBox.Center.X && !this.Flipped);
        }

        protected void setSpeed(int speed)
        {
            _speed = speed;
            _xSpeed = speed;
            _ySpeed = speed / 2;
        }
        public static int rInt(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return _rand.Next(min, max);
            }
        }
    }
}
