using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Stein
    /// </summary>
    class Stone : StaticSprite, ICanCollide, IPlaySound
    {
        //Animasjonen som spilles av når man slår på en stein
        public AnimatedStaticSprite stoneHitArt { get; set; }

        //steinens kollisjonsboks
        private Rectangle _footBox;
        public Rectangle FootBox
        {
            get { return _footBox; }
            private set { }
        }

        //Brukes ikke (men trengs for ICanCollide)
        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        //random-klasse
        private static Random _rand = new Random();
        private static readonly object syncLock = new object();

        //spilleren
        private Player _player1;

        //lydkontroller
        public IManageAudio _audioManager { get; set; }
        public String Directory { get; set; }

        //hvor mye en stein tåler
        public int endurance;

        //om steinen har gull
        private bool _hasGold;

        public Stone(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, bool hasGold, Game game, Player player1)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            _player1 = player1;
            stoneHitArt = new AnimatedStaticSprite("stonehit", new Rectangle(_destinationRectangle.X - 40, _destinationRectangle.Y - ((99-_destinationRectangle.Height)/2), 180, 99), Vector2.Zero, 4, 50, true);
            stoneHitArt.IsPlaying = false;
            endurance = 8;
            _footBox = new Rectangle(destinationRectangle.X, destinationRectangle.Bottom - 20, destinationRectangle.Width, 20);
            setLayerDepth(_footBox.Bottom);
            _hasGold = hasGold;
            _audioManager = (IManageAudio)game.Services.GetService(typeof(IManageAudio));
            Directory = "stone";
        }
        public Stone(Rectangle destinationRectangle, int sourceYPos, int color, bool hasGold, Game game, Player player1)
            : this("stone", destinationRectangle, new Rectangle(0, sourceYPos, destinationRectangle.Width, destinationRectangle.Height),
                new Color(200, color + 30, color, 255), 0, Vector2.Zero, SpriteEffects.None, destinationRectangle.Bottom, hasGold, game, player1)
        { }

        //Når steinen slås
        public void IsHit()
        {
            //spill av truffet-animasjon
            stoneHitArt.currentFrame = 0;
            stoneHitArt.IsPlaying = true;
            //mist styrke
            endurance--;
            //om steinen knuses
            if (endurance <= 0)
            {
                //om steinen har gull
                if (_hasGold)
                {
                    //bytt source, spill lyd, gi penger 
                    stoneHitArt.ChangeYPosition(198);
                    _sourceRectangle.X = 500;
                    _audioManager.AddSound(Directory + "/money");
                    _player1.addMoney(rInt(0, 10) * _player1.combatLevel);
                }
                //om steinen ikke har gull
                else
                {
                    //bytt source, spill lyd
                    stoneHitArt.ChangeYPosition(99);
                    _sourceRectangle.X = 400;
                    _audioManager.AddSound(Directory+"/crumble");
                }
            }
            //bytt source annethvert slag
            else if (endurance % 2 == 0)
            {
                _sourceRectangle.X += _sourceRectangle.Width;
            }
        }

        //for å passe på at randomen blir random
        public static int rInt(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return _rand.Next(min, max);
            }
        }
    }
}
