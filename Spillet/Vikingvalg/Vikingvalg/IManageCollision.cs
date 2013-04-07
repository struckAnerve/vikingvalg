using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    interface IManageCollision
    {
        void AddCollidable(ICanCollideBorder canCollide);
        void RemoveCollidable(ICanCollideBorder toRemove);

        void Enable();
        void Disable();
    }
}
