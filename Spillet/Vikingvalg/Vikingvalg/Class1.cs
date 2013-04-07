/*using System;
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
    class AnimatedEnemy : AnimatedCharacter
    {
        protected int xTarget { get; set; }
        protected int yTarget { get; set; }
        protected Player _player1 { get; set; }
        private static Random rand = new Random();
        public int mobIndex { get; set; }
        private int targetSpan = 7;
        int[] yPosArray;
        private Point _distance;
        private int _yTarget { get { return _player1.FootBox.Bottom; } }
        private Point _target;
        bool positionRight = true;
        bool attackRight = true;
        private Rectangle _lastAllowedPosition;
        public AnimatedEnemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Player player1, int hitPoints)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale, hitPoints)
        {
            yPosArray = new int[] { 0, -150, -100, 0, 100, 150 };
            _player1 = player1;
            _lastAllowedPosition = _destinationRectangle;
            //Legger til alle navn på animasjoner som fienden har, brukes for å laste inn riktige animasjoner.
            animationList.Add("attack1");
            animationList.Add("attack2");
            animationList.Add("walk");
            animationList.Add("run");
            animationList.Add("taunt");
        }
        public override void Update()
        {
            setLayerDepth((float)(_footBox.Bottom / 70f));
            if (AnimationState != "attacking")
            {
                walk();
            }
            else if (animationPlayer.Transitioning == false && animationPlayer.CurrentKeyframeIndex > 0 && animationPlayer.CurrentKeyframeIndex == (animationPlayer.currentPlayingAnimation.Keyframes.Count() - 1))
            {
                Console.WriteLine(withinRangeOfTarget());
                if (withinRangeOfTarget()) _player1.takeDamage();
                idle();
            }
        }
        public void taunt()
        {
            if (animationPlayer.CurrentAnimation != "taunt" && AnimationState != "taunting")
            {
                animationPlayer.TransitionToAnimation("taunt", 0.2f);
                AnimationState = "taunting";
            }
        }
        /// <summary>
        /// Gjør om animasjonen som blir spilt til "strikeSword" hvis den animasjonen ikke allerede er aktiv
        /// </summary>
        public void attack1()
        {
            if (AnimationState != "attack1" && AnimationState != "attacking")
            {
                animationPlayer.TransitionToAnimation("attack1", 0.2f);
                AnimationState = "attacking";
            }
        }
        public void attack2()
        {
            if (spriteAnimation.CurrentAnimation != "attack2" && AnimationState != "attacking")
            {
                animationPlayer.TransitionToAnimation("attack2", 0.2f);
                AnimationState = "attacking";
            }
        }
        public void attackFormation()
        {
            setAttackTargetDistance();
            if (withinRangeOfTarget() && AnimationState != "attacking" && rand.Next(0, 100) > 95)
            {
                if (rand.Next(0, 2) < 1) attack1();
                else attack2();
            }
        }
        public void waitingFormation()
        {
            if (rInt(0, 1000) >= 999)
            {
                positionRight = !positionRight;
            }
            setWaitingTargetDistance();
            if (withinRangeOfTarget())
            {
                idle();
            }
        }
        public void walk()
        {
            if (activeEnemy == this) attackFormation();
            else waitingFormation();

            if (_footBox.Center.X > _player1.FootBox.Center.X) attackRight = true;
            else attackRight = false;

            if (withinRangeOfTarget())
            {
                if (attackRight) Flipped = true;
                else Flipped = false;
            }
            else
            {
                if (spriteAnimation.CurrentAnimation != "walking" && AnimationState != "walking")
                {
                    animationPlayer.TransitionToAnimation("walk", 0.2f);
                    AnimationState = "walking";
                }
                if (AnimationState == "walking")
                {
                    // Kode fra http://www.berecursive.com/2008/c/rotating-a-sprite-towards-an-object-in-xna //
                    //Calculate the required rotation by doing a two-variable arc-tan
                    float rotation = (float)Math.Atan2(_distance.Y, _distance.X);
                    //Move square towards mouse by closing the gap 3 pixels per update
                    _xSpeed = (int)(_speed * Math.Cos(rotation));
                    _ySpeed = (int)(_speed * Math.Sin(rotation));
                    // Kode fra http://www.berecursive.com/2008/c/rotating-a-sprite-towards-an-object-in-xna //
                    _footBox.X -= _xSpeed;
                    _footBox.Y -= _ySpeed;
                    if (_xSpeed >= 0)
                    {
                        Flipped = true;
                    }
                    else if (_xSpeed < 0 && _footBox.Center.X < xTarget)
                    {
                        Flipped = false;
                    }
                    _destinationRectangle.X = _footBox.Center.X - footBoxXOffset;
                    _destinationRectangle.Y = _footBox.Y - footBoxXOffset;
                    _lastAllowedPosition = _footBox;
                    healthbar.setPosition(_footBox);
                }
            }
        }
        public void takeDamage()
        {
            hp -= 10;
            healthbar.updateHealtBar(hp);
            if (hp <= 0) Console.WriteLine("Dead");
        }
        private bool blocked()
        {
            if (BlockedBottom || BlockedTop || BlockedLeft || BlockedRight) return true;
            return false;
        }
        private bool withinRangeOfTarget()
        {
            if (_footBox.Bottom > _target.Y + 6 - targetSpan && _footBox.Bottom < _target.Y + targetSpan)
            {
                if (attackRight)
                    return (_footBox.Left > _target.X - targetSpan && _footBox.Left < _target.X + targetSpan);
                else
                    return (_footBox.Right > _target.X - targetSpan && _footBox.Right < _target.X + targetSpan && _footBox.Bottom > _target.Y - targetSpan && _footBox.Bottom < _target.Y + targetSpan);
            }
            return false;
        }
        public static int rInt(int min, int max)
        {
            int t = rand.Next(min, max);
            return t;
        }
        
    }
}
*/