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
        private int flipTimer = 0;
        private Rectangle _lastAllowedPosition;
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
            flipTimer--;
            xTarget = _player1.FootBox.X + _player1.FootBox.Width+5;
            yTarget = _player1.FootBox.Y + _player1.FootBox.Height;

            xDistance = _footBox.X - xTarget;
            yDistance = _footBox.Y + _footBox.Height - yTarget;
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
        /// <summary>
        /// Holder oversikt over hvilke knapper som er trykket, og hvorvidt spilleren er blokkert
        /// </summary>
        /// <param name="inputService">Inputservice som holder oversikt over input</param>
        public void walk()
        {
            /* Hvis "walking" animasjonen ikke er aktiv, og AnimationState ikke er "walking"
             * aktiveres "walking" animasjonen, og bytter AnimationState til "walking" */
            if (_footBox.X > xTarget - targetSpan && _footBox.X < xTarget + targetSpan && _footBox.Y + _footBox.Height > yTarget - targetSpan && _footBox.Y + _footBox.Height < yTarget + targetSpan)
            {
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
                    if (_xSpeed >= 0 || _lastAllowedPosition.X == _footBox.X && flipTimer <= 0)
                    {
                        Flipped = true;
                        flipTimer = 4;
                    }
                    else if (_xSpeed < 0 && flipTimer <= 0) 
                    {
                        Flipped = false;
                        flipTimer = 4;
                    }
                    /*
                    if (BlockedLeft && _xSpeed < 0 || BlockedRight && _xSpeed > 0 && ColidingWith is AnimatedCharacter)
                    {
                        _footBox.X = _lastAllowedPosition.X;
                    }
                    */
                    if (BlockedBottom && _ySpeed < 0 || BlockedTop && _ySpeed > 0 && ColidingWith is AnimatedCharacter)
                    {
                        _footBox.Y = _lastAllowedPosition.Y;
                        if (_footBox.X == _lastAllowedPosition.X)
                        {
                            _footBox.X++;
                            flipTimer = 4;
                        }
                    }
                    _destinationRectangle.X = _footBox.X + footBoxWidth / 2 - footBoxXOffset;
                    _destinationRectangle.Y = _footBox.Y -footBoxXOffset;
                    _lastAllowedPosition = _footBox;

                }
            }
        }
        public void takeDamage()
        {
            hp -= 10;
            if (hp <= 0) Console.WriteLine("Dead");
        }
    }
}
