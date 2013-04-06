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
    class AnimatedStaticSprite : StaticSprite
    {
        protected int _currentFrame = 0;
        protected int _numberOfFrames;
        protected float _animationStepTime;
        protected float _timer;

        public bool IsPlaying { get; set; }

        public AnimatedStaticSprite(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, int numberOfFrames, float animationStepTime)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            _numberOfFrames = numberOfFrames;
            _animationStepTime = animationStepTime;
            IsPlaying = true;
        }
        public AnimatedStaticSprite(String artName, Rectangle destinationRectangle, Vector2 sourcePos, int numberOfFrames, float animationStepTime)
            : this(artName, destinationRectangle, new Rectangle((int)sourcePos.X, (int)sourcePos.Y, destinationRectangle.Width, destinationRectangle.Height),
                new Color(255, 255, 255, 255), 0, Vector2.Zero, SpriteEffects.None, 0.3f, numberOfFrames, animationStepTime)
        { }

        public void Update(GameTime gameTime)
        {
            if (_currentFrame >= _numberOfFrames)
            {
                _currentFrame++;
            }
            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timer >= _animationStepTime)
            {
                _timer -= _animationStepTime;
                _currentFrame++;
                _sourceRectangle.X = _destinationRectangle.Width*_currentFrame;
            }
        }
    }
}
