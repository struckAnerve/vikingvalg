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
    abstract class Menu
    {
        IManageSprites spriteService;
        public IManageStates stateService;

        public List<Sprite> toDrawMenuClass = new List<Sprite>();

        public Menu(IManageSprites spriteService, IManageStates stateService)
        {
            this.spriteService = spriteService;
            this.stateService = stateService;
        }
        
        //Kan kanskje defineres i denne klassen?
        public abstract void ChangeMenuState(String changeToState);
        public abstract void MainState();
        //public void SettingsState() { }
        public abstract void Update(IManageInput inputService, GameTime gameTime);

        public void AddDrawable(Sprite toAdd)
        {
            if (toAdd == null || toDrawMenuClass.Contains(toAdd))
            {
                Console.WriteLine("Menu: Unable to add drawable!");
                return;
            }

            toDrawMenuClass.Add(toAdd);
        }

        public void RemoveDrawable(Sprite toRemove)
        {
            toDrawMenuClass.Remove(toRemove);
        }
    }
}
