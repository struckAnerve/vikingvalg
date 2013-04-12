using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vikingvalg
{
    /// <summary>
    /// Interfacet til komponenten som tar seg av tegning
    /// </summary>
    interface IManageSprites
    {
        //Størrelsen på vinduet representert med en Vector2 (som dessverre betyr at x = width og y = height)
        Vector2 GameWindowSize { get; }
        //et tall som representerer margen fra toppen av vinduet og ned til der hvor spilleren kan gå
        int WalkBlockTop { get; set; }

        //boolean som bestemmer om karakterers liv skal tegnes eller ikke
        bool DrawHealthBar { get; set; }

        //en liste over listene med Sprites som skal tegnes. Denne inneholder til enhver tid det aller meste som skal tegnes
        List<List<Sprite>> ListsToDraw { get; set; }

        /// <summary>
        /// Laster inn en Texture2D fra en Sprite, og legger den i en liste. Vi tenkte at det ville være en god idé å samle all
        /// innlasting i denne komponenten.
        /// </summary>
        /// <param name="toLoad">Spriten man ønsker å laste</param>
        void LoadDrawable(Sprite toLoad);

        /// <summary>
        /// Returnerer en Texture2D utifra navnet (laster den inn og legger den inn i en liste dersom den ikke ligger der fra før)
        /// </summary>
        /// <param name="artName">Navnet på filen du ønsker å laste inn som en Texture2D og få returnert</param>
        /// <returns>Texture2Den som er lagret som artName</returns>
        Texture2D LoadTexture2D(String artName);

        /// <summary>
        /// Måler og returnerer bredden og høyden på en streng
        /// </summary>
        /// <param name="text">Teksten du vil måle</param>
        /// <returns>Returnerer bredd og høyde representert ved en Vector2, x = width y = height</returns>
        Vector2 TextSize(String text);

        /// <summary>
        /// Funskjon som returnerer en "oppdelt" streng basert på en gitt maksbredde
        /// </summary>
        /// <param name="text">Teksten du vil dele opp</param>
        /// <param name="maxLineWidth">Maks bredde</param>
        /// <returns>Returnerer en streng med \n der teksten må deles opp for å passe inn i maksbredden</returns>
        String WrapText(string text, float maxLineWidth);
    }
}
