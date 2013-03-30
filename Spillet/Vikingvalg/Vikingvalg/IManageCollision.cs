using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    interface IManageCollision
    {
        void AddCollidable(ICanCollide canCollide);
        void RemoveCollidable(ICanCollide toRemove);
    }
}
