﻿using System;
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
    class PauseMenu : Menu
    {
        public ContinueButton continueButton;

        private enum _possiblePauseMenuStates { Main };
        private String _pauseMenuState;

        public PauseMenu(IManageSprites spriteService, IManageStates stateService)
            : base(spriteService, stateService)
        {
            ChangeMenuState("Main");

            //Bør forbedres
            continueButton = new ContinueButton(new Vector2(
                ((int)spriteService.GameWindowSize.X / 2 - 90),
                ((int)spriteService.GameWindowSize.Y / 2 - 37)), this);
            spriteService.LoadDrawable(continueButton);
        }

        public override void ChangeMenuState(string changeToState)
        {
            if (!Enum.IsDefined(typeof(_possiblePauseMenuStates), changeToState))
            {
                Console.WriteLine("Unable to change MainMenu state (you are trying to change to an unkown state: '" + changeToState + "')");
                return;
            }

            _pauseMenuState = changeToState;
        }

        public override void MainState()
        {
            toDrawMenuClass.Clear();
            AddDrawable((Sprite)continueButton);
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
