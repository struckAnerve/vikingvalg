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
    /// Klassen for hovedmenyen
    /// </summary>
    class MainMenu : Menu
    {
        //Spill-knappen
        public PlayButton playButton;

        //Mulige hovedmenytilstander
        private enum _possibleMainMenuStates { Main };
        //nåværende hovedmenytilstand
        private String _mainMenuState;

        public MainMenu(IManageSprites spriteService, IManageStates stateService)
            : base(spriteService, stateService)
        {
            //man starter hovedmenyen i denne tilstanden
            ChangeMenuState("Main");

            //opprett Spill-knappen
            playButton = new PlayButton(new Vector2(
                ((int)spriteService.GameWindowSize.X / 2 - 90),
                ((int)spriteService.GameWindowSize.Y / 2 - 37)), this);
            spriteService.LoadDrawable(playButton);
        }

        /// <summary>
        /// For å endre hovedmenytilstand
        /// </summary>
        /// <param name="changeToState"></param>
        public override void ChangeMenuState(string changeToState)
        {
            if (!Enum.IsDefined(typeof(_possibleMainMenuStates), changeToState))
            {
                Console.WriteLine("Unable to change MainMenu state (you are trying to change to an unkown state: '" + changeToState + "')");
                return;
            }

            _mainMenuState = changeToState;
        }

        //Kalles på når man endrer til hovedtilstanden til hovedmenyen
        public override void MainState()
        {
            toDrawMenuClass.Clear();
            AddDrawable((Sprite)playButton);
        }

        public override void Update(IManageInput inputService, GameTime gameTime)
        {
            //oppdaterer det som er i tegnelisten
            foreach (Sprite toUpdate in toDrawMenuClass)
            {
                if (toUpdate is IUseInput)
                {
                    IUseInput needsInput = (IUseInput)toUpdate;
                    needsInput.Update(inputService);
                }
                else
                {
                    toUpdate.Update();
                }
                if (toUpdate is AnimatedSprite)
                {
                    AnimatedSprite updatableAnimation = (AnimatedSprite)toUpdate;
                    updatableAnimation.animationPlayer.Update(gameTime);
                }
            }
        }
    }
}
