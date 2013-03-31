using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    interface IManageSprites
    {
        bool DrawInGame { get; set; }
        bool UpdateInGame { get; set; }
        void AddInGameDrawable(Sprite drawable);
        void RemoveInGameDrawable(Sprite drawable);

        bool DrawAndUpdateMenu { get; set; }
        void AddMenuDrawable(Sprite drawable);
        void RemoveMenuDrawable(Sprite drawable);
    }
}
