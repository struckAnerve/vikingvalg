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
    public class InputManager : Microsoft.Xna.Framework.GameComponent, IManageInput
    {
        private KeyboardState PrevKeys { get; set; }
        private KeyboardState CurrKeys { get; set; }
        public MouseState PrevMouse { get; private set; }
        public MouseState CurrMouse { get; private set; }

        public InputManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            CurrKeys = Keyboard.GetState();
            CurrMouse = Mouse.GetState();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PrevKeys = CurrKeys;
            CurrKeys = Keyboard.GetState();
            PrevMouse = CurrMouse;
            CurrMouse = Mouse.GetState();
        }

        public bool KeyIsUp(Keys key)
        {
            return CurrKeys.IsKeyUp(key);
        }

        public bool KeyIsDown(Keys key)
        {
            return CurrKeys.IsKeyDown(key);
        }

        public bool KeyWasPressedThisFrame(Keys key)
        {
            if (CurrKeys.IsKeyDown(key) && PrevKeys.IsKeyUp(key))
                return true;
            return false;
        }
    }
}
