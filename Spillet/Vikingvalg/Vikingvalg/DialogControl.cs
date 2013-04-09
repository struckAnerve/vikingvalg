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
    class DialogControl
    {
        protected NeutralNPC _npc;

        protected Color _boxColor;

        protected StaticSprite _npcNameBox;
        protected StaticSprite _npcTalkBox;
        protected StaticSprite _playerNameBox;
        protected StaticSprite _playerTalkBox;

        public DialogControl(NeutralNPC npc)
        {
            _npc = npc;

            _boxColor = new Color(0, 0, 0, 100);


            //Litt massive opprettelser for å legge boksene i en salgs mal
            _npcTalkBox = new StaticSprite("box", new Rectangle(40, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y-40, 520, 40),
                new Rectangle(0, 0, 520, 40), 701f, _boxColor);
            _npcNameBox = new StaticSprite("box", new Rectangle(70, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y-70, 30, 30),
                new Rectangle(0, 0, 30, 30), 701f, _boxColor);
            _playerTalkBox = new StaticSprite("box",
                new Rectangle((int)_npc.inGameLevel.spriteService.GameWindowSize.X - 560, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y - 40, 520, 40),
                new Rectangle(0, 0, 520, 40), 701f, _boxColor);
            _playerNameBox = new StaticSprite("box",
                new Rectangle((int)_npc.inGameLevel.spriteService.GameWindowSize.X - 100, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y - 70, 30, 30),
                new Rectangle(0, 0, 30, 30), 701f, _boxColor);
        }

        public void Update(IManageInput inputService)
        {

        }

        public void Enabled()
        {
            _npc.inGameLevel.AddInGameLevelDrawable(_npcNameBox);
            _npc.inGameLevel.AddInGameLevelDrawable(_npcTalkBox);
            _npc.inGameLevel.AddInGameLevelDrawable(_playerNameBox);
            _npc.inGameLevel.AddInGameLevelDrawable(_playerTalkBox);
        }
        public void Disabled()
        {
            _npc.inGameLevel.RemoveInGameLevelDrawable(_npcNameBox);
            _npc.inGameLevel.RemoveInGameLevelDrawable(_npcTalkBox);
            _npc.inGameLevel.RemoveInGameLevelDrawable(_playerNameBox);
            _npc.inGameLevel.RemoveInGameLevelDrawable(_playerTalkBox);
        }
    }
}
