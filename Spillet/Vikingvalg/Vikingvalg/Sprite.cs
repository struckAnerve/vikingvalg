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
    public abstract class Sprite
    {
        //Bør flyttes
        protected int _speed = 3;

        //Bør flyttes
        protected int footBoxHeight = 40;
        protected int footBoxXOfset;
        protected int footBoxYOfset;

        public Color Color { get; protected set; }
        public float Rotation { get; protected set; }

        public Sprite(Color color, float rotation)
        {
            Color = color;
            Rotation = rotation;
        }

        public virtual void Update()
        { }
    }
}
