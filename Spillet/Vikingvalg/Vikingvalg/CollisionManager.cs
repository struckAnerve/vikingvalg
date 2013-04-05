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
        //Liste over alt på skjermen som kan kræsje
        private List<ICanCollide> _canCollideList = new List<ICanCollide>();
        //Liste over alt på skjermen som kan kræsje utenom den det sjekkes mot
        private List<ICanCollide> _listToCheck = new List<ICanCollide>();

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

        public void AddCollidable(ICanCollide canCollide)
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
            _listToCheck.Add(canCollide);
        }

        public void RemoveCollidable(ICanCollide toRemove)
        {
            _canCollideList.Remove(toRemove);
            _listToCheck.Remove(toRemove);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (ICanCollide canCollide in _canCollideList)
            {
                canCollide.BlockedTop = false;
                canCollide.BlockedRight = false;
                canCollide.BlockedBottom = false;
                canCollide.BlockedLeft = false;

                //Logikk for kollisjon mot klientvinduet
                if (canCollide.FootBox.Y <= 0)
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

                //Logikk for kollisjon mot andre objekter
                _listToCheck.Remove(canCollide);
                if (_listToCheck != null)
                {
                    foreach (ICanCollide canCollideTwo in _listToCheck)
                    {
                        if (canCollide.FootBox.Intersects(canCollideTwo.FootBox))
                        {
                            _intersectionRectangle = Rectangle.Intersect(canCollide.FootBox, canCollideTwo.FootBox);
                            //hvis canCollideTwo er i topp-venstre hjørne i forhold til canCollide
                            if (canCollide.FootBox.Center.X >= canCollideTwo.FootBox.Center.X &&
                                canCollide.FootBox.Center.Y >= canCollideTwo.FootBox.Center.Y)
                            {
                                //hvis det er kræsjet mer fra toppen enn fra siden
                                if (_intersectionRectangle.Width > _intersectionRectangle.Height)
                                {
                                    canCollide.BlockedTop = true;
                                    canCollideTwo.BlockedBottom = true;
                                }
                                //hvis det er kræsjet mer fra siden enn fra toppen (eller det er kræsjet like mye)
                                else
                                {
                                    canCollide.BlockedLeft = true;
                                    canCollideTwo.BlockedRight = true;
                                }
                            }
                            //hvis canCollideTwo er i topp-høyre hjørne i forhold til canCollide
                            else if (canCollide.FootBox.Center.X <= canCollideTwo.FootBox.Center.X &&
                                canCollide.FootBox.Center.Y >= canCollideTwo.FootBox.Center.Y)
                            {
                                //hvis det er kræsjet mer fra toppen enn fra siden
                                if (_intersectionRectangle.Width > _intersectionRectangle.Height)
                                {
                                    canCollide.BlockedTop = true;
                                    canCollideTwo.BlockedBottom = true;
                                }
                                //hvis det er kræsjet mer fra siden enn fra toppen (eller det er kræsjet like mye)
                                else
                                {
                                    canCollide.BlockedRight = true;
                                    canCollideTwo.BlockedLeft = true;
                                }
                            }
                            //hvis canCollideTwo er i bunn-høyre hjørne i forhold til canCollide
                            else if (canCollide.FootBox.Center.X <= canCollideTwo.FootBox.Center.X &&
                                canCollide.FootBox.Center.Y <= canCollideTwo.FootBox.Center.Y)
                            {
                                //hvis det er kræsjet mer fra bunnen enn fra siden
                                if (_intersectionRectangle.Width > _intersectionRectangle.Height)
                                {
                                    canCollide.BlockedBottom = true;
                                    canCollideTwo.BlockedTop = true;
                                }
                                //hvis det er kræsjet mer fra siden enn fra bunnen (eller det er kræsjet like mye)
                                else
                                {
                                    canCollide.BlockedRight = true;
                                    canCollideTwo.BlockedLeft = true;
                                }
                            }
                            //hvis canCollideTwo er i bunn-venstre hjørne i forhold til canCollide
                            else if (canCollide.FootBox.Center.X >= canCollideTwo.FootBox.Center.X &&
                                canCollide.FootBox.Center.Y <= canCollideTwo.FootBox.Center.Y)
                            {
                                //hvis det er kræsjet mer fra bunnen enn fra siden
                                if (_intersectionRectangle.Width > _intersectionRectangle.Height)
                                {
                                    canCollide.BlockedBottom = true;
                                    canCollideTwo.BlockedTop = true;
                                }
                                //hvis det er kræsjet mer fra siden enn fra bunnen (eller det er kræsjet like mye)
                                else
                                {
                                    canCollide.BlockedLeft = true;
                                    canCollideTwo.BlockedRight = true;
                                }
                            }
                            if (canCollide is AnimatedSprite)
                            {
                                if (canCollideTwo is AnimatedSprite)
                                {
                                    AnimatedSprite animatedSpriteColision = (AnimatedSprite)canCollideTwo;
                                }
                            }   
                        }
                    }
                }
                _listToCheck.Add(canCollide);
            }

            base.Update(gameTime);
        }
    }
}
