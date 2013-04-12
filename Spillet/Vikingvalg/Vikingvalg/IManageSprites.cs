using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Vikingvalg
{
    interface IManageSprites
    {
        //Skal kanskje ikke stå her? Brukes flere steder gjennom SpriteManager
        Vector2 GameWindowSize { get; }
        int WalkBlockTop { get; set; }

        bool DrawHealthBar { get; set; }

        List<List<Sprite>> ListsToDraw { get; set; }
        void LoadDrawable(Sprite toLoad);
        Texture2D LoadTexture2D(String artName);
        Vector2 TextSize(String text);
        String WrapText(string text, float maxLineWidth);
    }
}
