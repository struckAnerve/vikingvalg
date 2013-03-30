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
        private List<ICanCollide> _canCollideList = new List<ICanCollide>();
        private List<ICanCollide> _listToCheck = new List<ICanCollide>();

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

        public void AddCollidable(ICanCollide canCollide)
        {
            if (canCollide == null || _canCollideList.Contains(canCollide))
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
            base.Update(gameTime);
            foreach (ICanCollide canCollide in _canCollideList)
            {
                _listToCheck.Remove(canCollide);

                canCollide.BlockedTop = false;
                canCollide.BlockedRight = false;
                canCollide.BlockedBottom = false;
                canCollide.BlockedLeft = false;

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

                if (_listToCheck != null)
                {
                    foreach (ICanCollide canCollideTwo in _listToCheck)
                    {
                        if (canCollide.FootBox.Intersects(canCollideTwo.FootBox))
                        {
                            if (canCollide.FootBox.Y - canCollideTwo.FootBox.Y >= canCollide.FootBox.Height - 6)
                            {
                                canCollide.BlockedTop = true;
                                canCollideTwo.BlockedBottom = true;
                            }
                            else if (Math.Abs(canCollide.FootBox.Y - canCollideTwo.FootBox.Y) >= canCollide.FootBox.Height - 6)
                            {
                                canCollide.BlockedBottom = true;
                                canCollideTwo.BlockedTop = true;
                            }
                            else if (canCollide.FootBox.X - canCollideTwo.FootBox.X < 0)
                            {
                                canCollide.BlockedRight = true;
                                canCollideTwo.BlockedLeft = true;
                            }
                            else 
                            {
                                canCollide.BlockedLeft = true;
                                canCollideTwo.BlockedRight = true;
                            }
                        }
                    }
                }
                _listToCheck.Add(canCollide);
            }
        }
    }
}
