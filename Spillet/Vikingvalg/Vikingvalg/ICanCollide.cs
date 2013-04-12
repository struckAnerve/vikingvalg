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
    /// <summary>
    /// Interfacet for objekter som skal kunne kollidere
    /// </summary>
    public interface ICanCollide
    {
        //boksen man kan kollidere med (sitter nederst ved "føttene")
        Rectangle FootBox { get; }

        //booleans for å si i hvilke retninger objektet er blokket
        bool BlockedLeft { get; set; }
        bool BlockedRight { get; set; }
        bool BlockedTop { get; set; }
        bool BlockedBottom { get; set; }
    }
}
