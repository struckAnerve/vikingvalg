using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Vikingvalg
{
    /// <summary>
    /// Komponent som tar seg av kollisjon
    /// </summary>
    public class CollisionManager : GameComponent, IManageCollision
    {
        private IManageSprites _spriteService;

        //Liste over alt p� skjermen som kan kolliderer
        private List<ICanCollide> _canCollideList = new List<ICanCollide>();
        //Liste over alt p� skjermen som kan kollidere utenom den det sjekkes mot
        private List<ICanCollide> _listToCheck = new List<ICanCollide>();

        //Rektangel som gj�r det mulig � bestemme hvilken side av et objekt man kolliderer med
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
            _spriteService = (IManageSprites)Game.Services.GetService(typeof(IManageSprites));

            base.Initialize();
        }

        /// <summary>
        /// Kall p� denne kompoentens Update-funksjon
        /// </summary>
        public void Enable()
        {
            this.Enabled = true;
        }
        /// <summary>
        /// SLutt � kalle p� denne komponentens Update-funksjon
        /// </summary>
        public void Disable()
        {
            this.Enabled = false;
        }

        /// <summary>
        /// Legg til et element i listene over elementer som kan kollidere dersom listene ikke allerede inneholder elementet
        /// </summary>
        /// <param name="canCollide">Elementet du vil legge inn</param>
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

        /// <summary>
        /// Fjern et element i listene over elementer som kan kollidere
        /// </summary>
        /// <param name="toRemove"></param>
        public void RemoveCollidable(ICanCollide toRemove)
        {
            _canCollideList.Remove(toRemove);
            _listToCheck.Remove(toRemove);
        }

        /// <summary>
        /// T�mmer kollisjonssjekklistene
        /// </summary>
        public void ClearCollisionList()
        {
            _canCollideList.Clear();
            _listToCheck.Clear();
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
                if (canCollide.FootBox.Y <= _spriteService.WalkBlockTop)
                {
                    canCollide.BlockedTop = true;//kolliderer topp
                }
                else if (canCollide.FootBox.Y + canCollide.FootBox.Height >= Game.Window.ClientBounds.Height)
                {
                    canCollide.BlockedBottom = true;//kolliderer bunn
                }
                if (canCollide.FootBox.X <= 0)
                {
                    canCollide.BlockedLeft = true;//kolliderer venstre
                }
                else if (canCollide.FootBox.X + canCollide.FootBox.Width >= Game.Window.ClientBounds.Width)
                {
                    canCollide.BlockedRight = true;//kolliderer h�yre
                }
                //Logikk for kollisjon mot andre objekter. Fjerner f�rst gjeldende objekt
                _listToCheck.Remove(canCollide);
                if (_listToCheck != null)
                {
                    foreach (ICanCollide canCollideTwo in _listToCheck)
                    {
                        //hvis det oppdages en kollisjon mellom to objekter
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
                        }
                    }
                    //legger tilbake det gjeldende objektet i listen
                    _listToCheck.Add(canCollide);
                }
            }

            base.Update(gameTime);
        }
    }
}
