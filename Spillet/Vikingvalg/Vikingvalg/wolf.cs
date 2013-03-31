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
    class Wolf : AnimatedSprite, IUseInput, ICanCollide
    {
        //Hitbox til spilleren
        private Rectangle _footBox;

        public Rectangle FootBox
        {
            get { return _footBox; }
            set { }
        }

        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        public Wolf(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, "wolfanimation/")
        {
            //Setter hitboxen til spilleren til 40px høy og bredden på spilleren / 2
            footBoxXOfset = destinationRectangle.Width / 2 + 25;
            footBoxYOfset = 0;
            Flipped = true;
            //Plasserer boksen midstilt nederst på spilleren.
            _footBox = new Rectangle(destinationRectangle.X - footBoxXOfset, destinationRectangle.Y + footBoxYOfset, destinationRectangle.Width, footBoxHeight);

            animationList = new List<String>();
            animationPlayer = new AnimationPlayer();

            //Legger til alle navn på animasjoner som spilleren har, brukes for å laste inn riktige animasjoner.
            animationList.Add("bite");
            animationList.Add("pounce");
            animationList.Add("walk");
            animationList.Add("run");
            animationList.Add("idle");
            animationList.Add("taunt");
        }
        public Wolf(Rectangle destinationRectangle)
            : this("mm", destinationRectangle, new Rectangle(0, 0, 375, 485), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 0.6f)
        { }
        public Wolf(Vector2 destinationPosition)
            : this(new Rectangle((int)destinationPosition.X, (int)destinationPosition.Y, 375, 485))
        { }
        public Wolf()
            : this(Vector2.Zero)
        { }

        public void Update(IManageInput inputService)
        {
            walk();
        }
        public void taunt()
        {
            if (AnimationState != "taunting")
            {
                animationPlayer.TransitionToAnimation("taunt", 0.2f);
                AnimationState = "taunting";
            }
        }
        public void idle()
        {
            if (AnimationState != "idle")
            {
                animationPlayer.TransitionToAnimation("idle", 0.2f);
                AnimationState = "idle";
            }
        }
        /// <summary>
        /// Gjør om animasjonen som blir spilt til "strikeSword" hvis den animasjonen ikke allerede er aktiv
        /// </summary>
        public void bite()
        {
            if (AnimationState != "biting")
            {
                animationPlayer.TransitionToAnimation("bite", 0.2f);
                AnimationState = "biting";
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
                    Flipped = !Flipped;
                }
                _destinationRectangle.X += _speed;
                _footBox.Y = _destinationRectangle.Y + footBoxYOfset;
                _footBox.X = _destinationRectangle.X - footBoxXOfset;
            }
        }
        /// <summary>
        /// Gjør om animasjonen som blir spilt til "strikeSword" hvis den animasjonen ikke allerede er aktiv
        /// </summary>
        public void pounce()
        {
            if (spriteAnimation.CurrentAnimation != "pouncing" && AnimationState != "pouncing")
            {
                animationPlayer.TransitionToAnimation("pounce", 0.2f);
                AnimationState = "pouncing";
            }
        }

    }
}
