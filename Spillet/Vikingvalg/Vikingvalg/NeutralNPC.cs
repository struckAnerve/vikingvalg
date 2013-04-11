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
    abstract class NeutralNpc : StaticSprite, IUseInput
    {
        public InGameLevel inGameLevel;

        public String npcName;
        protected Player _player1;

        public bool inConversation;
        public DialogControl dialogController;

        protected int _talkingRangeBoxOffset = 40;
        protected int _talkingRangeBoxHeight = 40;
        protected Rectangle _talkingRangeBox;

        public NeutralNpc(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, Player player, InGameLevel inGameLevel)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth)
        {
            this.inGameLevel = inGameLevel;

            inConversation = false;
            _player1 = player;

            _talkingRangeBox = new Rectangle(destinationRectangle.Left - _talkingRangeBoxOffset, destinationRectangle.Bottom - _talkingRangeBoxHeight,
                destinationRectangle.Width + _talkingRangeBoxOffset * 2, _talkingRangeBoxHeight);

            inGameLevel.spriteService.LoadDrawable(new StaticSprite("box"));
        }

        public void Update(IManageInput inputService)
        {
            if(_player1.FootBox.Intersects(_talkingRangeBox) && _player1.FacesTowards(this._talkingRangeBox.Center.X) && inputService.KeyWasPressedThisFrame(Keys.F))
            {
                ChangeConversationState();
            }
            if (inConversation)
            {
                if (!_player1.FootBox.Intersects(_talkingRangeBox))
                {
                    ChangeConversationState();
                }
            }
        }

        public void ChangeConversationState()
        {
            inConversation = !inConversation;
        }
    }
}
