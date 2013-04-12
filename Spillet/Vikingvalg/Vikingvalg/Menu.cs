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
    /// Superklasse for menyene
    /// </summary>
    abstract class Menu
    {
        //komponenter
        private IManageSprites _spriteService;
        public IManageStates stateService;

        //liste over det som skal tegnes
        public List<Sprite> toDrawMenuClass = new List<Sprite>();

        public Menu(IManageSprites spriteService, IManageStates stateService)
        {
            this._spriteService = spriteService;
            this.stateService = stateService;
        }
        
        public abstract void ChangeMenuState(String changeToState);
        public abstract void MainState();

        public abstract void Update(IManageInput inputService, GameTime gameTime);

        /// <summary>
        /// Legg til en Sprite i tegnelisten
        /// </summary>
        /// <param name="toAdd"></param>
        public void AddDrawable(Sprite toAdd)
        {
            if (toAdd == null || toDrawMenuClass.Contains(toAdd))
            {
                Console.WriteLine("Menu: Unable to add drawable!");
                return;
            }

            toDrawMenuClass.Add(toAdd);
        }

        //Fjern Sprite fra tegnelisten
        public void RemoveDrawable(Sprite toRemove)
        {
            toDrawMenuClass.Remove(toRemove);
        }
    }
}
