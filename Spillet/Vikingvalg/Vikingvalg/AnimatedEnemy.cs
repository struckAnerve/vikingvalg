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
    class AnimatedEnemy : AnimatedCharacter, ICanCollide
    {
        protected int xTarget { get; set; }
        protected int yTarget { get; set; }
        protected Player _player1 { get; set; }
        private float xDistance;
        private float yDistance;
        private int targetSpan = 7;
        private Rectangle _lastAllowedPosition;
        private String attackPosition;
        public AnimatedEnemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Player player1)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale)
        {
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
            walk();
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
                AnimationState = "attack2";
            }
        }
        public void attackFormation()
        {
            if (_footBox.Center.X > _player1.FootBox.Center.X)
            {
                xTarget = _player1.FootBox.X + _player1.FootBox.Width + 5;
                xDistance = _footBox.X - xTarget;
                attackPosition = "right";
            }
            else
            {
                xTarget = _player1.FootBox.X - 5;
                xDistance = _footBox.X + _footBox.Width - xTarget;
                attackPosition = "left";
            }
            yTarget = _player1.FootBox.Y + _player1.FootBox.Height;
            yDistance = _footBox.Y + _footBox.Height - yTarget;
        }
        public void waitingFormation()
        {
            if (_footBox.Center.X > _player1.FootBox.Center.X)
            {
                xTarget = _player1.FootBox.X + _player1.FootBox.Width + 5 + 100;
                xDistance = _footBox.X - xTarget;
                attackPosition = "right";
            }
            else
            {
                xTarget = _player1.FootBox.X - 5 - 100;
                xDistance = _footBox.X + _footBox.Width - xTarget;
                attackPosition = "left";
            }
            yTarget = _player1.FootBox.Y + _player1.FootBox.Height + 200;
            yDistance = _footBox.Y + _footBox.Height - yTarget;
        }
        public void walk()
        {
            if (activeEnemy == this) attackFormation();
            else waitingFormation();
            
            //Console.WriteLine(xTarget + " " + (xTarget + targetSpan) + " " + (xTarget - targetSpan) + " " + _footBox.X + " " + _footBox.Right);
            if (withinRangeOfTarget())
            {
                if (attackPosition == "right") Flipped = true;
                else Flipped = false;
                idle();
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
                    float rotation = (float)Math.Atan2(yDistance, xDistance);
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
                }
            }
        }
        public void takeDamage()
        {
            hp -= 10;
            if (hp <= 0) Console.WriteLine("Dead");
        }
        private bool blocked()
        {
            if (BlockedBottom || BlockedTop || BlockedLeft || BlockedRight) return true;
            return false;
        }
        private bool withinRangeOfTarget(){
            if (_footBox.Bottom > yTarget - targetSpan && _footBox.Bottom < yTarget + targetSpan)
            {
                if (attackPosition == "right")
                    return (_footBox.Left > xTarget - targetSpan && _footBox.Left < xTarget + targetSpan);
                else if (attackPosition == "left")
                    return (_footBox.Right > xTarget - targetSpan && _footBox.Right < xTarget + targetSpan && _footBox.Bottom > yTarget - targetSpan && _footBox.Bottom < yTarget + targetSpan);
            }
            return false;
        }
    }
}
