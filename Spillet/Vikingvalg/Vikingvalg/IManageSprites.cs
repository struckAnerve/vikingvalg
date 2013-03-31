using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    interface IManageSprites
    {
        //Tegn/oppdater det som skjer in-game
        bool DrawInGame { get; set; }
        bool UpdateInGame { get; set; }
        void AddInGameDrawable(Sprite drawable);
        void RemoveInGameDrawable(Sprite drawable);

        //Tegn/oppdater det som skjer i menyer
    }
}
