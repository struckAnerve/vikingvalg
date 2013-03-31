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
    class Player : AnimatedSprite, IUseInput, ICanCollide
    {
        private Rectangle _footBox;

        public Rectangle FootBox 
        {
            get { return _footBox; }
            set { }
        }

        Dictionary<string, string> playerBoneList = new Dictionary<string, string>();

        public bool BlockedLeft { get; set; }
        public bool BlockedRight { get; set; }
        public bool BlockedTop { get; set; }
        public bool BlockedBottom { get; set; }

        public Player(String artName, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
            : base(artName, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth, "playerAnimation/")
        {
            footBoxXOfset = destinationRectangle.Width / 2;
            footBoxYOfset = 40;
            _footBox = new Rectangle(destinationRectangle.X - footBoxXOfset, destinationRectangle.Y + footBoxYOfset, destinationRectangle.Width, footBoxHeight);

            animationList = new List<String>();
            animationPlayer = new AnimationPlayer();

            animationList.Add("walkCycle");
            animationList.Add("idle");
            animationList.Add("strikeSword");
            animationList.Add("block");
            animationList.Add("strikeSword");

            playerBoneList.Add("head", "head");
            playerBoneList.Add("torso", "torso");
            playerBoneList.Add("bicep", "bicepL");
            playerBoneList.Add("forearmR", "forearmR");
            playerBoneList.Add("idleShieldArm", "idleShieldArm");
            playerBoneList.Add("blockingShieldArm", "blockingShieldArm");
            playerBoneList.Add("shield", "shield");
            playerBoneList.Add("swordHand", "swordHand");
            playerBoneList.Add("thigh", "thighL");
            playerBoneList.Add("shin", "shinL");
            playerBoneList.Add("foot", "footL");
        }
        public Player(Rectangle destinationRectangle)
            : this("mm", destinationRectangle, new Rectangle(0, 0, 375, 485), new Color(255, 255, 255, 1f), 0, Vector2.Zero, SpriteEffects.None, 0.6f)
        { }
        public Player(Vector2 destinationPosition)
            : this(new Rectangle((int)destinationPosition.X, (int)destinationPosition.Y, 375, 485))
        { }
        public Player()
            : this(Vector2.Zero)
        { }

        public void Update(IManageInput inputService)
        {
            if (inputService.KeyIsDown(Keys.Left) || inputService.KeyIsDown(Keys.Down) || inputService.KeyIsDown(Keys.Up) || inputService.KeyIsDown(Keys.Right))
            {
                walk(inputService);
            }
            else
            {
                idle();
            }
        }

        public void idle()
        {
            if (AnimationState != "idle")
            {
                animationPlayer.TransitionToAnimation("idle", 0.2f);
                AnimationState = "idle";
            }
        }
        public void attackSlash()
        {
            if (AnimationState != "slashing")
            {
                animationPlayer.TransitionToAnimation("slashing", 0.2f);
                AnimationState = "slashing";
            }
        }
        public void walk(IManageInput inputService)
        {
            if (spriteAnimation.CurrentAnimation != "walking" && AnimationState != "walking")
            {
                animationPlayer.TransitionToAnimation("walkCycle", 0.2f);
                AnimationState = "walking";
            }
            if (AnimationState == "walking")
            {
                if (inputService.KeyIsDown(Keys.Right) && inputService.KeyIsDown(Keys.Left))
                {
                    idle();
                }
                else if (inputService.KeyIsDown(Keys.Right) && inputService.KeyIsUp(Keys.Left) && !BlockedRight)
                {
                    _destinationRectangle.X += _speed;
                    Flipped = false;
                }
                else if (inputService.KeyIsDown(Keys.Left) && inputService.KeyIsUp(Keys.Right) && !BlockedLeft)
                {
                    _destinationRectangle.X -= _speed;
                    Flipped = true;
                }
                else
                {
                    idle();
                }
                if (inputService.KeyIsDown(Keys.Up) && !BlockedTop)
                {
                    _destinationRectangle.Y -= _speed;
                    _footBox.Y = (_destinationRectangle.Y + footBoxYOfset);
                }
                else if (inputService.KeyIsDown(Keys.Down) && !BlockedBottom)
                {
                    _destinationRectangle.Y += _speed;
                    _footBox.Y = (_destinationRectangle.Y + footBoxYOfset);
                }
                _footBox.X = _destinationRectangle.X - footBoxXOfset;
            }
        }
        public void block()
        {
            if (animationPlayer.CurrentAnimation != "blocking")
            {
                spriteAnimation.TransitionToAnimation("blocking", 0.2f);
                AnimationState = "blocking";
            }
        }

    }
}
