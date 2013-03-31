using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    interface IManageSprites
    {
        List<List<Sprite>> ListsToDraw { get; set; }
        void LoadDrawable(Sprite toLoad);
    }
}
