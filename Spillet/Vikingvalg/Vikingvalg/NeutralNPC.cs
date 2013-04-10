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
    class NeutralNPC : StaticSprite, IUseInput
    {
        public InGameLevel inGameLevel;

        protected Player _player1;

        public String npcName;
        protected bool _inConversation;
        protected DialogControl _dialogController;

        protected int _talkingRangeBoxOffset = 40;
        protected int _talkingRangeBoxHeight = 40;
        protected Rectangle _talkingRangeBox;

        public NeutralNPC(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, String npcName, Player player, InGameLevel inGameLevel)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            this.inGameLevel = inGameLevel;

            _inConversation = false;
            this.npcName = npcName;
            _player1 = player;

            _talkingRangeBox = new Rectangle(destinationRectangle.Left - _talkingRangeBoxOffset, destinationRectangle.Bottom - _talkingRangeBoxHeight,
                destinationRectangle.Width + _talkingRangeBoxOffset * 2, _talkingRangeBoxHeight);

            inGameLevel.spriteService.LoadDrawable(new StaticSprite("box"));
            _dialogController = new DialogControl(this);
        }
        public NeutralNPC(String artName, Rectangle destinationRectangle, String npcName, Player player, InGameLevel inGameLevel)
            : this(artName, destinationRectangle, new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height),
                new Color(255, 255, 255, 255), 0, Vector2.Zero, SpriteEffects.None, destinationRectangle.Bottom, npcName, player, inGameLevel)
        { }

        public void Update(IManageInput inputService)
        {
            if(_player1.FootBox.Intersects(_talkingRangeBox) && _player1.FacesTowards(this._talkingRangeBox.Center.X) && inputService.KeyWasPressedThisFrame(Keys.F))
            {
                ChangeConversationState();
            }
            if (_inConversation)
            {
                Console.Write("hei, ");
                if (!_player1.FootBox.Intersects(_talkingRangeBox))
                {
                    ChangeConversationState();
                }
            }
        }

        public void ChangeConversationState()
        {
            _inConversation = !_inConversation;


            //if for å sjekke om man skal legge til tekstboks eller fjerne
            if (_inConversation)
            {
                _dialogController.Enabled();
            }
            else
            {
                _dialogController.Disabled();
            }
        }
    }
}
