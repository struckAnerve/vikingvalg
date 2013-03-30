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

using Demina;

namespace Vikingvalg
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        //trenger vi denne (programmet kjører uten i skrivende stund, men jeg lar den være i tilfelle)?
        SpriteBatch spriteBatch;
        //Spiller av animasjon
        Vector2 playerPos = new Vector2(400, 300);


        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        Animation blockWalk;
        AnimationPlayer playerBottomAnimation = new AnimationPlayer();
        Texture2D newTexture;

<<<<<<< HEAD
        bool rotateCharacter = false;
        String playerState = "standing";
        GameComponent input;
        IHandleInput inputService;

=======
>>>>>>> 19dd482b6ab5d686729cd7bc39c41b6a00617f9c
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            DrawableGameComponent gameManager = new GameStateManager(this);
            Components.Add(gameManager);
            Services.AddService(typeof(IManageStates), gameManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
<<<<<<< HEAD
            dictionary.Add("head", "head");
            dictionary.Add("torso", "torso");
            dictionary.Add("bicep", "bicepL");
            dictionary.Add("forearmR", "forearmR");
            dictionary.Add("idleShieldArm", "idleShieldArm");
            dictionary.Add("shield", "shield");
            dictionary.Add("swordHand", "swordHand");
            dictionary.Add("thigh", "thighL");
            dictionary.Add("shin", "shinL");
            dictionary.Add("foot", "footL");
            playerBottomAnimation.PlaySpeedMultiplier = 1.5f;
            input = new InputComponent(this);
            Components.Add(input);
            Services.AddService(typeof(IHandleInput), input);
            inputService = (IHandleInput)Services.GetService(typeof(IHandleInput));
=======
>>>>>>> 19dd482b6ab5d686729cd7bc39c41b6a00617f9c
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
<<<<<<< HEAD

            // TODO: use this.Content to load your game content here
            Player p1 = new Player();
            IDrawSprites renderer = (IDrawSprites)Services.GetService(typeof(IDrawSprites));
            renderer.AddDrawable((Sprite)p1);
            addPlayerAnimations();
            newTexture = Content.Load<Texture2D>(@"Animation/torsoGreen");
=======
>>>>>>> 19dd482b6ab5d686729cd7bc39c41b6a00617f9c
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

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
<<<<<<< HEAD
            if (inputService.KeyIsDown(Keys.E) || inputService.KeyIsDown(Keys.Escape) )
            {
                this.Exit();
            }
            checkInput();

            if (inputService.KeyIsDown(Keys.F))
            {
                playerBottomAnimation.setTexture("Torso", newTexture);
                //Console.Write(playerBottomAnimation.setTexture(blockWalk, newTexture));
                /*playerPos.Y += 20;
                var fileContents = System.IO.File.ReadAllText(@"../../../../VikingvalgContent/Animation/idle.anim");
                
                fileContents = fileContents.Replace("bicep", "torso");
                playerBottomAnimation.AddAnimation("blocking", Content.Load<Animation>(@"Animation/idle"));
                System.IO.File.WriteAllText(@"../../../../VikingvalgContent/Animation/idle.anim", fileContents);
                resetAnimation();*/
            }


            


            playerBottomAnimation.Update(gameTime);
=======
>>>>>>> 19dd482b6ab5d686729cd7bc39c41b6a00617f9c
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
<<<<<<< HEAD

            // TODO: Add your drawing code here
            playerBottomAnimation.Draw(spriteBatch, playerPos, rotateCharacter, false, 0f, Color.White, new Vector2(0.5f, 0.5f), Matrix.Identity);
            //animationPlayer.Draw(spriteBatch, playerPos);
=======
            
>>>>>>> 19dd482b6ab5d686729cd7bc39c41b6a00617f9c
            base.Draw(gameTime);
        }
        public void resetAnimation()
        {
            playerBottomAnimation = new AnimationPlayer();
        }
        public void addPlayerAnimations()
        {
            blockWalk = Content.Load<Animation>(@"Animation/battleBlockWalk");
            
            playerBottomAnimation.AddAnimation("walk", blockWalk);
            playerBottomAnimation.AddAnimation("standing", Content.Load<Animation>(@"Animation/idle"));
            playerBottomAnimation.AddAnimation("strikeSword", Content.Load<Animation>(@"Animation/strikeSword"));
            playerBottomAnimation.AddAnimation("blocking", Content.Load<Animation>(@"Animation/block"));
            playerBottomAnimation.AddAnimation("slashing", Content.Load<Animation>(@"Animation/strikeSword"));
            
            playerBottomAnimation.StartAnimation("standing");
        }
        public void checkInput()
        {
            if (inputService.KeyIsDown(Keys.W))
            {
                playerPos.Y -= 2;
            }
            if (inputService.KeyIsDown(Keys.S))
            {
                playerPos.Y += 2;
            }
            if ((inputService.KeyIsDown(Keys.D) || inputService.KeyIsDown(Keys.A)) && inputService.KeyIsUp(Keys.LeftShift) && inputService.KeyIsUp(Keys.Space))
            {
                walk();
            }
            else if (inputService.KeyIsDown(Keys.Space))
            {
                attackSlash();
            }
            else if (inputService.KeyIsDown(Keys.LeftShift))
            {
                block();
            }
            else
            {
                idle();
            }
        }
        public void idle()
        {
            if (playerState != "standing")
            {
                playerBottomAnimation.TransitionToAnimation("standing", 0.2f);
                playerState = "standing";
            }
        }
        public void attackSlash()
        {
            if (playerState != "slashing")
            {
                playerBottomAnimation.TransitionToAnimation("slashing", 0.2f);
                playerState = "slashing";
            }
        }
        public void walk()
        {
            if (playerBottomAnimation.CurrentAnimation != "walking" && playerState != "walking")
            {
                playerBottomAnimation.TransitionToAnimation("walk", 0.2f);
                playerState = "walking";
            }
            if (playerState == "walking")
            {
                if (inputService.KeyIsDown(Keys.D))
                {
                    playerPos.X += 6;
                    rotateCharacter = false;
                }
                else
                {
                    playerPos.X -= 6;
                    rotateCharacter = true;
                }
            }
        }
        public void block()
        {
            if (playerState != "blocking")
            {
                playerBottomAnimation.TransitionToAnimation("blocking", 0.2f);
                playerState = "blocking";
            }
        }
    }
}
