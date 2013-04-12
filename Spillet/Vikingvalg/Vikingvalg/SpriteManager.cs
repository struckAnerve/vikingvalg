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
using System.Text;

using Demina;
namespace Vikingvalg
{
    public class SpriteManager : DrawableGameComponent, IManageSprites
    {
        //Skal kanskje ikke stå her? Brukes flere steder gjennom SpriteManager
        public Vector2 GameWindowSize { get; protected set; }
        public int WalkBlockTop { get; set; }

        public bool DrawHealthBar { get; set; }

        private SpriteBatch _spriteBatch;
        private Dictionary<String, Texture2D> _loadedStaticArt = new Dictionary<String,Texture2D>();

        //Midlertidig
        Texture2D smallthing;

        private SpriteFont _arialFont;
        private Color _defaultFontColor = new Color(255, 255, 255, 255);
        private float playerDepth;
        public List<List<Sprite>> ListsToDraw { get; set; }
        private List<Sprite> sortedList;
        public SpriteManager(Game game)
            : base(game)
        {
            ListsToDraw = new List<List<Sprite>>();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            _arialFont = Game.Content.Load<SpriteFont>("arial");
            _arialFont.LineSpacing = 22;

            //midlertidig
            smallthing = Game.Content.Load<Texture2D>(@"redPixel");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            ListsToDraw = new List<List<Sprite>>();
            GameWindowSize = new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            WalkBlockTop = 170;
            base.Initialize();
        }
        public void LoadDrawable(Sprite toLoad)
        {
            if (toLoad is StaticSprite)
            {
                StaticSprite staticElement = (StaticSprite)toLoad;
                if (!(_loadedStaticArt.ContainsKey(staticElement.ArtName)) && toLoad != null)
                {
                    _loadedStaticArt.Add(staticElement.ArtName, Game.Content.Load<Texture2D>(staticElement.ArtName));
                }
            }
            else if (toLoad is AnimatedSprite)
            {
                AnimatedSprite drawableAnimation = (AnimatedSprite)toLoad;
                foreach (String animationName in drawableAnimation.animationList)
                {
                    drawableAnimation.animationPlayer.AddAnimation(animationName, Game.Content.Load<Animation>(@"Animations/" + drawableAnimation.Directory + "Animation/" + animationName));
                }

                drawableAnimation.animationPlayer.StartAnimation("idle");
            }
        }
        public Texture2D LoadTexture2D(String artName)
        {
            if (!_loadedStaticArt.ContainsKey(artName))
            {
                _loadedStaticArt.Add(artName, Game.Content.Load<Texture2D>(artName));
            }
            return _loadedStaticArt[artName];
        }
            

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (List<Sprite> listToDraw in ListsToDraw)
            {
                sortedList = listToDraw.OrderBy(x => x.LayerDepth).ToList();
                foreach (Sprite spr in sortedList)
                {
                    if (spr is Player)
                        playerDepth = spr.LayerDepth;
                }
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                foreach (Sprite drawable in sortedList)
                {
                    //tegn bak spiller
                    if (drawable is StaticSprite && drawable.LayerDepth <= playerDepth)
                    {
                        if (drawable is Stone)
                        {
                            Stone stoneToDraw = (Stone)drawable;
                            if(stoneToDraw.stoneHitArt.IsPlaying)
                                DrawStaticSprite(stoneToDraw.stoneHitArt);
                        }
                        DrawStaticSprite(drawable);
                    }
                }
                _spriteBatch.End();

                _spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                foreach (Sprite drawable in sortedList)
                {
                    drawAnimatedSprite(drawable);
                }
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                foreach (Sprite drawable in sortedList)
                {

                    if (drawable is Player)
                    {
                        Player _player1 = (Player) drawable;
                        DrawSpriteFont("Combat level: "+_player1.combatLevel.ToString(), new Vector2(10, 5));
                        DrawSpriteFont("Experience: " + _player1.totalXP.ToString(), new Vector2(10, 25));
                        DrawSpriteFont("Gold: " + _player1.totalGold.ToString(), new Vector2(10, 45));
                    }
                    //tegn foran spiller
                    if (drawable is StaticSprite)
                    {
                        if (drawable.LayerDepth > playerDepth)
                        {
                            if (drawable is Stone)
                            {
                                Stone stoneToDraw = (Stone)drawable;
                                if (stoneToDraw.stoneHitArt.IsPlaying)
                                    DrawStaticSprite(stoneToDraw.stoneHitArt);
                            }
                            DrawStaticSprite(drawable);
                        }
                        if (drawable is NeutralNpc)
                        {
                            NeutralNpc conversationNpc = (NeutralNpc)drawable;
                            if (conversationNpc.inConversation)
                            {
                                //tegn dialogboks
                                DrawStaticSprite(conversationNpc.dialogController.npcNameBox);
                                DrawStaticSprite(conversationNpc.dialogController.npcTalkBox);
                                DrawStaticSprite(conversationNpc.dialogController.playerNameBox);
                                DrawStaticSprite(conversationNpc.dialogController.playerTalkBox);

                                //tegn dialogtekst
                                DrawSpriteFont(conversationNpc.npcName, conversationNpc.dialogController.npcNamePos);
                                DrawSpriteFont(conversationNpc.dialogController.npcSays, conversationNpc.dialogController.npcSaysPos);
                                DrawSpriteFont("You", conversationNpc.dialogController.playerNamePos);
                                foreach (PlayerTextAnswer playerAnswer in conversationNpc.dialogController.playerAnswers)
                                {
                                    DrawSpriteFont(playerAnswer.answer, playerAnswer.AnswerBoxLocation, playerAnswer.textColor);
                                }
                            }
                        }
                    }
                    else if (drawable is AnimatedCharacter && DrawHealthBar)
                    {
                        AnimatedCharacter drawableCharacter = (AnimatedCharacter)drawable;
                        if (drawableCharacter.activeEnemy == drawableCharacter || drawableCharacter is Player)
                        {
                            DrawStaticSprite(drawableCharacter.healthbar.healthBarSprite);
                            DrawStaticSprite(drawableCharacter.healthbar.healthContainerSprite);
                        }
                    }
                }
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void DrawBoxPerimeter(Rectangle box)
        {
            _spriteBatch.Draw(smallthing, new Vector2(box.X, box.Y), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X + box.Width, box.Y + box.Height), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X + box.Width / 2, box.Y + box.Height), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X + box.Width, box.Y), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X + box.Width / 2, box.Y), Color.White);
            _spriteBatch.Draw(smallthing, new Vector2(box.X, box.Y + box.Height), Color.White);
        }
        private void DrawStaticSprite(Sprite drawable)
        {
            StaticSprite staticDrawableSprite = (StaticSprite)drawable;
            _spriteBatch.Draw(_loadedStaticArt[staticDrawableSprite.ArtName], staticDrawableSprite.DestinationRectangle,
                staticDrawableSprite.SourceRectangle, staticDrawableSprite.Color, staticDrawableSprite.Rotation,
                staticDrawableSprite.Origin, staticDrawableSprite.Effects, 1f);
        }
        private void drawAnimatedSprite(Sprite drawable)
        {
            if (drawable is AnimatedSprite)
            {
                AnimatedSprite drawableAnimation = (AnimatedSprite)drawable;
                //De neste to linjene er lagt til fordi Karl Gustav Georgsen var dum og tegnet ulven feil vei.
                bool animFlip = drawableAnimation.Flipped;
                if (drawableAnimation is WolfEnemy) animFlip = !drawableAnimation.Flipped;
                drawableAnimation.animationPlayer.Draw(_spriteBatch, drawableAnimation.DestinationRectangle, animFlip, drawableAnimation.Rotation, drawableAnimation.Scale);
                /*
                //Husk å fjerne
                if (drawableAnimation is AnimatedCharacter)
                {
                    _spriteBatch.Begin();
                    AnimatedCharacter p1 = (AnimatedCharacter)drawableAnimation;
                    DrawBoxPerimeter(p1.FootBox);
                    _spriteBatch.End();
                } 
                */
            }
        }
        private void DrawSpriteFont(String toDraw, Vector2 whereToDraw)
        {
            _spriteBatch.DrawString(_arialFont, toDraw, whereToDraw, _defaultFontColor);
        }
        private void DrawSpriteFont(String toDraw, Vector2 whereToDraw, Color color)
        {
            _spriteBatch.DrawString(_arialFont, toDraw, whereToDraw, color);
        }

        public Vector2 TextSize(String text)
        {
            return _arialFont.MeasureString(text);
        }

        //funksjon fra http://www.xnawiki.com/index.php/Basic_Word_Wrapping
        public string WrapText(string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = _arialFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = _arialFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
    }
}
