using System;
using Microsoft.Xna.Framework.Input;

namespace Vikingvalg
{
    interface IManageInput
    {
        //tastaturets forrige og nåværende tilstand
        MouseState PrevMouse { get; }
        MouseState CurrMouse { get; }

        /// <summary>
        /// Sjekker om gitt knapp ikke er trykket
        /// </summary>
        /// <param name="key">Knapp å sjekke</param>
        /// <returns>true hvis knappen ikke er trykket</returns>
        bool KeyIsUp(Keys key);

        /// <summary>
        /// Sjekker om gitt knapp er trykker
        /// </summary>
        /// <param name="key">Knapp å sjekke</param>
        /// <returns>true hvis knappen er trykket</returns>
        bool KeyIsDown(Keys key);

        /// <summary>
        /// Sjekker om knappen er trykket på i denne framen. Nyttig om man bare skal sjekke et trykk (ikke at det holdes inne)
        /// </summary>
        /// <param name="key">Knapp å sjekke</param>
        /// <returns>true hvis knappen er trykket på i denne framen</returns>
        bool KeyWasPressedThisFrame(Keys key);

        /// <summary>
        /// Sjekker om museknappen er trykket på i denne framen. Nyttig om man bare skal sjekke et trykk (ikke at det holdes inne)
        /// </summary>
        /// <param name="button">Knapp å sjekke (left/right/middle)</param>
        /// <returns>true dersom knappen ble trykket inn i denne framen</returns>
        bool MouseWasPressedThisFrame(String button);

        //få alle knapper som presses kan jo også være fint å få med?
    }
}
