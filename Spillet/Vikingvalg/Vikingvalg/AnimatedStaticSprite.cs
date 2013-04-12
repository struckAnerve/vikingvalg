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

namespace Vikingvalg
{
    /// <summary>
    /// Underklasse av StaticSprite, som kan animeres i tillegg (er skrevet relativt spesifikt)
    /// </summary>
    class AnimatedStaticSprite : StaticSprite
    {
        //nåværende frame
        public int currentFrame = 0;
        //antall frames (i x-retning)
        protected int _numberOfFrames;
        //hvor fort man skal bytte til neste frame
        protected float _animationStepTime;
        //tidtaker
        protected float _timer;
        //om den skal spilles en gang eller for alltid
        protected bool _playOnce;

        //om animasjonen spiller
        public bool IsPlaying { get; set; }

        public AnimatedStaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, int numberOfFrames, float animationStepTime, bool playOnce)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            _numberOfFrames = numberOfFrames;
            _animationStepTime = animationStepTime;
            _playOnce = playOnce;
            IsPlaying = true;
        }
        public AnimatedStaticSprite(String artName, Rectangle destinationRectangle, Vector2 sourcePos, int numberOfFrames, float animationStepTime, bool playOnce)
            : this(artName, destinationRectangle, new Rectangle((int)sourcePos.X, (int)sourcePos.Y, destinationRectangle.Width, destinationRectangle.Height),
                new Color(255, 255, 255, 255), 0, Vector2.Zero, SpriteEffects.None, 0.3f, numberOfFrames, animationStepTime, playOnce)
        { }

        public void Update(GameTime gameTime)
        {
            //sjekker om animasjonen skal loopes eller ikke
            if (currentFrame >= _numberOfFrames)
            {
                if (_playOnce)
                {
                    IsPlaying = false;
                }
                else
                    currentFrame = 0;
            }
            //ny source x
            _sourceRectangle.X = _destinationRectangle.Width * currentFrame;
            //oppdaterer tid
            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //bytt frame dersom tiden er inne
            if (_timer >= _animationStepTime)
            {
                _timer -= _animationStepTime;
                currentFrame++;
            }
        }

        /// <summary>
        /// For å endre source i Y-retning
        /// </summary>
        /// <param name="newYPos"></param>
        public void ChangeYPosition(int newYPos)
        {
            _sourceRectangle.Y = newYPos;
        }
    }
}
