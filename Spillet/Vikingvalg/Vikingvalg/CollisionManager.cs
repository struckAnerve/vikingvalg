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
    public class CollisionManager : Microsoft.Xna.Framework.GameComponent, IManageCollision
    {
        IManageSprites spriteService;

        //Liste over alt p� skjermen som kan kr�sje
        private List<ICanCollideBorder> _canCollideList = new List<ICanCollideBorder>();
        //Liste over alt p� skjermen som kan kr�sje utenom den det sjekkes mot
        private List<ICanCollideBorder> _listToCheck = new List<ICanCollideBorder>();

        private Rectangle _intersectionRectangle = new Rectangle();

        public CollisionManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));

            base.Initialize();
        }

        public void Enable()
        {
            this.Enabled = true;
        }
        public void Disable()
        {
            this.Enabled = false;
        }

        public void AddCollidable(ICanCollideBorder canCollide)
        {
            if(_canCollideList.Contains(canCollide))
            {
                return;
            }
            if (canCollide == null)
            {
                Console.WriteLine("Unable to add collidable!");
                return;
            }

            _canCollideList.Add(canCollide);
            if(canCollide is ICanCollideAll) 
                _listToCheck.Add(canCollide);
        }

        public void RemoveCollidable(ICanCollideBorder toRemove)
        {
            _canCollideList.Remove(toRemove);
            if(toRemove is ICanCollideAll) 
                _listToCheck.Remove(toRemove);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (ICanCollideBorder canCollide in _canCollideList)
            {
                canCollide.BlockedTop = false;
                canCollide.BlockedRight = false;
                canCollide.BlockedBottom = false;
                canCollide.BlockedLeft = false;

                //Logikk for kollisjon mot klientvinduet
                if (canCollide.FootBox.Y <= spriteService.WalkBlockTop)
                {
                    canCollide.BlockedTop = true;
                }
                else if (canCollide.FootBox.Y + canCollide.FootBox.Height >= Game.Window.ClientBounds.Height)
                {
                    canCollide.BlockedBottom = true;
                }
                if (canCollide.FootBox.X <= 0)
                {
                    canCollide.BlockedLeft = true;
                }
                else if (canCollide.FootBox.X + canCollide.FootBox.Width >= Game.Window.ClientBounds.Width)
                {
                    canCollide.BlockedRight = true;
                }
                if (canCollide is ICanCollideAll)
                {
                    //Logikk for kollisjon mot andre objekter
                    _listToCheck.Remove(canCollide);
                    if (_listToCheck != null)
                    {
                        foreach (ICanCollideBorder canCollideTwo in _listToCheck)
                        {
                            if (canCollide.FootBox.Intersects(canCollideTwo.FootBox))
                            {
                                _intersectionRectangle = Rectangle.Intersect(canCollide.FootBox, canCollideTwo.FootBox);
                                //hvis canCollideTwo er i topp-venstre hj�rne i forhold til canCollide
                                if (canCollide.FootBox.Center.X >= canCollideTwo.FootBox.Center.X &&
                                    canCollide.FootBox.Center.Y >= canCollideTwo.FootBox.Center.Y)
                                {
                                    //hvis det er kr�sjet mer fra toppen enn fra siden
                                    if (_intersectionRectangle.Width > _intersectionRectangle.Height)
                                    {
                                        canCollide.BlockedTop = true;
                                        canCollideTwo.BlockedBottom = true;
                                    }
                                    //hvis det er kr�sjet mer fra siden enn fra toppen (eller det er kr�sjet like mye)
                                    else
                                    {
                                        canCollide.BlockedLeft = true;
                                        canCollideTwo.BlockedRight = true;
                                    }
                                }
                                //hvis canCollideTwo er i topp-h�yre hj�rne i forhold til canCollide
                                else if (canCollide.FootBox.Center.X <= canCollideTwo.FootBox.Center.X &&
                                    canCollide.FootBox.Center.Y >= canCollideTwo.FootBox.Center.Y)
                                {
                                    //hvis det er kr�sjet mer fra toppen enn fra siden
                                    if (_intersectionRectangle.Width > _intersectionRectangle.Height)
                                    {
                                        canCollide.BlockedTop = true;
                                        canCollideTwo.BlockedBottom = true;
                                    }
                                    //hvis det er kr�sjet mer fra siden enn fra toppen (eller det er kr�sjet like mye)
                                    else
                                    {
                                        canCollide.BlockedRight = true;
                                        canCollideTwo.BlockedLeft = true;
                                    }
                                }
                                //hvis canCollideTwo er i bunn-h�yre hj�rne i forhold til canCollide
                                else if (canCollide.FootBox.Center.X <= canCollideTwo.FootBox.Center.X &&
                                    canCollide.FootBox.Center.Y <= canCollideTwo.FootBox.Center.Y)
                                {
                                    //hvis det er kr�sjet mer fra bunnen enn fra siden
                                    if (_intersectionRectangle.Width > _intersectionRectangle.Height)
                                    {
                                        canCollide.BlockedBottom = true;
                                        canCollideTwo.BlockedTop = true;
                                    }
                                    //hvis det er kr�sjet mer fra siden enn fra bunnen (eller det er kr�sjet like mye)
                                    else
                                    {
                                        canCollide.BlockedRight = true;
                                        canCollideTwo.BlockedLeft = true;
                                    }
                                }
                                //hvis canCollideTwo er i bunn-venstre hj�rne i forhold til canCollide
                                else if (canCollide.FootBox.Center.X >= canCollideTwo.FootBox.Center.X &&
                                    canCollide.FootBox.Center.Y <= canCollideTwo.FootBox.Center.Y)
                                {
                                    //hvis det er kr�sjet mer fra bunnen enn fra siden
                                    if (_intersectionRectangle.Width > _intersectionRectangle.Height)
                                    {
                                        canCollide.BlockedBottom = true;
                                        canCollideTwo.BlockedTop = true;
                                    }
                                    //hvis det er kr�sjet mer fra siden enn fra bunnen (eller det er kr�sjet like mye)
                                    else
                                    {
                                        canCollide.BlockedLeft = true;
                                        canCollideTwo.BlockedRight = true;
                                    }
                                }
                                if (canCollide is Player)
                                {
                                    if (canCollideTwo is AnimatedSprite)
                                    {
                                        Player playerCollidable = (Player)canCollide;
                                        AnimatedSprite animatedSpriteColision = (AnimatedSprite)canCollideTwo;
                                        playerCollidable.ColidingWith = animatedSpriteColision;
                                    }
                                }
                            }
                        }
                    }
                    _listToCheck.Add(canCollide);
                }
            }
            base.Update(gameTime);
        }
        public void ClearCollisionList()
        {
            _canCollideList.Clear();
        }
    }
}
