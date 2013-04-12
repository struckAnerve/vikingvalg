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
    public abstract class AnimatedSprite : Sprite
    {
        /* Hvilken animasjonsstate spriten er i 
         * (Annerledes fra currentplayinganimation, tar ikke hensyn til transition) */
        public String AnimationState{get; protected set;} 
        public float Scale { get; protected set; } //Størrelses-faktoren til spriten
        public bool Flipped { get; set; } //Hvorvidt spriten ser til høyre eller venstre

        public Rectangle DestinationRectangle //Hvor spriten skal tegnes
        {
            get { return _destinationRectangle; }
            protected set { _destinationRectangle = value; }
        }

        public abstract String Directory { get; set; } //Mappen til animasjonen
        public List<String> animationList { get; protected set; } //Liste over navn på animasjoner
        public AnimationPlayer animationPlayer { get; set; } //Avspilleren til animasjonen

        //Hitbox til spriten
        protected Rectangle _footBox;

        public Rectangle FootBox
        {
            get { return _footBox; }
            set { }
        }

        public AnimatedSprite(Rectangle destinationRectangle, float layerDepth, float scale)
            : base(layerDepth)
        {
            Flipped = false;
            AnimationState = "idle";
            animationList = new List<String>();
            animationList.Add("idle");
            animationPlayer = new AnimationPlayer();
            Scale = scale;
            DestinationRectangle = destinationRectangle;
            LayerDepth = layerDepth;
        }
        /// <summary>
        /// Aktiverer "idle" animasjonen, dette er standardanimasjonen til alt som er av AnimatedSprite typen
        /// </summary>
        public void idle()
        {
            if (AnimationState != "idle")
            {
                animationPlayer.TransitionToAnimation("idle", 0.2f);
                AnimationState = "idle";
            }
        }
    }
}
