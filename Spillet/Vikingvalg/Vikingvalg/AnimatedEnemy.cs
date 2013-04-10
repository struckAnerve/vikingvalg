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

using Demina;

namespace Vikingvalg
{
    class AnimatedEnemy : AnimatedCharacter
    {
        public int mobIndex { get; set; }
        public Player _player1 { private get; set; }
        
        private Point _distance;
        private Point _target;
        private static Random _rand = new Random();
        private int _targetSpan = 7;
        private int[] _yPosArray;
        private bool _positionRight = true;
        private bool _attackRight = true;
        private bool _attackOfOpportunity = false;
        private bool _firstAttack = true;
        private Rectangle _lastAllowedPosition;
        public AnimatedEnemy(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth, float scale, Player player1, int hitPoints)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, scale, hitPoints)
        {
            _yPosArray = new int[] { 0, -150, -100, 0, 100, 150 };
            _player1 = player1;
            _lastAllowedPosition = _destinationRectangle;
            //Legger til alle navn på animasjoner som fienden har, brukes for å laste inn riktige animasjoner.
            animationList.Add("attack1");
            animationList.Add("attack2");
            animationList.Add("walk");
            animationList.Add("run");
            animationList.Add("taunt");
        }
        public override void Update()
        {
            setLayerDepth(_footBox.Bottom );
            if (AnimationState != "attacking")
            {
                walk();
            }
            else if (animationPlayer.Transitioning == false && animationPlayer.CurrentKeyframeIndex > 0 && animationPlayer.CurrentKeyframeIndex >= _attackDamageFrame)
            {
                if (!_attacked)
                {
                   setAttackTargetDistance();
                   _targetSpan += 70;
                   if (withinRangeOfTarget(_footBox, _target))
                       _player1.takeDamage();
                   currentSoundEffect = "attack1";
                   _targetSpan -= 70;
                   _attackOfOpportunity = false;
                   _attacked = true;
                }
                if (animationPlayer.CurrentKeyframeIndex == (animationPlayer.currentPlayingAnimation.Keyframes.Count() - 1))
                {
                    idle();
                    _attacked = false;
                }
                
            }
        }
        public void taunt()
        {
            if (animationPlayer.CurrentAnimation != "taunt" && AnimationState != "taunting")
            {
                animationPlayer.TransitionToAnimation("taunt", 0.2f);
                AnimationState = "taunting";
            }
        }
        public virtual void attack1()
        {
            if (AnimationState != "attack1" && AnimationState != "attacking")
            {
                animationPlayer.TransitionToAnimation("attack1", 0.2f);
                AnimationState = "attacking";
            }
        }
        public virtual void attack2()
        {
            if (spriteAnimation.CurrentAnimation != "attack2" && AnimationState != "attacking")
            {
                animationPlayer.TransitionToAnimation("attack2", 0.2f);
                AnimationState = "attacking";
            }
        }
        public void attackFormation()
        {
            setAttackTargetDistance();
            if (_firstAttack || (withinRangeOfTarget(_footBox, _target) && AnimationState != "attacking" && _rand.Next(0, 100) > 95))
            {
                if (_rand.Next(0, 2) < 1) attack1();
                else attack2();
                _firstAttack = false;
            }
        }
        public void waitingFormation()
        {
            if (rInt(0, 1000) >= 999)
            {
                _positionRight = !_positionRight;
            }
            setWaitingTargetDistance();
            if (withinRangeOfTarget(_footBox, _target))
            {
                idle();
            }
        }
        public void walk()
        {
            if (rInt(0, 1000) >= 999 && _attackOfOpportunity == false) _attackOfOpportunity = true;
            if (activeEnemy == this || _attackOfOpportunity){
                attackFormation();
            }
            else waitingFormation();
            if (_footBox.Center.X > _player1.FootBox.Center.X) _attackRight = true;
            else _attackRight = false;
            if (withinRangeOfTarget(_footBox, _target))
            {
                if (_attackRight) Flipped = true;
                else Flipped = false;
            }
            else
            {
                if (activeEnemy == this) Console.WriteLine(animationPlayer.CurrentAnimation + " " + spriteAnimation.CurrentAnimation);
                if (animationPlayer.CurrentAnimation != "walking" && AnimationState != "walking")
                {
                    animationPlayer.TransitionToAnimation("walk", 0.2f);
                    AnimationState = "walking";
                }
                if (AnimationState == "walking")
                {
                    // Kode fra http://www.berecursive.com/2008/c/rotating-a-sprite-towards-an-object-in-xna //
                    //Calculate the required rotation by doing a two-variable arc-tan
                    float rotation = (float)Math.Atan2(_distance.Y, _distance.X);
                    //Move square towards mouse by closing the gap by speed per update
                    _xSpeed = (int)(_speed * Math.Cos(rotation));
                    _ySpeed = (int)(_speed * Math.Sin(rotation));
                    // Kode fra http://www.berecursive.com/2008/c/rotating-a-sprite-towards-an-object-in-xna //
                    _footBox.X -= _xSpeed;
                    _footBox.Y -= _ySpeed;
                    if (_xSpeed > 0 && _footBox.Center.X > _target.X)
                    {
                        Flipped = true;
                    }
                    else if (_xSpeed < 0 && _footBox.Center.X < _target.X)
                    {
                        Flipped = false;
                    }
                    _firstAttack = true;
                    _destinationRectangle.X = _footBox.Center.X - footBoxXOffset;
                    _destinationRectangle.Y = _footBox.Y - footBoxXOffset;
                    healthbar.setPosition(_footBox);
                }
            }
        }
        public void takeDamage()
        {
            hp -= 10;
            healthbar.updateHealtBar(hp);
            if (hp <= 0) Console.WriteLine("Dead");
        }
        private bool blocked()
        {
            if (BlockedBottom || BlockedTop || BlockedLeft || BlockedRight) return true;
            return false;
        }
        private bool withinRangeOfTarget(Rectangle box, Point target){
            if (box.Bottom > target.Y + 6 - _targetSpan && box.Bottom < target.Y + _targetSpan)
            {
                if (_attackRight)
                    return (box.Left > target.X - _targetSpan && box.Left < target.X + _targetSpan);
                else 
                    return (box.Right > target.X - _targetSpan && box.Right < target.X + _targetSpan && box.Bottom > target.Y - _targetSpan && box.Bottom < target.Y + _targetSpan);
            }
            return false;
        }
        public static int rInt(int min, int max)
        {
            int t = _rand.Next(min, max);
            return t;
        }
        public void setAttackTargetDistance()
        {
            if (_footBox.Center.X > _player1.FootBox.Center.X)
            {
                _target.X = _player1.FootBox.X + _player1.FootBox.Width + 5;
                _distance.X = _footBox.X - _target.X;
            }
            else
            {
                _target.X = _player1.FootBox.X - 5;
                _distance.X = _footBox.X + _footBox.Width - _target.X;
            }
            _target.Y = _player1.FootBox.Y + _player1.FootBox.Height;
            _distance.Y = _footBox.Y + _footBox.Height - _target.Y;
        }
        private void setWaitingTargetDistance()
        {
            if (_positionRight)
            {
                _target.Y = _player1.FootBox.Top + _yPosArray[mobIndex];
                while (_target.Y - _footBox.Height - 40 < 170) _target.Y += 1;
                while (_target.Y > 700) _target.Y -= 2;
                _target.X = _player1.FootBox.Right + 100 + (int)((Math.Sqrt(Math.Pow(200, 2) - Math.Pow(_yPosArray[mobIndex], 2)) * 1.2));
                while (_target.X + _footBox.Width > 1350) _target.X--;
                _distance.X = _footBox.Left - _target.X;
            }
            else if (!_positionRight)
            {
                _target.Y = _player1.FootBox.Top + (_yPosArray[mobIndex] * -1);
                while (_target.Y - _footBox.Height - 40 < 170) _target.Y += 2;
                while (_target.Y > 700) _target.Y -= 2;
                _target.X = _player1.FootBox.Left - 100 - (int)((Math.Sqrt(Math.Pow(200, 2) - Math.Pow((_yPosArray[mobIndex] * -1), 2)) * 1.2));
                while (_target.X - _footBox.Width < -100) _target.X += 2;
                _distance.X = _footBox.Right - _target.X;
            }
            _distance.Y = _footBox.Y + _footBox.Height - _target.Y;
        }
    }
}
