using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Vikingvalg
{
    /// <summary>
    /// Holder kontroll over størrelser på dialogbokser, og tekst (posisjon, hva som skal sies osv)
    /// </summary>
    class DialogControl
    {
        //NPCen dette objektet hører til
        protected NeutralNpc _npc;

        //fargen på dialogboksen
        protected Color _boxColor;
        //høyde på en linje tekst
        protected int _lineHeight;
        //maks tekstbredde (piksler)
        protected int _maxTextWidth = 500;

        //dialogbokser og navnebokser
        public StaticSprite npcNameBox;
        public StaticSprite npcTalkBox;
        public StaticSprite playerNameBox;
        public StaticSprite playerTalkBox;

        //tekstmargin i forhold til boksene
        private int _textOffsetY = 7;
        //hvor mange tekstlinjer NPCen har i sin dialogboks
        private int _npcTalkBoxLines = 0;
        //hvor mange tekstlinjer spilleren har i sin dialogboks
        private int _playerTalkBoxLines = 0;
        //representerer tekstlinjene i boksen med flest tekstlinjer
        private int _talkBoxLines = 0;

        //posisjonen på navnene
        public Vector2 npcNamePos;
        public Vector2 playerNamePos;

        //teksten til NPCen
        public String npcSays;
        //posisjonen på teksten til NPCen
        public Vector2 npcSaysPos = Vector2.Zero;
        //Liste over hvert av spillerens svaralternativer
        public List<PlayerTextAnswer> playerAnswers = new List<PlayerTextAnswer>();
        //standradfarge på tekst
        private Color _defaultAnswerColor = new Color(100, 100, 100, 255);
        //farge på hovered tekst
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
                //sjekker om et svaralternativ er hovret eller ikke
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

                //hvis svaralternativet klikkes
                if(answer.hovered && inputService.MouseWasPressedThisFrame("left"))
                {
                    _npc.AnswerClicked(answer);
                    break;
                }
            }
        }

        /// <summary>
        /// Endre teksten som NPCen skal si
        /// </summary>
        /// <param name="changeTo">teksten du vil endre til</param>
        public void ChangeNpcDialog(String changeTo)
        {
            //sjekker om NPCens dialogboks er den høyeste
            bool wasHighestBox = false;
            if (_npcTalkBoxLines == _talkBoxLines)
                wasHighestBox = true;

            //legger inn \n hvis changeTo er bredere enn maksbredden
            npcSays = _npc.inGameLevel.spriteService.WrapText(changeTo, _maxTextWidth);

            //_npcTalkBoxLines = 1 + hver \n i teksten
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

        /// <summary>
        /// Legg til et svaralternativ for spilleren
        /// </summary>
        /// <param name="answerToAdd"></param>
        /// <param name="answerDesc"></param>
        public void AddPlayerAnswer(String answerToAdd, String answerDesc)
        {
            //legger til en "-", samt en \n dersom answerToAdd er bredere enn maksbredden
            answerToAdd = _npc.inGameLevel.spriteService.WrapText(" - " + answerToAdd, _maxTextWidth);

            //finner ut av hvor mange linjer dette svaret består av (1 + hver \n i svaret)
            int numOfLines = 1;
            //funnet her: http://stackoverflow.com/questions/541954/how-would-you-count-occurences-of-a-string-within-a-string-c
            numOfLines += answerToAdd.Length - answerToAdd.Replace("\n", "").Length;

            //hvis _playerTalkBoxLines har flest linjer
            if (_playerTalkBoxLines + numOfLines > _talkBoxLines)
            {
                ChangeHeight(numOfLines, true);
            }

            //legg til et nytt PlayerTextAnswer i listen over svaralternativer
            playerAnswers.Add(new PlayerTextAnswer(answerToAdd, answerDesc, new Rectangle(playerTalkBox.DestinationX + 10,
                playerTalkBox.DestinationRectangle.Top + _textOffsetY + (_lineHeight*_playerTalkBoxLines), 500, _lineHeight), _defaultAnswerColor));
            _playerTalkBoxLines += numOfLines;
        }

        /// <summary>
        /// Fjern alle spillerens svaralternativer fra listen
        /// </summary>
        public void RemovePlayerAnswers()
        {
            //sjekker om _playerTalkBoxLines har flest linjer
            bool wasHighestBox = false;
            if (_playerTalkBoxLines == _talkBoxLines)
                wasHighestBox = true;

            //fjerner alle svaralternativene, da er det nødvendigvis 0 linjer igjen
            playerAnswers.Clear();
            _playerTalkBoxLines = 0;

            //hvis dialogboksen til spilleren hadde flest linjer settes ny høyde til høyden på NPCens dialogboks
            if (_playerTalkBoxLines < _talkBoxLines && wasHighestBox)
            {
                ChangeHeight(_talkBoxLines - _npcTalkBoxLines, false);
                _talkBoxLines = _npcTalkBoxLines;
            }
        }

        /// <summary>
        /// Endre høyden på dialogboksene
        /// </summary>
        /// <param name="numOfLines">antall linjer man skal legge til/trekke fra</param>
        /// <param name="bigger">om boksene skal bli større</param>
        private void ChangeHeight(int numOfLines, bool bigger)
        {
            //regner ut hvor mye det skal legges til/trekkes fra (i piksler) og finner ut om det skal legges til/trekkes fra
            int heightChange = _lineHeight * numOfLines;
            if (!bigger)
                heightChange *= -1;

            //endrer hver boks, flytter på tekst (ja, det er mye å flytte på)
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