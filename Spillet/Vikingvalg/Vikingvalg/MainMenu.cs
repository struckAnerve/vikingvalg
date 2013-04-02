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
    class MainMenu : Menu
    {
        public StaticSprite playButton;
        public StaticSprite settingsButton;

        private enum _possibleMainMenuStates { Main };
        private String _mainMenuState;

        public MainMenu(IManageSprites spriteService)
            : base(spriteService)
        {
            ChangeMenuState("Main");

            //Bør forbedres
            playButton = new StaticSprite("play", new Rectangle(
                ((int)spriteService.GameWindowSize.X / 2 - 90),
                ((int)spriteService.GameWindowSize.Y / 2 - 37),
                180, 75));
            settingsButton = new StaticSprite("settings", new Rectangle(
                ((int)spriteService.GameWindowSize.X / 2 - 90),
                ((int)spriteService.GameWindowSize.Y / 2 - 24 + (playButton.SourceRectangle.Height + 40)),
                180, 49));
        }

        public override void ChangeMenuState(string changeToState)
        {
            if (!Enum.IsDefined(typeof(_possibleMainMenuStates), changeToState))
            {
                Console.WriteLine("Unable to change MainMenu state (you are trying to change to an unkown state: '" + changeToState + "')");
                return;
            }

            _mainMenuState = changeToState;
        }

        public override void MainState()
        {
            AddDrawable((Sprite)playButton);
            AddDrawable((Sprite)settingsButton);
        }

        public override void Update(IManageInput inputService)
        {
            //if state == main <-- slik?

            //Midlertidig kode for å teste endring av State
            /*
            if (inputService.KeyWasPressedThisFrame(Keys.Tab))
            {
                stateService.ChangeState("InGame");
            }
            */

            /* Mulig noe av dette må med?
            foreach (Sprite toUpdate in ToDrawMainMenu)
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
            */
        }
    }
}
