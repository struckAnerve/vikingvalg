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
    interface IManageInput
    {
        MouseState PrevMouse { get; }
        MouseState CurrMouse { get; }

        bool KeyIsUp(Keys key);
        bool KeyIsDown(Keys key);
        bool KeyWasPressedThisFrame(Keys key);
        bool MouseWasPressedThisFrame(String button);
        //få alle knapper som presses
    }
}
