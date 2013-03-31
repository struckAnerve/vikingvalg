using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    interface IManageSprites
    {
        void AddInGameDrawable(Sprite drawable);
        void RemoveInGameDrawable(Sprite drawable);
        void AddMenuDrawable(Sprite drawable);
        void RemoveMenuDrawable(Sprite drawable);
    }
}
