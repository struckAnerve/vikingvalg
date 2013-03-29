using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    interface IHandleCollision
    {
        void AddCollidable(ICanCollide canCollide);
        void RemoveCollidable(ICanCollide toRemove);
    }
}
