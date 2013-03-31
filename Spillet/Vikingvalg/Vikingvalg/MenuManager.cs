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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MenuManager : Microsoft.Xna.Framework.GameComponent
    {
        IManageStates stateService;
        IManageInput inputService;
        IManageSprites spriteService;

        private ClickableMenuElement _playButton;
        private ClickableMenuElement _settingsButton;

        public List<Sprite> ToDrawMenu { get; private set; }

        public MenuManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            stateService = (IManageStates)Game.Services.GetService(typeof(IManageStates));
            inputService = (IManageInput)Game.Services.GetService(typeof(IManageInput));
            spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));

            ToDrawMenu = new List<Sprite>();

            //Må forbedres (FOR Å SI DET SÅNN)
            _playButton = new ClickableMenuElement("play", new Rectangle(
                (Game.Window.ClientBounds.Width / 2 - 90),
                (Game.Window.ClientBounds.Height / 2 - 37),
                180, 75));
            AddDrawable((Sprite)_playButton);
            _settingsButton = new ClickableMenuElement("settings", new Rectangle(
                (Game.Window.ClientBounds.Width / 2 - 90),
                (Game.Window.ClientBounds.Height / 2 - 24 + (_playButton.SourceRectangle.Height + 40)),
                180, 49));
            AddDrawable((Sprite)_settingsButton);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Midlertidig kode for å teste endring av State
            if (inputService.KeyWasPressedThisFrame(Keys.Tab))
            {
                stateService.ChangeState("InGame");
            }

            foreach (Sprite toUpdate in ToDrawMenu)
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

            base.Update(gameTime);
        }

        public void AddDrawable(Sprite toAdd)
        {
            if (toAdd == null || ToDrawMenu.Contains(toAdd))
            {
                Console.WriteLine("MenuManager: Unable to add drawable!");
                return;
            }

            spriteService.LoadDrawable(toAdd);
            ToDrawMenu.Add(toAdd);
        }

        public void RemoveDrawable(Sprite toRemove)
        {
            ToDrawMenu.Remove(toRemove);
        }
    }
}
