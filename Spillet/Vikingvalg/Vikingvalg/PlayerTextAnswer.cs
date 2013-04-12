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
    class PlayerTextAnswer
    {
        public String answer;
        public String answerDesc;
        public Rectangle answerBox;
        private Vector2 _answerBoxLocation = Vector2.Zero;
        public Vector2 AnswerBoxLocation
        {
            get
            {
                _answerBoxLocation.X = answerBox.X;
                _answerBoxLocation.Y = answerBox.Y;
                return _answerBoxLocation; 
            }
            private set { }
        }
        public Color textColor;
        public bool hovered;

        public PlayerTextAnswer(String answer, String answerDesc, Rectangle answerBox, Color textColor, bool hovered)
        {
            this.answer = answer;
            this.answerDesc = answerDesc;
            this.answerBox = answerBox;
            this.textColor = textColor;
            this.hovered = hovered;
        }
        public PlayerTextAnswer(String answer, String answerDesc, Rectangle answerBox, Color textColor)
            : this(answer, answerDesc, answerBox, textColor, false)
        { 
        }
    }
}
