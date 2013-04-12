using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Demina;

namespace Vikingvalg
{
    /// <summary>
    /// hovedoppgaven til SpriteManager (/IManageSprites) er � tegne alt som skal p� skjermen. Tar seg ogs� av innlasting 
    /// og � distribuere globale st�rrelser
    /// </summary>
    public class SpriteManager : DrawableGameComponent, IManageSprites
    {
        //St�rrelsen p� vinduet representert med en Vector2 (som dessverre betyr at x = width og y = height)
        public Vector2 GameWindowSize { get; protected set; }
        //et tall som representerer margen fra toppen av vinduet og ned til der hvor spilleren kan g�
        public int WalkBlockTop { get; set; }

        //boolean som bestemmer om karakterers liv skal tegnes eller ikke
        public bool DrawHealthBar { get; set; }

        private SpriteBatch _spriteBatch;
        //en dictionary med innlastede Texture2Der (n�kkelen er navnet p� filen den har lastet inn)
        private Dictionary<String, Texture2D> _loadedStaticArt = new Dictionary<String,Texture2D>();

        //SpriteFont for Arial
        private SpriteFont _arialFont;
        //Standard tekstfarge
        private Color _defaultFontColor = new Color(255, 255, 255, 255);
        //spillerens dybde p� skjermen (han er dypere jo h�yere "opp" han g�r)
        private float playerDepth;

        //en liste over listene med Sprites som skal tegnes. Denne inneholder til enhver tid det aller meste som skal tegnes
        public List<List<Sprite>> ListsToDraw { get; set; }
        //en sortert (p� layer depth) liste med Sprites. M� til for at Sprites skal tegnes i riktig rekkef�lge (vi m�tte gj�re
        //det slik pga. Demina)
        private List<Sprite> sortedList;

        public SpriteManager(Game game)
            : base(game)
        {
            ListsToDraw = new List<List<Sprite>>();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            //laster inn Arial og setter linjeh�yde
            _arialFont = Game.Content.Load<SpriteFont>("arial");
            _arialFont.LineSpacing = 22;

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //finner vindusst�rrelsen og legger den i en property for � gj�re den tilgjengelig flere steder
            GameWindowSize = new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            //toppmargin (spiller kan ikke g� h�yere enn dette)
            WalkBlockTop = 170;

            base.Initialize();
        }

        /// <summary>
        /// Laster inn en Texture2D fra en Sprite, og legger den i en liste. Vi tenkte at det ville v�re en god id� � samle all
        /// innlasting i denne komponenten.
        /// </summary>
        /// <param name="toLoad">Spriten man �nsker � laste</param>
        public void LoadDrawable(Sprite toLoad)
        {
            //skiller mellom StaticSprite og AnimatedSprite
            if (toLoad is StaticSprite)
            {
                StaticSprite staticElement = (StaticSprite)toLoad;
                //laster inn en Texture2D og legger den i en liste dersom den ikke ligger i listen fra f�r
                if (!(_loadedStaticArt.ContainsKey(staticElement.ArtName)))
                {
                    _loadedStaticArt.Add(staticElement.ArtName, Game.Content.Load<Texture2D>(staticElement.ArtName));
                }
            }
            else if (toLoad is AnimatedSprite)
            {
                AnimatedSprite drawableAnimation = (AnimatedSprite)toLoad;
                //laster inn animasjoner utifra en liste i AnimatedSprite, legger dem til i animationPlayer (Demina)
                foreach (String animationName in drawableAnimation.animationList)
                {
                    drawableAnimation.animationPlayer.AddAnimation(animationName, Game.Content.Load<Animation>(@"Animations/" + drawableAnimation.Directory + "Animation/" + animationName));
                }
                //bestemmer startanimasjon (Demina)
                drawableAnimation.animationPlayer.StartAnimation("idle");
            }
        }

        /// <summary>
        /// Returnerer en Texture2D utifra navnet (laster den inn og legger den inn i en liste dersom den ikke ligger der fra f�r)
        /// </summary>
        /// <param name="artName">Navnet p� filen du �nsker � laste inn som en Texture2D og f� returnert</param>
        /// <returns>Texture2Den som er lagret som artName</returns>
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
            //for hver liste i lister som skal tegnes
            foreach (List<Sprite> listToDraw in ListsToDraw)
            {
                //sorterer listen
                sortedList = listToDraw.OrderBy(x => x.LayerDepth).ToList();
                //finner spillerens dybde
                foreach (Sprite spr in sortedList)
                {
                    if (spr is Player)
                        playerDepth = spr.LayerDepth;
                }
                //tegn bak spiller
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                //for hver Sprite i listen
                foreach (Sprite drawable in sortedList)
                {
                    if (drawable is StaticSprite && drawable.LayerDepth <= playerDepth)
                    {
                        //om elementet er Stone skal det sjekkes "steinspruten" skal tegnes
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
                    if (drawable is AnimatedSprite)
                    {
                        AnimatedSprite drawableAnimation = (AnimatedSprite)drawable;
                        drawAnimatedSprite(drawableAnimation);
                    }
                }
                //tegn foran spiller
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                foreach (Sprite drawable in sortedList)
                {
                    //Her tegnes teksten som forteller om xp, gull og level
                    if (drawable is Player)
                    {
                        Player player1 = (Player) drawable;
                        DrawSpriteFont("Combat level: " + player1.combatLevel.ToString(), new Vector2(10, 5));
                        DrawSpriteFont("Experience: " + player1.totalXP.ToString(), new Vector2(10, 25));
                        DrawSpriteFont("Gold: " + player1.totalGold.ToString(), new Vector2(10, 45));
                    }
                    if (drawable is StaticSprite)
                    {
                        if (drawable.LayerDepth > playerDepth)
                        {
                            //om elementet er Stone skal det sjekkes "steinspruten" skal tegnes
                            if (drawable is Stone)
                            {
                                Stone stoneToDraw = (Stone)drawable;
                                if (stoneToDraw.stoneHitArt.IsPlaying)
                                    DrawStaticSprite(stoneToDraw.stoneHitArt);
                            }
                            DrawStaticSprite(drawable);
                        }
                        //om drawable er NeutralNpc skal det sjekkes om man skal tegne samtaletekst og boksene som ligger rundt teksten
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
                    //sjekker om man skal tegne karakterers liv
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

        /// <summary>
        /// Funskjonen som blir kalt dersom man skal tegne en StaticSprite
        /// </summary>
        /// <param name="drawable">Spriten man skal tegne</param>
        private void DrawStaticSprite(Sprite drawable)
        {
            StaticSprite staticDrawableSprite = (StaticSprite)drawable;
            _spriteBatch.Draw(_loadedStaticArt[staticDrawableSprite.ArtName], staticDrawableSprite.DestinationRectangle,
                staticDrawableSprite.SourceRectangle, staticDrawableSprite.Color, staticDrawableSprite.Rotation,
                staticDrawableSprite.Origin, staticDrawableSprite.Effects, 1f);
        }

        /// <summary>
        /// Funksjonen som blir kalt dersom man skal tegne en AnimatedSprite
        /// </summary>
        /// <param name="drawableAnimation">AnimatedSprtien som skal tegnes</param>
        private void drawAnimatedSprite(AnimatedSprite drawableAnimation)
        {
            //De neste to linjene er lagt til fordi Karl Gustav Georgsen var dum og tegnet ulven feil vei.
            bool animFlip = drawableAnimation.Flipped;
            if (drawableAnimation is WolfEnemy) animFlip = !drawableAnimation.Flipped;
            drawableAnimation.animationPlayer.Draw(_spriteBatch, drawableAnimation.DestinationRectangle, animFlip, drawableAnimation.Rotation, drawableAnimation.Scale);
        }

        /// <summary>
        /// Funskjonen som blir kalt dersom tekst skal tegnes
        /// </summary>
        /// <param name="toDraw">Teksten som skal tegnes</param>
        /// <param name="whereToDraw">Hvor teksten skal tegnes</param>
        private void DrawSpriteFont(String toDraw, Vector2 whereToDraw)
        {
            _spriteBatch.DrawString(_arialFont, toDraw, whereToDraw, _defaultFontColor);
        }

        /// <summary>
        /// En teksttegnefunksjon som tar imot en farge i tillegg
        /// </summary>
        /// <param name="toDraw">Teksten som skal tegnes</param>
        /// <param name="whereToDraw">Hvor teksten skal tegnes</param>
        /// <param name="color">Tekstfargen</param>
        private void DrawSpriteFont(String toDraw, Vector2 whereToDraw, Color color)
        {
            _spriteBatch.DrawString(_arialFont, toDraw, whereToDraw, color);
        }

        /// <summary>
        /// M�ler og returnerer bredden og h�yden p� en streng
        /// </summary>
        /// <param name="text">Teksten du vil m�le</param>
        /// <returns>Returnerer bredd og h�yde representert ved en Vector2, x = width y = height</returns>
        public Vector2 TextSize(String text)
        {
            return _arialFont.MeasureString(text);
        }

        //funksjon fra http://www.xnawiki.com/index.php/Basic_Word_Wrapping
        /// <summary>
        /// Funskjon som returnerer en "oppdelt" streng basert p� en gitt maksbredde
        /// </summary>
        /// <param name="text">Teksten du vil dele opp</param>
        /// <param name="maxLineWidth">Maks bredde</param>
        /// <returns>Returnerer en streng med \n der teksten m� deles opp for � passe inn i maksbredden</returns>
        public string WrapText(string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = TextSize(" ").X;

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
