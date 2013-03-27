using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    interface IDrawSprites
    {
        void AddDrawable(Sprite drawable);
        void RemoveDrawable(Sprite drawable);
    }
}
