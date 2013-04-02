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
        public PlayButton playButton;
        public SettingsButton settingsButton;

        private enum _possibleMainMenuStates { Main };
        private String _mainMenuState;

        public MainMenu(IManageSprites spriteService)
            : base(spriteService)
        {
            ChangeMenuState("Main");

            //Bør forbedres
            playButton = new PlayButton(new Vector2(
                ((int)spriteService.GameWindowSize.X / 2 - 90),
                ((int)spriteService.GameWindowSize.Y / 2 - 37)), this);
            spriteService.LoadDrawable(playButton);
            settingsButton = new SettingsButton(new Vector2(
                ((int)spriteService.GameWindowSize.X / 2 - 90),
                ((int)spriteService.GameWindowSize.Y / 2 - 24 + (playButton.SourceRectangle.Height + 40))), this);
            spriteService.LoadDrawable(settingsButton);
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
            toDrawMenuClass.Clear();
            AddDrawable((Sprite)playButton);
            AddDrawable((Sprite)settingsButton);
        }

        public override void Update(IManageInput inputService, GameTime gameTime)
        {
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
