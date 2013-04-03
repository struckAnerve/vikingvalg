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
    class AnimatedEnemy : AnimatedSprite, ICanCollide
    {
        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        public AnimatedSprite ColidingWith { get; set; }
        //Hitbox til spilleren
        protected Rectangle _footBox;

        public Rectangle FootBox
        {
            get { return _footBox; }
            set { }
        }
        public AnimatedEnemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, String animationDirectory, float scale)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, animationDirectory, scale)
        {
            //Legger til alle navn på animasjoner som fienden har, brukes for å laste inn riktige animasjoner.
            animationList.Add("attack1");
            animationList.Add("attack2");
            animationList.Add("walk");
            animationList.Add("run");
            animationList.Add("taunt");

            //Plasserer boksen midstilt nederst på karakteren.
            //_footBox = new Rectangle((int)(destinationRectangle.X - footBoxXOfset), (int)(destinationRectangle.Y + footBoxYOfset), destinationRectangle.Width, (int)(footBoxHeight));
        }
        public override void Update()
        {
            //walk();
        }
        public void taunt()
        {
            if (animationPlayer.CurrentAnimation != "taunt" && AnimationState != "taunting")
            {
                animationPlayer.TransitionToAnimation("taunt", 0.2f);
                AnimationState = "taunting";
            }
        }
        public void idle()
        {
            if (animationPlayer.CurrentAnimation != "idle" && AnimationState != "idle")
            {
                animationPlayer.TransitionToAnimation("idle", 0.2f);
                AnimationState = "idle";
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
            if (spriteAnimation.CurrentAnimation != "walking" && AnimationState != "walking")
            {
                animationPlayer.TransitionToAnimation("walk", 0.2f);
                AnimationState = "walking";
            }
            if (AnimationState == "walking")
            {
                if ((BlockedLeft && _speed < 0) || (BlockedRight && _speed > 0))
                {
                    _speed *= -1;
                    footBoxXOffset *= -1;
                    Flipped = !Flipped;
                }
                _destinationRectangle.X += _speed;
                _footBox.X = _destinationRectangle.X - footBoxWidth / 2 + footBoxXOffset;
                _footBox.Y = _destinationRectangle.Y + footBoxYOffset;
            }
        }
        protected void setFootBox(Rectangle targetBox)
        {
            _footBox = new Rectangle(targetBox.X, targetBox.Y, targetBox.Width, targetBox.Height);
        }
        public void Kill()
        {
            Console.WriteLine("Ouch!");
        }
    }
}
