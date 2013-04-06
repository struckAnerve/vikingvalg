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
        protected int footBoxHeight { get; set; }
        protected int footBoxWidth { get; set; }
        protected int footBoxXOffset { get; set; }
        protected int footBoxYOffset { get; set; }

        public float LayerDepth { get; set; }
        public Color Color { get; protected set; }
        public float Rotation { get; protected set; }

        public Sprite(Color color, float rotation, float layerDepth)
        {
            Color = color;
            Rotation = rotation;
            LayerDepth = layerDepth;
        }
        public void setLayerDepth(float layerDepth)
        {
            LayerDepth = layerDepth;
        }
        public virtual void Update()
        { }
    }
}
