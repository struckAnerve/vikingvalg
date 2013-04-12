using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vikingvalg
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        //bestem bakgrunnsfarge
        private Color _backgroundColor = new Color(78, 48, 8, 255);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //setter bredde og høyde på spillvinduet
            graphics.PreferredBackBufferWidth = 1245;
            graphics.PreferredBackBufferHeight = 700;
            
          //her opprettes alle spillets komponenter
            //holder orden på hvilken tilstand spillet er i (pauset, i spill, i meny osv.)
            GameComponent gameStateManager = new GameStateManager(this);
            Components.Add(gameStateManager);
            Services.AddService(typeof(IManageStates), gameStateManager);

            //hovedoppgaven til SpriteManager (/IManageSprites) er å tegne alt som skal på skjermen.
            DrawableGameComponent spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            Services.AddService(typeof(IManageSprites), spriteManager);

            //har ansvaret for kollisjonssjekking
            GameComponent collisionManager = new CollisionManager(this);
            Components.Add(collisionManager);
            Services.AddService(typeof(IManageCollision), collisionManager);

            //holder orden på inn-data
            GameComponent inputManager = new InputManager(this);
            Components.Add(inputManager);
            Services.AddService(typeof(IManageInput), inputManager);

            //styrer menyen (hva som skal tegnes og oppdateres i menyene)
            GameComponent menuManager = new MenuManager(this);
            Components.Add(menuManager);
            Services.AddService(typeof(MenuManager), menuManager);

            //styrer selve spillet (oppretter levler, bestemmer hva som skal tegnes og oppdateres osv.)
            GameComponent inGameManager = new InGameManager(this);
            Components.Add(inGameManager);
            Services.AddService(typeof(InGameManager), inGameManager);

            //styrer spillets lyd
            DrawableGameComponent audioManager = new AudioManager(this);
            Components.Add(audioManager);
            Services.AddService(typeof(IManageAudio), audioManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Musen er synlig
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        { }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //tegn bakgrunnsfargen
            GraphicsDevice.Clear(_backgroundColor);
            
            base.Draw(gameTime);
        }
    }
}
