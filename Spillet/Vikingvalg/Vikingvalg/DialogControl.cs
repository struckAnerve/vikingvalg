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
        protected NeutralNpc _npc;

        protected Color _boxColor;
        protected int _lineHeight;
        protected int _maxTextWidth = 500;

        public StaticSprite npcNameBox;
        public StaticSprite npcTalkBox;
        public StaticSprite playerNameBox;
        public StaticSprite playerTalkBox;

        private int _textOffsetY = 7;
        private int _npcTalkBoxLines = 0;
        private int _playerTalkBoxLines = 0;
        private int _talkBoxLines = 0;

        public Vector2 npcNamePos;
        public Vector2 playerNamePos;

        public String npcSays;
        public Vector2 npcSaysPos = Vector2.Zero;
        public List<PlayerTextAnswer> playerAnswers = new List<PlayerTextAnswer>();
        private Color _defaultAnswerColor = new Color(100, 100, 100, 255);
        private Color _hoveredAnswerColor = new Color(255, 255, 255, 255);

        public DialogControl(NeutralNpc npc)
        {
            _npc = npc;

            _boxColor = new Color(0, 0, 0, 100);
            _lineHeight = (int)_npc.inGameLevel.spriteService.TextSize("H").Y+12;

            //Litt massive opprettelser for å legge boksene i en salgs mal. Source har ikke noe å si?
            npcTalkBox = new StaticSprite("box", new Rectangle(40, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y, _maxTextWidth + 20, 0),
                new Rectangle(0, 0, 1, 1), 1001f, _boxColor);
            npcNameBox = new StaticSprite("box", new Rectangle(70, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y-(_lineHeight),
                (int)_npc.inGameLevel.spriteService.TextSize(_npc.npcName).X + 10, _lineHeight), new Rectangle(0, 0, 1, 1), 1001f, _boxColor);
            playerTalkBox = new StaticSprite("box",
                new Rectangle((int)_npc.inGameLevel.spriteService.GameWindowSize.X - 560, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y, _maxTextWidth + 20, 0),
                new Rectangle(0, 0, 1, 1), 1001f, _boxColor);
            playerNameBox = new StaticSprite("box",
                new Rectangle((int)_npc.inGameLevel.spriteService.GameWindowSize.X - 105, (int)_npc.inGameLevel.spriteService.GameWindowSize.Y - (_lineHeight), 35, _lineHeight),
                new Rectangle(0, 0, 1, 1), 1001f, _boxColor);

            npcSaysPos.X = npcTalkBox.DestinationX + 10;
            npcSaysPos.Y = npcTalkBox.DestinationY + _textOffsetY;

            npcNamePos = new Vector2(npcNameBox.DestinationX + 5, npcNameBox.DestinationY + 6);
            playerNamePos = new Vector2(playerNameBox.DestinationX + 5, playerNameBox.DestinationY + 6);
        }

        public void Update(IManageInput inputService)
        {
            foreach (PlayerTextAnswer answer in playerAnswers)
            {
                if (!answer.hovered && answer.answerBox.Contains(inputService.CurrMouse.X, inputService.CurrMouse.Y))
                {
                    answer.hovered = true;
                    answer.textColor = _hoveredAnswerColor;
                }
                else if(answer.hovered && !answer.answerBox.Contains(inputService.CurrMouse.X, inputService.CurrMouse.Y))
                {
                    answer.hovered = false;
                    answer.textColor = _defaultAnswerColor;
                }

                if(answer.hovered && inputService.MouseWasPressedThisFrame("left"))
                {
                    _npc.AnswerClicked(answer);
                    break;
                }
            }
        }

        public void ChangeNpcDialog(String changeTo)
        {
            bool wasHighestBox = false;
            if (_npcTalkBoxLines == _talkBoxLines)
                wasHighestBox = true;

            npcSays = _npc.inGameLevel.spriteService.WrapText(changeTo, _maxTextWidth);

            _npcTalkBoxLines = 1;
            //funnet her: http://stackoverflow.com/questions/541954/how-would-you-count-occurences-of-a-string-within-a-string-c
            _npcTalkBoxLines += npcSays.Length - npcSays.Replace("\n", "").Length;

            //Teksten er størst
            if (_npcTalkBoxLines > _playerTalkBoxLines && _npcTalkBoxLines > _talkBoxLines)
            {
                ChangeHeight(_npcTalkBoxLines - _talkBoxLines, true);
                _talkBoxLines = _npcTalkBoxLines;
            }
            //Teksten var den største, og er mindre enn før
            else if (_npcTalkBoxLines < _talkBoxLines && wasHighestBox)
            {
                if (_npcTalkBoxLines > _playerTalkBoxLines)
                {
                    ChangeHeight(_talkBoxLines - _npcTalkBoxLines, false);
                    _talkBoxLines = _npcTalkBoxLines;
                }
                else
                {
                    ChangeHeight(_talkBoxLines - _playerTalkBoxLines, false);
                    _talkBoxLines = _playerTalkBoxLines;
                }
            }
        }
        public void AddPlayerAnswer(String answerToAdd, String answerDesc)
        {
            answerToAdd = _npc.inGameLevel.spriteService.WrapText(" - " + answerToAdd, _maxTextWidth);

            int numOfLines = 1;
            //funnet her: http://stackoverflow.com/questions/541954/how-would-you-count-occurences-of-a-string-within-a-string-c
            numOfLines += answerToAdd.Length - answerToAdd.Replace("\n", "").Length;

            if (_playerTalkBoxLines + numOfLines > _talkBoxLines)
            {
                ChangeHeight(numOfLines, true);
            }

            playerAnswers.Add(new PlayerTextAnswer(answerToAdd, answerDesc, new Rectangle(playerTalkBox.DestinationX + 10,
                playerTalkBox.DestinationRectangle.Top + _textOffsetY + (_lineHeight*_playerTalkBoxLines), 500, _lineHeight), _defaultAnswerColor));
            _playerTalkBoxLines += numOfLines;
        }
        public void RemovePlayerAnswers()
        {
            bool wasHighestBox = false;
            if (_playerTalkBoxLines == _talkBoxLines)
                wasHighestBox = true;

            playerAnswers.Clear();
            _playerTalkBoxLines = 0;

            if (_playerTalkBoxLines < _talkBoxLines && wasHighestBox)
            {
                ChangeHeight(_talkBoxLines - _npcTalkBoxLines, false);
                _talkBoxLines = _npcTalkBoxLines;
            }
        }

        private void ChangeHeight(int numOfLines, bool bigger)
        {
            int heightChange = _lineHeight * numOfLines;
            if (!bigger)
                heightChange *= -1;

            npcNameBox.DestinationY -= heightChange;
            npcNamePos.Y = npcNameBox.DestinationY + 6;
            npcTalkBox.DestinationY -= heightChange;
            npcTalkBox.DestinationHeight += heightChange;
            npcSaysPos.Y = npcTalkBox.DestinationY + _textOffsetY;
            playerNameBox.DestinationY -= heightChange;
            playerNamePos.Y = playerNameBox.DestinationY + 6;
            playerTalkBox.DestinationY -= heightChange;
            playerTalkBox.DestinationHeight += heightChange;
            foreach (PlayerTextAnswer playerAnswer in playerAnswers)
            {
                playerAnswer.answerBox.Y -= heightChange;
            }
        }
    }
}