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
    class AnimatedEnemy : AnimatedCharacter
    {
        protected int xTarget { get; set; }
        protected int yTarget { get; set; }
        protected Player _player1 { get; set; }
        private float xDistance;
        private float yDistance;
        private static Random rand = new Random();
        public int mobIndex{get; set;}
        private int targetSpan = 7;
        int[] yPosArray;
        bool positionRight = true;
        bool attackRight = true;
        private Rectangle _lastAllowedPosition;
        public AnimatedEnemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Player player1)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale)
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
            }
            else
            {
                xTarget = _player1.FootBox.X - 5;
                xDistance = _footBox.X + _footBox.Width - xTarget;
            }
            yTarget = _player1.FootBox.Y + _player1.FootBox.Height;
            yDistance = _footBox.Y + _footBox.Height - yTarget;
        }
        public void waitingFormation()
        {
            yTarget = _player1.FootBox.Y + _player1.FootBox.Height + yPosArray[mobIndex];
            // y = sqrt(300^2 - tall^2)
            if (rInt(0, 1000) >= 999)
            {
                positionRight = !positionRight;
            }
            if (positionRight){
                xTarget = _player1.FootBox.Right + 100 + (int)((Math.Sqrt(Math.Pow(200, 2) - Math.Pow(yPosArray[mobIndex], 2)) * 1.2));
                xDistance = _footBox.Left - xTarget;
            }
            else if (!positionRight)
            {
                xTarget = _player1.FootBox.Left - 100 - (int)((Math.Sqrt(Math.Pow(200, 2) - Math.Pow((yPosArray[mobIndex] * -1), 2)) * 1.2));
                xDistance = _footBox.Right - xTarget;
            } 
            
            yDistance = _footBox.Y + _footBox.Height - yTarget;
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
                if (attackRight)
                    return (_footBox.Left > xTarget - targetSpan && _footBox.Left < xTarget + targetSpan);
                else 
                    return (_footBox.Right > xTarget - targetSpan && _footBox.Right < xTarget + targetSpan && _footBox.Bottom > yTarget - targetSpan && _footBox.Bottom < yTarget + targetSpan);
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
