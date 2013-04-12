using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Vikingvalg
{
    /// <summary>
    /// Behandler inn-data
    /// </summary>
    public class InputManager : GameComponent, IManageInput
    {
        //tastaturets forrige og nåværende tilstand
        private KeyboardState PrevKeys { get; set; }
        private KeyboardState CurrKeys { get; set; }

        //musens forrige og nåværende tilstand
        public MouseState PrevMouse { get; private set; }
        public MouseState CurrMouse { get; private set; }

        public InputManager(Game game)
            : base(game)
        { }

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

            //opdaterer inndata-tilstandene
            PrevKeys = CurrKeys;
            CurrKeys = Keyboard.GetState();
            PrevMouse = CurrMouse;
            CurrMouse = Mouse.GetState();
        }

        /// <summary>
        /// Sjekker om gitt knapp ikke er trykket
        /// </summary>
        /// <param name="key">Knapp å sjekke</param>
        /// <returns>true hvis knappen ikke er trykket</returns>
        public bool KeyIsUp(Keys key)
        {
            return CurrKeys.IsKeyUp(key);
        }

        /// <summary>
        /// Sjekker om gitt knapp er trykker
        /// </summary>
        /// <param name="key">Knapp å sjekke</param>
        /// <returns>true hvis knappen er trykket</returns>
        public bool KeyIsDown(Keys key)
        {
            return CurrKeys.IsKeyDown(key);
        }

        /// <summary>
        /// Sjekker om knappen er trykket på i denne framen. Nyttig om man bare skal sjekke et trykk (ikke at det holdes inne)
        /// </summary>
        /// <param name="key">Knapp å sjekke</param>
        /// <returns>true hvis knappen er trykket på i denne framen</returns>
        public bool KeyWasPressedThisFrame(Keys key)
        {
            if (CurrKeys.IsKeyDown(key) && PrevKeys.IsKeyUp(key))
                return true;
            return false;
        }

        /// <summary>
        /// Sjekker om museknappen er trykket på i denne framen. Nyttig om man bare skal sjekke et trykk (ikke at det holdes inne)
        /// </summary>
        /// <param name="button">Knapp å sjekke (left/right/middle)</param>
        /// <returns>true dersom knappen ble trykket inn i denne framen</returns>
        public bool MouseWasPressedThisFrame(String button)
        {
            if (button == "left" && CurrMouse.LeftButton == ButtonState.Pressed && PrevMouse.LeftButton == ButtonState.Released) return true;
            if (button == "right" && CurrMouse.RightButton == ButtonState.Pressed && PrevMouse.RightButton == ButtonState.Released) return true;
            if (button == "middle" && CurrMouse.MiddleButton == ButtonState.Pressed && PrevMouse.MiddleButton == ButtonState.Released) return true;
            return false;
        }
    }
}
