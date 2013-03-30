using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Demina
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			xnaDisplay.InitializeHandler += xnaDisplay_Initialize;
			xnaDisplay.DrawHandler += xnaDisplay_Draw;
			xnaDisplay.MouseDown += new MouseEventHandler(xnaDisplay_MouseDown);
			xnaDisplay.MouseUp += new MouseEventHandler(xnaDisplay_MouseUp);
			xnaDisplay.MouseMove += new MouseEventHandler(xnaDisplay_MouseMove);
			xnaDisplay.KeyDown += new KeyEventHandler(xnaDisplay_KeyDown);

			FormClosing += new FormClosingEventHandler(Form1_FormClosing);

			Application.Idle += delegate { xnaDisplay.Invalidate(); };

			NewAnimation();

			zoomComboBox.SelectedItem = "100%";
			zoomComboBox.SelectedIndexChanged += new EventHandler(zoomComboBox_SelectedIndexChanged);

			frameRateComboBox.SelectedItem = "25";
			frameRateComboBox.SelectedIndexChanged += new EventHandler(frameRateComboBox_SelectedIndexChanged);

			string[] args = Environment.GetCommandLineArgs();

			if (args != null && args.Length > 1 && args[1] != null)
			{
				startupAnimation = args[1];
			}
		}

		void NewAnimation()
		{
			keyframes = new List<Keyframe>();
			keyframes.Add(new Keyframe(0));

			boneTextures = new List<Texture2D>();
			boneTextureFileNames = new List<string>();
			boneTexturePixels = new List<Color[]>();

			currentKeyframe = 0;
			currentBoneIndex = -1;

			loopFrame = -1;

			atExistingKeyframe = true;
			existingKeyframe = keyframes[0];

			copiedKeyframe = null;

			previouslySelectedBones.Clear();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (dirtyFlag)
			{
				DialogResult dialogResult = MessageBox.Show("Unsaved changes will be lost. Do you want to save?",
					"Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

				switch (dialogResult)
				{
					case DialogResult.Cancel:
						e.Cancel = true;
						return;
					case DialogResult.No:
						break;
					case DialogResult.Yes:
						if (string.IsNullOrEmpty(savePath))
							saveAsToolStripMenuItem_Click(sender, e);
						else
							Save(savePath);
						break;
				}
			}
		}

		private void xnaDisplay_Click(object sender, EventArgs e)
		{
			xnaDisplay.Focus();
		}

		void xnaDisplay_MouseDown(object sender, MouseEventArgs e)
		{
            bool controlHeld = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            bool shiftHeld = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

			switch (e.Button)
			{
				case MouseButtons.Left:
                    if (controlHeld)
                    {
                        horizontalLinePosition1 = e.Y;
                    }
                    else if (shiftHeld)
                    {
                        horizontalLinePosition2 = e.Y;
                    }
                    else
                    {
                        if (e.Y > xnaDisplay.Height - KEYFRAME_BOX_BOTTOM_MARGIN - KEYFRAME_BOX_SIZE)
                        {
                            if (e.X > KEYFRAME_BOX_LEFT_MARGIN && e.X < (xnaDisplay.Width - KEYFRAME_BOX_RIGHT_MARGIN))
                            {
                                UpdateCurrentKeyframe((e.X - KEYFRAME_BOX_LEFT_MARGIN) / (KEYFRAME_BOX_SIZE + KEYFRAME_BOX_PADDING) + leftKeyframe);
                            }
                            else if (e.X <= KEYFRAME_BOX_LEFT_MARGIN)
                            {
                                int keyframesPerScreen = (xnaDisplay.Width - (KEYFRAME_BOX_LEFT_MARGIN + KEYFRAME_BOX_RIGHT_MARGIN)) / (KEYFRAME_BOX_SIZE + KEYFRAME_BOX_PADDING);

                                leftKeyframe -= keyframesPerScreen;
                                leftKeyframe = Math.Max(0, leftKeyframe);
                            }
                            else
                            {
                                int keyframesPerScreen = (xnaDisplay.Width - (KEYFRAME_BOX_LEFT_MARGIN + KEYFRAME_BOX_RIGHT_MARGIN)) / (KEYFRAME_BOX_SIZE + KEYFRAME_BOX_PADDING);

                                leftKeyframe += keyframesPerScreen;
                            }
                        }
                        else
                        {
                            Vector2 clickPosition = new Vector2(e.X, e.Y);
							Vector2 invertedClickPosition = new Vector2(xnaDisplay.Width - e.X, xnaDisplay.Height - e.Y);
                            int selectedBoneIndex = -1;

                            string zoomString = zoomComboBox.SelectedItem.ToString();
                            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(zoomString, @"[0-9]+");
                            float zoom = (float)int.Parse(m.Value);

                            Matrix zoomMatrix = Matrix.CreateScale(zoom / 100) * Matrix.CreateTranslation(xnaDisplay.Width / 2, xnaDisplay.Height / 2, 0);
                            clickPosition = Vector2.Transform(clickPosition, Matrix.Invert(zoomMatrix));
							invertedClickPosition = Vector2.Transform(invertedClickPosition, Matrix.Invert(zoomMatrix));

                            if (atExistingKeyframe && existingKeyframe != null)
                            {
								if (existingKeyframe.FlipHorizontally)
									clickPosition.X = invertedClickPosition.X;
								if (existingKeyframe.FlipVertically)
									clickPosition.Y = invertedClickPosition.Y;

                                List<int> possibleSelections = new List<int>();

                                for (int boneIndex = 0; boneIndex < existingKeyframe.Bones.Count; boneIndex++)
                                {
                                    Bone bone = existingKeyframe.Bones[boneIndex];

                                    Matrix inverseTransform = Matrix.Invert(bone.Transform);
                                    Vector2 clickInBoneSpace = Vector2.Transform(clickPosition, inverseTransform);

                                    Color[] pixels = boneTexturePixels[bone.TextureIndex];
									if (bone.TextureFlipHorizontal)
										pixels = FlipPixelsHorizontal(pixels, boneTextures[bone.TextureIndex].Width, boneTextures[bone.TextureIndex].Height);
									if(bone.TextureFlipVertical)
										pixels = FlipPixelsVertical(pixels, boneTextures[bone.TextureIndex].Width, boneTextures[bone.TextureIndex].Height);

                                    Point texturePosition = new Point((int)clickInBoneSpace.X + boneTextures[bone.TextureIndex].Width / 2,
                                        (int)clickInBoneSpace.Y + boneTextures[bone.TextureIndex].Height / 2);
                                    if (texturePosition.X >= 0 && texturePosition.X < boneTextures[bone.TextureIndex].Width &&
                                        texturePosition.Y >= 0 && texturePosition.Y < boneTextures[bone.TextureIndex].Height &&
                                        pixels[texturePosition.Y * boneTextures[bone.TextureIndex].Width + texturePosition.X].A > 0)
                                    {
                                        possibleSelections.Add(boneIndex);
                                    }
                                }

                                bool foundOne = false;

                                foreach (int boneIndex in possibleSelections)
                                {
                                    if (previouslySelectedBones.FindIndex(b => b == boneIndex) == -1)
                                    {
                                        previouslySelectedBones.Add(boneIndex);
                                        selectedBoneIndex = boneIndex;
                                        foundOne = true;
                                        break;
                                    }
                                }

                                if (!foundOne)
                                {
                                    if (possibleSelections.Count > 0)
                                        selectedBoneIndex = currentBoneIndex;

                                    foreach (int boneIndex in possibleSelections)
                                    {
                                        if (boneIndex != currentBoneIndex)
                                        {
                                            selectedBoneIndex = boneIndex;
                                            previouslySelectedBones.Clear();
                                            previouslySelectedBones.Add(selectedBoneIndex);
                                            break;
                                        }
                                    }
                                }
                            }

                            currentBoneIndex = selectedBoneIndex;
                        }
                    }
					break;

				case MouseButtons.Right:
                    if (controlHeld)
                    {
                        verticalLinePosition1 = e.X;
                    }
                    else if (shiftHeld)
                    {
                        verticalLinePosition2 = e.X;
                    }
                    else
                    {
                        rightMouseDown = true;
						middleMouseDown = false;
						currentlyRotating = false;
                    }
					break;

				case MouseButtons.Middle:
					middleMouseDown = true;
					rightMouseDown = false;
					currentlyTranslating = false;
					break;
			}

			lastMousePosition = new Point(e.X, e.Y);
		}

		Color[] FlipPixelsHorizontal(Color[] pixels, int width, int height)
		{
			Color[] result = new Color[width * height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					result[y * width + (width - x - 1)] = pixels[y * width + x];
				}
			}

			return result;
		}

		Color[] FlipPixelsVertical(Color[] pixels, int width, int height)
		{
			Color[] result = new Color[width * height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					result[(height - y - 1) * width + x] = pixels[y * width + x];
				}
			}

			return result;
		}

		void xnaDisplay_MouseUp(object sender, MouseEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
					break;

				case MouseButtons.Right:
					rightMouseDown = false;
					break;

				case MouseButtons.Middle:
					middleMouseDown = false;
					break;
			}

			foreach (Keyframe k in keyframes)
			{
				k.UpdateBones();
			}

			lastMousePosition = new Point(e.X, e.Y);
		}

		void xnaDisplay_MouseMove(object sender, MouseEventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				bool updateRequired = false;

				string zoomString = zoomComboBox.SelectedItem.ToString();
				System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(zoomString, @"[0-9]+");
				float zoom = int.Parse(m.Value) / 100.0f;

				if (middleMouseDown)
				{
					if (!currentlyTranslating)
					{
						currentlyTranslating = true;
						AddUndo();
						ClearRedo();
					}

					float angle = existingKeyframe.Bones[currentBoneIndex].TransformedRotation - existingKeyframe.Bones[currentBoneIndex].Rotation;
					Matrix rotation = Matrix.CreateRotationZ(-angle);
					Vector2 direction = new Vector2(e.X - lastMousePosition.X, e.Y - lastMousePosition.Y);
					if (lockXButton.Checked)
						direction.X = 0;
					if (lockYButton.Checked)
						direction.Y = 0;
					if (existingKeyframe.FlipVertically)
						direction.Y *= -1;
					if (existingKeyframe.FlipHorizontally)
						direction.X *= -1;
					direction /= zoom;
					Vector2 transformedDirection = Vector2.Transform(direction, rotation);

					existingKeyframe.Bones[currentBoneIndex].Position += transformedDirection;
					updateRequired = true;
				}
				else if (rightMouseDown)
				{
					if (!currentlyRotating)
					{
						currentlyRotating = true;
						AddUndo();
						ClearRedo();
					}

					float rotation = (e.X - lastMousePosition.X) / 100.0f;
					if (existingKeyframe.FlipVertically)
						rotation *= -1;
					if (existingKeyframe.FlipHorizontally)
						rotation *= -1;
					existingKeyframe.Bones[currentBoneIndex].Rotation += rotation;
					updateRequired = true;
				}

				if (updateRequired)
				{
					foreach (Keyframe k in keyframes)
					{
						k.UpdateBones();
					}
				}
			}

			lastMousePosition = new Point(e.X, e.Y);
		}

		void xnaDisplay_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.F:
					NextBone();
					break;
				case Keys.G:
					PreviousBone();
					break;
				case Keys.H:
					if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
					{
						existingKeyframe.Bones[currentBoneIndex].Hidden = !existingKeyframe.Bones[currentBoneIndex].Hidden;
					}
					break;

				case Keys.T:
					NextTexture();
					break;

				case Keys.R:
					PreviousTexture();
					break;

                case Keys.D:
                    //Move to the next frame
                    if (loopFrame != -1)
                    {
                        if (currentKeyframe + 1 >= loopFrame)
                            UpdateCurrentKeyframe(0);
                        else
                            UpdateCurrentKeyframe(currentKeyframe + 1);
                    }
                    else
                    {
                        if(currentKeyframe + 1 <= keyframes[keyframes.Count - 1].FrameNumber)
                            UpdateCurrentKeyframe(currentKeyframe + 1);
                    }
                    break;

                case Keys.A:
                    //Move to the prev frame
                    if (loopFrame != -1)
                    {
                        if (currentKeyframe > 0)
                            UpdateCurrentKeyframe(currentKeyframe - 1);
                        else
                            UpdateCurrentKeyframe(loopFrame - 1);
                    }
                    else
                    {
                        if(currentKeyframe > 0)
                            UpdateCurrentKeyframe(currentKeyframe - 1);
                    }
                    break;
			}
		}

		void UpdateCurrentKeyframe(int frame)
		{
			previouslySelectedBones.Clear();

			currentKeyframe = frame;

			int foundIndex = keyframes.FindIndex(key => key.FrameNumber == currentKeyframe);
			if (foundIndex != -1)
			{
				atExistingKeyframe = true;
				existingKeyframe = keyframes[foundIndex];
				currentKeyframeIndex = foundIndex;

				if (currentBoneIndex >= existingKeyframe.Bones.Count)
					currentBoneIndex = -1;
			}
			else
			{
				atExistingKeyframe = false;
				existingKeyframe = null;

				for (int keyIndex = 0; keyIndex < keyframes.Count; keyIndex++)
				{
					if (keyframes[keyIndex].FrameNumber < currentKeyframe)
					{
						// at last key
						//		loop point is after current position
						//		no loop
						// between keys
						//		next key is before loop point (or no loop)
						//		next key is after loop point

						if (keyIndex == keyframes.Count - 1)
						{
							if (loopFrame > currentKeyframe)
							{
								existingKeyframe = null;
								interpolatedKeyframe = Keyframe.Interpolate(currentKeyframe, keyframes[keyIndex], keyframes[keyIndex].FrameNumber, keyframes[0], loopFrame);
							}
							else
							{
								existingKeyframe = null;
								interpolatedKeyframe = new Keyframe(keyframes[keyIndex]);
								interpolatedKeyframe.FrameNumber = currentKeyframe;
							}
							break;
						}
						else if (keyframes[keyIndex + 1].FrameNumber > currentKeyframe)
						{
							if (loopFrame > currentKeyframe && loopFrame < keyframes[keyIndex + 1].FrameNumber)
							{
								existingKeyframe = null;
								interpolatedKeyframe = Keyframe.Interpolate(currentKeyframe, keyframes[keyIndex], keyframes[keyIndex].FrameNumber, keyframes[0], loopFrame);
							}
							else
							{
								existingKeyframe = null;
								interpolatedKeyframe = Keyframe.Interpolate(currentKeyframe, keyframes[keyIndex], keyframes[keyIndex + 1]);
							}
							break;
						}
					}
				}
			}
		}

		void xnaDisplay_Initialize(object sender, EventArgs e)
		{
			contentManager = new ContentManager(xnaDisplay.Services, "DeminaContent");
			spriteBatch = new SpriteBatch(xnaDisplay.GraphicsDevice);

			PresentationParameters pp = xnaDisplay.GraphicsDevice.PresentationParameters;
			previousRenderTarget = new RenderTarget2D(xnaDisplay.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, DepthFormat.None);
			nextRenderTarget = new RenderTarget2D(xnaDisplay.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, DepthFormat.None);

			blankTexture = new Texture2D(xnaDisplay.GraphicsDevice, 1, 1);
			blankTexture.SetData<Color>(new Color[] { Color.White });

			font = contentManager.Load<SpriteFont>("Fonts/ProggyCleanTTSZ_12");
			keyframeArrow = contentManager.Load<Texture2D>("Graphics/keyframe_arrow");
			loopArrow = contentManager.Load<Texture2D>("Graphics/loop_arrow");
			redMaskEffect = contentManager.Load<Effect>("Shaders/redmask");
			onionSkinEffect = contentManager.Load<Effect>("Shaders/onionskin");

			leftArrow = contentManager.Load<Texture2D>("Graphics/left_arrow");
			rightArrow = contentManager.Load<Texture2D>("Graphics/right_arrow");

			xnaDisplay.GraphicsDevice.DeviceResetting += new EventHandler<System.EventArgs>(GraphicsDevice_DeviceResetting);

			if(!string.IsNullOrEmpty(startupAnimation))
				OpenAnimation(startupAnimation);
		}

		void GraphicsDevice_DeviceResetting(object sender, EventArgs e)
		{
			PresentationParameters pp = xnaDisplay.GraphicsDevice.PresentationParameters;

			previousRenderTarget.Dispose();
			previousRenderTarget = new RenderTarget2D(xnaDisplay.GraphicsDevice, xnaDisplay.Width, xnaDisplay.Height, false, pp.BackBufferFormat, DepthFormat.None);
			nextRenderTarget.Dispose();
			nextRenderTarget = new RenderTarget2D(xnaDisplay.GraphicsDevice, xnaDisplay.Width, xnaDisplay.Height, false, pp.BackBufferFormat, DepthFormat.None);
		}

		void xnaDisplay_Draw(object sender, EventArgs e)
		{
			Keyframe keyframeToDraw;

			string zoomString = zoomComboBox.SelectedItem.ToString();
			System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(zoomString, @"[0-9]+");
			float zoom = (float)int.Parse(m.Value);

			xnaDisplay.GraphicsDevice.SetRenderTarget(previousRenderTarget);
			xnaDisplay.GraphicsDevice.Clear(Color.Transparent);

			if (previousOnionSkinButton.Checked && atExistingKeyframe && currentKeyframeIndex > 0)
			{
				keyframeToDraw = keyframes[currentKeyframeIndex - 1];
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(zoom / 100 * (keyframeToDraw.FlipHorizontally ? -1 : 1), zoom / 100 * (keyframeToDraw.FlipVertically ? -1 : 1), 1) * Matrix.CreateTranslation(xnaDisplay.Width / 2, xnaDisplay.Height / 2, 0));

				foreach (Bone bone in keyframeToDraw.Bones)
				{
					if (bone.Hidden)
						continue;

					SpriteEffects spriteEffects = SpriteEffects.None;
					if (bone.TextureFlipHorizontal)
						spriteEffects = SpriteEffects.FlipHorizontally;
					if (bone.TextureFlipVertical)
						spriteEffects |= SpriteEffects.FlipVertically;

					spriteBatch.Draw(boneTextures[bone.TextureIndex], bone.TransformedPosition, null, Color.White, bone.TransformedRotation, new Vector2(boneTextures[bone.TextureIndex].Width / 2, boneTextures[bone.TextureIndex].Height / 2), bone.TransformedScale, spriteEffects, 0);
				}

				spriteBatch.End();
			}

			xnaDisplay.GraphicsDevice.SetRenderTarget(nextRenderTarget);
			xnaDisplay.GraphicsDevice.Clear(Color.Transparent);

			if (nextOnionSkinButton.Checked && atExistingKeyframe && currentKeyframeIndex < keyframes.Count - 1)
			{
				keyframeToDraw = keyframes[currentKeyframeIndex + 1];
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(zoom / 100 * (keyframeToDraw.FlipHorizontally ? -1 : 1), zoom / 100 * (keyframeToDraw.FlipVertically ? -1 : 1), 1) * Matrix.CreateTranslation(xnaDisplay.Width / 2, xnaDisplay.Height / 2, 0));

				foreach (Bone bone in keyframeToDraw.Bones)
				{
					if (bone.Hidden)
						continue;

					SpriteEffects spriteEffects = SpriteEffects.None;
					if (bone.TextureFlipHorizontal)
						spriteEffects = SpriteEffects.FlipHorizontally;
					if (bone.TextureFlipVertical)
						spriteEffects |= SpriteEffects.FlipVertically;

					spriteBatch.Draw(boneTextures[bone.TextureIndex], bone.TransformedPosition, null, Color.White, bone.TransformedRotation, new Vector2(boneTextures[bone.TextureIndex].Width / 2, boneTextures[bone.TextureIndex].Height / 2), bone.TransformedScale, spriteEffects, 0);
				}

				spriteBatch.End();
			}

			xnaDisplay.GraphicsDevice.SetRenderTarget(null);

			xnaDisplay.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, onionSkinEffect);
			onionSkinEffect.Parameters["Color"].SetValue(new Vector3(0, 0, 1));
			onionSkinEffect.Parameters["Alpha"].SetValue(0.5f);
			spriteBatch.Draw(previousRenderTarget, Vector2.Zero, Color.White);
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, onionSkinEffect);
			onionSkinEffect.Parameters["Color"].SetValue(new Vector3(0, 1, 0));
			onionSkinEffect.Parameters["Alpha"].SetValue(0.5f);
			spriteBatch.Draw(nextRenderTarget, Vector2.Zero, Color.White);
			spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture, new Rectangle(0, horizontalLinePosition1, xnaDisplay.Width, 1), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(0, horizontalLinePosition2, xnaDisplay.Width, 1), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(verticalLinePosition1, 0, 1, xnaDisplay.Height), Color.White);
            spriteBatch.Draw(blankTexture, new Rectangle(verticalLinePosition2, 0, 1, xnaDisplay.Height), Color.White);
            spriteBatch.End();

			if (atExistingKeyframe && existingKeyframe != null)
				keyframeToDraw = existingKeyframe;
			else
				keyframeToDraw = interpolatedKeyframe;

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(zoom / 100 * (keyframeToDraw.FlipHorizontally ? -1 : 1), zoom / 100 * (keyframeToDraw.FlipVertically ? -1 : 1), 1) * Matrix.CreateTranslation(xnaDisplay.Width/2, xnaDisplay.Height/2, 0));

			foreach (Bone bone in keyframeToDraw.Bones)
			{
				if (bone.Hidden)
					continue;

				SpriteEffects spriteEffects = SpriteEffects.None;
				if (bone.TextureFlipHorizontal)
					spriteEffects = SpriteEffects.FlipHorizontally;
				if (bone.TextureFlipVertical)
					spriteEffects |= SpriteEffects.FlipVertically;

				spriteBatch.Draw(boneTextures[bone.TextureIndex], bone.TransformedPosition, null, Color.White, bone.TransformedRotation, new Vector2(boneTextures[bone.TextureIndex].Width / 2, boneTextures[bone.TextureIndex].Height / 2), bone.TransformedScale, spriteEffects, 0);
			}

			spriteBatch.End();

			if(currentBoneIndex != -1)
			{
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, redMaskEffect, Matrix.CreateScale(zoom / 100 * (keyframeToDraw.FlipHorizontally ? -1 : 1), zoom / 100 * (keyframeToDraw.FlipVertically ? -1 : 1), 1) * Matrix.CreateTranslation(xnaDisplay.Width / 2, xnaDisplay.Height / 2, 0));

				if (atExistingKeyframe && existingKeyframe != null)
				{
					Bone bone = existingKeyframe.Bones[currentBoneIndex];

					SpriteEffects spriteEffects = SpriteEffects.None;
					if (bone.TextureFlipHorizontal)
						spriteEffects = SpriteEffects.FlipHorizontally;
					if (bone.TextureFlipVertical)
						spriteEffects |= SpriteEffects.FlipVertically;

					spriteBatch.Draw(boneTextures[bone.TextureIndex], bone.TransformedPosition, null, Color.White, bone.TransformedRotation, new Vector2(boneTextures[bone.TextureIndex].Width / 2, boneTextures[bone.TextureIndex].Height / 2), bone.TransformedScale, spriteEffects, 0);
				}

				spriteBatch.End();
			}

			spriteBatch.Begin();

			int keyframesPerScreen = (xnaDisplay.Width - (KEYFRAME_BOX_LEFT_MARGIN + KEYFRAME_BOX_RIGHT_MARGIN)) / (KEYFRAME_BOX_SIZE + KEYFRAME_BOX_PADDING);

			int j;
			for (j = 0; j < keyframes.Count; j++)
			{
				if (keyframes[j].FrameNumber >= leftKeyframe)
					break;
			}

			for (int i = leftKeyframe; i < leftKeyframe + keyframesPerScreen + 1; i++)
			{
				int screenKey = i - leftKeyframe;

				Rectangle destination = new Rectangle(screenKey * (KEYFRAME_BOX_SIZE + KEYFRAME_BOX_PADDING) + KEYFRAME_BOX_LEFT_MARGIN,
					xnaDisplay.Height - KEYFRAME_BOX_BOTTOM_MARGIN - KEYFRAME_BOX_SIZE,
					KEYFRAME_BOX_SIZE, KEYFRAME_BOX_SIZE);

				if (i == loopFrame)
				{
					spriteBatch.Draw(loopArrow, destination, Color.White);
				}
				else if (j < keyframes.Count && i == keyframes[j].FrameNumber)
				{
					spriteBatch.Draw(blankTexture, destination, Color.White);
					j++;
				}
				else
				{
					spriteBatch.Draw(blankTexture, destination, Color.Black);
				}

				if (((i + 1) % 5) == 0)
				{
					string text = (i + 1).ToString();
					Vector2 textSize = font.MeasureString(text);
					Vector2 textPosition = new Vector2(screenKey * (KEYFRAME_BOX_SIZE + KEYFRAME_BOX_PADDING) - textSize.X / 2 + KEYFRAME_BOX_SIZE / 2 + KEYFRAME_BOX_LEFT_MARGIN,
						xnaDisplay.Height - KEYFRAME_LABEL_OFFSET_Y);

					textPosition.X = (float)Math.Round(textPosition.X);
					textPosition.Y = (float)Math.Round(textPosition.Y);

					spriteBatch.DrawString(font, (i + 1).ToString(), textPosition, Color.White);
				}
			}

			if (currentKeyframe >= leftKeyframe && currentKeyframe <= leftKeyframe + keyframesPerScreen)
			{
				spriteBatch.Draw(keyframeArrow, new Vector2((currentKeyframe - leftKeyframe) * (KEYFRAME_BOX_SIZE + KEYFRAME_BOX_PADDING) - keyframeArrow.Width / 2 + KEYFRAME_BOX_SIZE / 2 + KEYFRAME_BOX_LEFT_MARGIN,
					xnaDisplay.Height - KEYFRAME_BOX_PADDING - keyframeArrow.Height), Color.White);
			}

			spriteBatch.Draw(leftArrow, new Vector2(0, xnaDisplay.Height - leftArrow.Height), Color.White);
			spriteBatch.Draw(rightArrow, new Vector2(xnaDisplay.Width - rightArrow.Width, xnaDisplay.Height - rightArrow.Height), Color.White);

			if (atExistingKeyframe && existingKeyframe != null && !string.IsNullOrEmpty(existingKeyframe.Trigger))
			{
				string triggerString = "Trigger: " + existingKeyframe.Trigger;
				spriteBatch.DrawString(font, triggerString, Vector2.Zero, Color.White);
			}

			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				string boneName = "Selected Bone: " + existingKeyframe.Bones[currentBoneIndex].Name;
				spriteBatch.DrawString(font, boneName, new Vector2(0, 16), Color.White);
			}

			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				string parentName = existingKeyframe.Bones[currentBoneIndex].ParentIndex == -1 ? "None" : existingKeyframe.Bones[existingKeyframe.Bones[currentBoneIndex].ParentIndex].Name;
				string boneName = "Selected Bone Parent: " + parentName;
				spriteBatch.DrawString(font, boneName, new Vector2(0, 32), Color.White);
			}

			spriteBatch.End();
		}

		private void newAnimationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dirtyFlag)
			{
				DialogResult dialogResult = MessageBox.Show("Unsaved changes will be lost. Do you want to save?",
					"Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
				switch (dialogResult)
				{
					case DialogResult.Cancel:
						return;
					case DialogResult.No:
						break;
					case DialogResult.Yes:
						if (string.IsNullOrEmpty(savePath))
							saveAsToolStripMenuItem_Click(sender, e);
						else
							Save(savePath);
						break;
				}
			}

			dirtyFlag = false;

			ClearUndo();
			ClearRedo();

			savePath = "";
			NewAnimation();

			if (openBoneDialog.ShowDialog() == DialogResult.OK)
			{
				xmlPath = openBoneDialog.FileName;

				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(openBoneDialog.FileName);

				XmlNodeList boneList = xmlDocument.SelectNodes("/Bones/Bone");
				List<string> boneNames = new List<string>();

				foreach (XmlNode boneNode in boneList)
				{
					boneNames.Add(boneNode.Attributes["name"].Value);
				}

				foreach (XmlNode boneNode in boneList)
				{
					string name = boneNode.Attributes["name"].Value;
					string textureFile = boneNode.Attributes["texture"].Value;
					string parentName = boneNode.Attributes["parent"].Value;
					int parentIndex = boneNames.FindIndex(s => s == parentName);

					if (!Path.IsPathRooted(textureFile))
					{
						textureFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(openBoneDialog.FileName), textureFile));
					}

					int textureIndex = boneTextureFileNames.FindIndex(s => s == textureFile);
					if (textureIndex == -1)
					{
						Texture2D texture;
						Color[] pixels;

						LoadPremultipledAlphaTexture(textureFile, out texture, out pixels);

						boneTextures.Add(texture);
						boneTextureFileNames.Add(textureFile);
						boneTexturePixels.Add(pixels);

						textureIndex = boneTextures.Count - 1;
					}

					Bone bone = new Bone(name, textureIndex, parentIndex);
					keyframes[0].AddBone(bone);
				}

				XmlNodeList textureList = xmlDocument.SelectNodes("/Bones/Texture");

				foreach (XmlNode textureNode in textureList)
				{
					string textureFile = textureNode.Attributes["texture"].Value;

					if (!Path.IsPathRooted(textureFile))
					{
						textureFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(openBoneDialog.FileName), textureFile));
					}

					int textureIndex = boneTextureFileNames.FindIndex(s => s == textureFile);
					if (textureIndex == -1)
					{
						Texture2D texture;
						Color[] pixels;

						LoadPremultipledAlphaTexture(textureFile, out texture, out pixels);

						boneTextures.Add(texture);
						boneTextureFileNames.Add(textureFile);
						boneTexturePixels.Add(pixels);
					}
				}

				keyframes[0].SortBones();
				keyframes[0].UpdateBones();
				currentBoneIndex = keyframes[0].Bones.Count - 1;
			}
		}

		private void newAnimationEmptyProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dirtyFlag)
			{
				DialogResult dialogResult = MessageBox.Show("Unsaved changes will be lost. Do you want to save?",
					"Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
				switch (dialogResult)
				{
					case DialogResult.Cancel:
						return;
					case DialogResult.No:
						break;
					case DialogResult.Yes:
						if (string.IsNullOrEmpty(savePath))
							saveAsToolStripMenuItem_Click(sender, e);
						else
							Save(savePath);
						break;
				}
			}

			dirtyFlag = false;
			savePath = "";
			NewAnimation();

			ClearUndo();
			ClearRedo();
		}

		private void copyKeyframeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null)
			{
				copiedKeyframe = new Keyframe(existingKeyframe);
			}
			else if (interpolatedKeyframe != null)
			{
				copiedKeyframe = new Keyframe(interpolatedKeyframe);
			}
		}

		private void pasteKeyframeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!atExistingKeyframe && copiedKeyframe != null)
			{
				AddUndo();
				ClearRedo();

				Keyframe key = new Keyframe(copiedKeyframe);
				key.FrameNumber = currentKeyframe;
				keyframes.Add(key);
				keyframes.Sort((k1, k2) => k1.FrameNumber.CompareTo(k2.FrameNumber));

				atExistingKeyframe = true;
				existingKeyframe = key;

				interpolatedKeyframe = null;
			}
		}

		private void addKeyframeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!atExistingKeyframe && interpolatedKeyframe != null)
			{
				AddUndo();
				ClearRedo();

				keyframes.Add(interpolatedKeyframe);

				keyframes.Sort((k1, k2) => k1.FrameNumber.CompareTo(k2.FrameNumber));

				atExistingKeyframe = true;
				existingKeyframe = interpolatedKeyframe;

				UpdateCurrentKeyframe(interpolatedKeyframe.FrameNumber);

				interpolatedKeyframe = null;
			}
		}

		private void deleteKeyframeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (currentKeyframe != 0 && atExistingKeyframe && existingKeyframe != null)
			{
				AddUndo();
				ClearRedo();

				keyframes.Remove(existingKeyframe);
				UpdateCurrentKeyframe(currentKeyframe);
			}
		}

		private void nextBoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NextBone();
		}

		private void previousBoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PreviousBone();
		}

		private void nextTextureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NextTexture();
		}

		private void previousTextureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PreviousTexture();
		}

		private void textureFlipHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				AddUndo();
				ClearRedo();

				existingKeyframe.Bones[currentBoneIndex].TextureFlipHorizontal = !existingKeyframe.Bones[currentBoneIndex].TextureFlipHorizontal;
			}
		}

		private void textureFlipVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				AddUndo();
				ClearRedo();

				existingKeyframe.Bones[currentBoneIndex].TextureFlipVertical = !existingKeyframe.Bones[currentBoneIndex].TextureFlipVertical;
			}
		}

		private void addBoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			boneInfoForm.nameTextBox.Text = "";
			boneInfoForm.parentTextBox.Text = "";
			boneInfoForm.insertTextBox.Text = "";
			boneInfoForm.texturePathTextBox.Text = "";
            boneInfoForm.browseButton.Enabled = true;

			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				boneInfoForm.parentTextBox.Text = existingKeyframe.Bones[currentBoneIndex].Name;
				boneInfoForm.insertTextBox.Text = existingKeyframe.Bones[currentBoneIndex].Name;
			}

			if (keyframes[0].Bones.Count == 0)
			{
				boneInfoForm.parentTextBox.ReadOnly = true;
				boneInfoForm.insertTextBox.ReadOnly = true;
			}
			else
			{
				boneInfoForm.parentTextBox.ReadOnly = false;
				boneInfoForm.insertTextBox.ReadOnly = false;
			}

			if (boneInfoForm.ShowDialog() == DialogResult.OK)
			{
				AddUndo();
				ClearRedo();

				int insertIndex = -1;
				int parentIndex = -1;

				for (int boneIndex = 0; boneIndex < keyframes[0].Bones.Count; boneIndex++)
				{
					if (keyframes[0].Bones[boneIndex].Name == boneInfoForm.parentTextBox.Text)
						parentIndex = boneIndex;
					if (keyframes[0].Bones[boneIndex].Name == boneInfoForm.insertTextBox.Text)
					{
						if (boneInfoForm.beforeRadioButton.Checked)
							insertIndex = boneIndex;
						else
							insertIndex = boneIndex + 1;
					}
				}

				if ((insertIndex == -1 && keyframes[0].Bones.Count != 0) || (parentIndex == -1 && boneInfoForm.parentTextBox.Text != ""))
				{
					MessageBox.Show("Error occured while adding bone. Please check parent and insert names and try again");
					return;
				}

				insertIndex = Math.Max(0, insertIndex);

				int textureIndex = LoadTexture(boneInfoForm.texturePathTextBox.Text);
				Bone bone = new Bone(boneInfoForm.nameTextBox.Text, textureIndex, parentIndex);

				foreach (Keyframe kf in keyframes)
				{
					kf.Bones.Insert(insertIndex, new Bone(bone));

					foreach (Bone b in kf.Bones)
					{
						if (b.ParentIndex >= insertIndex)
							b.ParentIndex++;
					}

					kf.SortBones();
					kf.UpdateBones();
				}

				currentBoneIndex = insertIndex;
			}
		}

		private void removeBoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				AddUndo();
				ClearRedo();

				int parentIndex = keyframes[0].Bones[currentBoneIndex].ParentIndex;

				foreach (Keyframe kf in keyframes)
				{
					kf.Bones.RemoveAt(currentBoneIndex);

					foreach (Bone b in kf.Bones)
					{
						if (b.ParentIndex == currentBoneIndex)
							b.ParentIndex = parentIndex;
						else if (b.ParentIndex > currentBoneIndex)
							b.ParentIndex--;
					}

					kf.SortBones();
					kf.UpdateBones();
				}

				if (parentIndex > currentBoneIndex)
					parentIndex--;
				currentBoneIndex = parentIndex;
			}
		}

		private void changeBoneInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				boneInfoForm.nameTextBox.Text = existingKeyframe.Bones[currentBoneIndex].Name;
				if (existingKeyframe.Bones[currentBoneIndex].ParentIndex == -1)
					boneInfoForm.parentTextBox.Text = "";
				else
					boneInfoForm.parentTextBox.Text = existingKeyframe.Bones[existingKeyframe.Bones[currentBoneIndex].ParentIndex].Name;
				boneInfoForm.parentTextBox.ReadOnly = false;
				boneInfoForm.insertTextBox.Text = "";
				boneInfoForm.insertTextBox.ReadOnly = true;
				boneInfoForm.texturePathTextBox.Text = boneTextureFileNames[existingKeyframe.Bones[currentBoneIndex].TextureIndex];
                boneInfoForm.browseButton.Enabled = false;

				if (boneInfoForm.ShowDialog() == DialogResult.OK)
				{
					AddUndo();
					ClearRedo();

					int parentIndex = -1;

					for (int boneIndex = 0; boneIndex < keyframes[0].Bones.Count; boneIndex++)
					{
						if (keyframes[0].Bones[boneIndex].Name == boneInfoForm.parentTextBox.Text)
							parentIndex = boneIndex;
					}

					foreach (Keyframe kf in keyframes)
					{
						kf.Bones[currentBoneIndex].ParentIndex = parentIndex;

						kf.SortBones();
						kf.UpdateBones();
					}
				}
			}
		}

		private void moveForwardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				if (currentBoneIndex < keyframes[0].Bones.Count - 1)
				{
					AddUndo();
					ClearRedo();

					foreach (Keyframe kf in keyframes)
					{
						Bone b = kf.Bones[currentBoneIndex];
						kf.Bones.RemoveAt(currentBoneIndex);
						kf.Bones.Insert(currentBoneIndex + 1, b);

						foreach (Bone bone in kf.Bones)
						{
							if (bone.ParentIndex == currentBoneIndex)
								bone.ParentIndex = currentBoneIndex + 1;
							else if (bone.ParentIndex == currentBoneIndex + 1)
								bone.ParentIndex = currentBoneIndex;
						}

						kf.SortBones();
						kf.UpdateBones();
					}

					currentBoneIndex = currentBoneIndex + 1;
				}
			}
		}

		private void moveBackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				if (currentBoneIndex > 0)
				{
					AddUndo();
					ClearRedo();

					foreach (Keyframe kf in keyframes)
					{
						Bone b = kf.Bones[currentBoneIndex];
						kf.Bones.RemoveAt(currentBoneIndex);
						kf.Bones.Insert(currentBoneIndex - 1, b);

						foreach (Bone bone in kf.Bones)
						{
							if (bone.ParentIndex == currentBoneIndex)
								bone.ParentIndex = currentBoneIndex - 1;
							else if (bone.ParentIndex == currentBoneIndex - 1)
								bone.ParentIndex = currentBoneIndex;
						}

						kf.SortBones();
						kf.UpdateBones();
					}

					currentBoneIndex = currentBoneIndex - 1;
				}
			}
		}

		void NextBone()
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				currentBoneIndex++;
				if (currentBoneIndex >= existingKeyframe.Bones.Count)
					currentBoneIndex = 0;
			}
		}

		void PreviousBone()
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				currentBoneIndex--;
				if (currentBoneIndex < 0)
					currentBoneIndex = existingKeyframe.Bones.Count - 1;
			}
		}

		void NextTexture()
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				AddUndo();
				ClearRedo();

				existingKeyframe.Bones[currentBoneIndex].TextureIndex++;
				if (existingKeyframe.Bones[currentBoneIndex].TextureIndex >= boneTextures.Count)
					existingKeyframe.Bones[currentBoneIndex].TextureIndex = 0;
			}
		}

		void PreviousTexture()
		{
			if (atExistingKeyframe && existingKeyframe != null && currentBoneIndex != -1)
			{
				AddUndo();
				ClearRedo();

				existingKeyframe.Bones[currentBoneIndex].TextureIndex--;
				if (existingKeyframe.Bones[currentBoneIndex].TextureIndex < 0)
					existingKeyframe.Bones[currentBoneIndex].TextureIndex = boneTextures.Count - 1;
			}
		}

		private void saveAnimationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(savePath))
				saveAsToolStripMenuItem_Click(sender, e);
			else
				Save(savePath);
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveAnimationDialog.ShowDialog() == DialogResult.OK)
			{
				savePath = saveAnimationDialog.FileName;
				Save(savePath);
			}
		}

		private void exportScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "png files|*.png";
			sfd.OverwritePrompt = true;

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				ExportCurrentFrame(sfd.FileName);
			}
		}

		private void exportAnimationFramesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (exportAnimationForm.ShowDialog() == DialogResult.OK)
			{
				string filename = exportAnimationForm.nameTextBox.Text;
				string path = exportAnimationForm.pathTextBox.Text;
				int frameInterval = (int)exportAnimationForm.intervalUpDown.Value;
				int startFrame = currentKeyframe;

				int maxFrame = loopFrame != -1 ? loopFrame - 1 : keyframes[keyframes.Count - 1].FrameNumber;

				int frameCount = 0;
				for (int currentFrame = 0; currentFrame <= maxFrame; currentFrame += frameInterval)
				{
					UpdateCurrentKeyframe(currentFrame);
					ExportCurrentFrame(path + "/" + filename + frameCount.ToString("D3") + ".png");
					frameCount++;
				}

				UpdateCurrentKeyframe(startFrame);
			}
		}

		private void ExportCurrentFrame(string filename)
		{
			string zoomString = zoomComboBox.SelectedItem.ToString();
			System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(zoomString, @"[0-9]+");
			float zoom = (float)int.Parse(m.Value);

			Keyframe keyframeToDraw;

			if (atExistingKeyframe && existingKeyframe != null)
				keyframeToDraw = existingKeyframe;
			else
				keyframeToDraw = interpolatedKeyframe;

			spriteBatch.GraphicsDevice.SetRenderTarget(previousRenderTarget);
			spriteBatch.GraphicsDevice.Clear(Color.Transparent);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(zoom / 100 * (keyframeToDraw.FlipHorizontally ? -1 : 1), zoom / 100 * (keyframeToDraw.FlipVertically ? -1 : 1), 1) * Matrix.CreateTranslation(xnaDisplay.Width / 2, xnaDisplay.Height / 2, 0));

			foreach (Bone bone in keyframeToDraw.Bones)
			{
				if (bone.Hidden)
					continue;

				SpriteEffects spriteEffects = SpriteEffects.None;
				if (bone.TextureFlipHorizontal)
					spriteEffects = SpriteEffects.FlipHorizontally;
				if (bone.TextureFlipVertical)
					spriteEffects |= SpriteEffects.FlipVertically;

				spriteBatch.Draw(boneTextures[bone.TextureIndex], bone.TransformedPosition, null, Color.White, bone.TransformedRotation, new Vector2(boneTextures[bone.TextureIndex].Width / 2, boneTextures[bone.TextureIndex].Height / 2), bone.TransformedScale, spriteEffects, 0);
			}

			spriteBatch.End();

			spriteBatch.GraphicsDevice.SetRenderTarget(null);

			using (FileStream fileStream = File.Open(filename, FileMode.OpenOrCreate))
			{
				previousRenderTarget.SaveAsPng(fileStream, previousRenderTarget.Width, previousRenderTarget.Height);
			}
		}

		private void openAnimationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dirtyFlag)
			{
				DialogResult dialogResult = MessageBox.Show("Unsaved changes will be lost. Do you want to save?",
					"Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
				switch (dialogResult)
				{
					case DialogResult.Cancel:
						return;
					case DialogResult.No:
						break;
					case DialogResult.Yes:
						if (string.IsNullOrEmpty(savePath))
							saveAsToolStripMenuItem_Click(sender, e);
						else
							Save(savePath);
						break;
				}
			}

			dirtyFlag = false;

			if (openAnimationDialog.ShowDialog() == DialogResult.OK)
			{
				OpenAnimation(openAnimationDialog.FileName);
			}
		}

		void OpenAnimation(string animationFile)
		{
			ClearUndo();
			ClearRedo();

			savePath = animationFile;

			NewAnimation();
			keyframes.Clear();

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(animationFile);

			frameRate = Int32.Parse(xmlDocument.SelectSingleNode("/Animation/FrameRate").InnerText);
			frameRateComboBox.SelectedItem = frameRate.ToString();

			loopFrame = Int32.Parse(xmlDocument.SelectSingleNode("/Animation/LoopFrame").InnerText);

			XmlNodeList nodeList = xmlDocument.SelectNodes("/Animation/Texture");
			foreach (XmlNode node in nodeList)
			{
				LoadTexture(node.InnerText, animationFile);
			}

			nodeList = xmlDocument.SelectNodes("/Animation/Keyframe");
			foreach (XmlNode node in nodeList)
			{
				Keyframe keyframe = new Keyframe(Int32.Parse(node.Attributes["frame"].Value));
				if (node.Attributes["trigger"] != null)
					keyframe.Trigger = node.Attributes["trigger"].Value;
				else
					keyframe.Trigger = "";

				if (node.Attributes["vflip"] != null)
					keyframe.FlipVertically = bool.Parse(node.Attributes["vflip"].Value);
				else
					keyframe.FlipVertically = false;

				if (node.Attributes["hflip"] != null)
					keyframe.FlipHorizontally = bool.Parse(node.Attributes["hflip"].Value);
				else
					keyframe.FlipHorizontally = false;

				XmlNodeList boneList = node.SelectNodes("Bone");
				foreach (XmlNode boneNode in boneList)
				{
					Bone bone = new Bone(boneNode.Attributes["name"].Value,
						Int32.Parse(boneNode.SelectSingleNode("TextureIndex").InnerText),
						Int32.Parse(boneNode.SelectSingleNode("ParentIndex").InnerText));

					bone.Hidden = bool.Parse(boneNode.SelectSingleNode("Hidden").InnerText);

					XmlNode tempNode = boneNode.SelectSingleNode("TextureFlipHorizontal");
					if (tempNode != null)
						bone.TextureFlipHorizontal = bool.Parse(tempNode.InnerText);
					tempNode = boneNode.SelectSingleNode("TextureFlipVertical");
					if (tempNode != null)
						bone.TextureFlipVertical = bool.Parse(tempNode.InnerText);

					bone.Position = new Vector2(float.Parse(boneNode.SelectSingleNode("Position/X").InnerText, CultureInfo.InvariantCulture),
						float.Parse(boneNode.SelectSingleNode("Position/Y").InnerText, CultureInfo.InvariantCulture));
					bone.Rotation = float.Parse(boneNode.SelectSingleNode("Rotation").InnerText, CultureInfo.InvariantCulture);
					bone.Scale = new Vector2(float.Parse(boneNode.SelectSingleNode("Scale/X").InnerText, CultureInfo.InvariantCulture),
						float.Parse(boneNode.SelectSingleNode("Scale/Y").InnerText, CultureInfo.InvariantCulture));

					keyframe.AddBone(bone);
				}
				keyframe.SortBones();
				keyframe.UpdateBones();

				keyframes.Add(keyframe);
			}

			if (keyframes[0].Bones.Count != 0)
			{
				currentBoneIndex = 0;
				currentKeyframe = 0;
				atExistingKeyframe = true;
				existingKeyframe = keyframes[0];
			}
		}

		private void setLoopPointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!atExistingKeyframe)
			{
				if (loopFrame == currentKeyframe)
					loopFrame = -1;
				else
					loopFrame = currentKeyframe;
			}
		}

		private void moveRootToOriginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null)
			{
				Bone bone = existingKeyframe.Bones.Find(b => b.ParentIndex == -1);
				if (bone != null)
				{
					AddUndo();
					ClearRedo();

					bone.Position = Vector2.Zero;
					existingKeyframe.UpdateBones();
				}
			}
		}

		private void setFramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null)
			{
				TriggerStringForm tgs = new TriggerStringForm();
				if (tgs.ShowDialog() == DialogResult.OK)
				{
					AddUndo();
					ClearRedo();

					existingKeyframe.Trigger = tgs.triggerStringTextBox.Text;
				}
			}
		}

		private void clearFrameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null)
			{
				AddUndo();
				ClearRedo();

				existingKeyframe.Trigger = "";
			}
		}

		private void flipFrameVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null)
			{
				AddUndo();
				ClearRedo();

				existingKeyframe.FlipVertically = !existingKeyframe.FlipVertically;
			}
		}

		private void flipHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (atExistingKeyframe && existingKeyframe != null)
			{
				AddUndo();
				ClearRedo();

				existingKeyframe.FlipHorizontally = !existingKeyframe.FlipHorizontally;
			}
		}

		private void playToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string exePath = Path.GetDirectoryName(Application.ExecutablePath);
			string tempPath = Path.GetFullPath(Path.Combine(exePath, "temp"));

			Directory.CreateDirectory(tempPath);

			string fileName = Path.Combine(tempPath, "temp.anim");
			Save(fileName);

			string executable = Path.Combine(exePath, "DeminaViewer.exe");

            string zoomString = zoomComboBox.SelectedItem.ToString();
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(zoomString, @"[0-9]+");
            int zoom = int.Parse(m.Value);

			System.Diagnostics.Process.Start(executable, '"' + fileName + '"' + " " + zoom.ToString());
		}

		private void addUnassignedTextureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "PNG Files|*.png";
			ofd.CheckFileExists = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				if (LoadTexture(ofd.FileName) == -1)
				{
					MessageBox.Show("Failed to load texture.");
				}
			}
		}

		void zoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		void frameRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			frameRate = int.Parse((string)frameRateComboBox.SelectedItem);
		}

		private void packAnimationsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AnimationPacker ap = new AnimationPacker();
			ap.Show();
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CanUndo())
				PerformUndo();
		}

		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CanRedo())
				PerformRedo();
		}

		private void reloadTexturesToolStripMenuItem_Click(object sender, EventArgs e)
		{
            boneTexturePixels.Clear();

			for(int i=0; i<boneTextureFileNames.Count; i++)
			{
				Texture2D texture;
				Color[] pixels;

				LoadPremultipledAlphaTexture(boneTextureFileNames[i], out texture, out pixels);
                boneTexturePixels.Add(pixels);
			}
		}

		void LoadPremultipledAlphaTexture(string texturePath, out Texture2D premultipliedTexture, out Color[] originalPixels)
		{
			using (FileStream fileStream = File.Open(texturePath, FileMode.Open))
			{
				using (Texture2D originalTexture = Texture2D.FromStream(xnaDisplay.GraphicsDevice, fileStream))
				{
					originalPixels = new Color[originalTexture.Width * originalTexture.Height];
					originalTexture.GetData<Color>(originalPixels);

					Color[] premultipliedPixels = new Color[originalTexture.Width * originalTexture.Height];

					for (int i = 0; i < originalPixels.Length; i++)
					{
						premultipliedPixels[i].R = (byte)(originalPixels[i].R * originalPixels[i].A / 255);
						premultipliedPixels[i].G = (byte)(originalPixels[i].G * originalPixels[i].A / 255);
						premultipliedPixels[i].B = (byte)(originalPixels[i].B * originalPixels[i].A / 255);
						premultipliedPixels[i].A = originalPixels[i].A;
					}

					premultipliedTexture = new Texture2D(xnaDisplay.GraphicsDevice, originalTexture.Width, originalTexture.Height, false, SurfaceFormat.Color);
					premultipliedTexture.SetData<Color>(premultipliedPixels);
				}
			}
		}
	
		int LoadTexture(string texturePath, string absolutePath)
		{
			if (!Path.IsPathRooted(texturePath))
			{
				texturePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(absolutePath), texturePath));
			}

			int textureIndex = boneTextureFileNames.FindIndex(s => s == texturePath);
			if (textureIndex == -1)
			{
				Texture2D texture;
				Color[] pixels;

				LoadPremultipledAlphaTexture(texturePath, out texture, out pixels);

				boneTexturePixels.Add(pixels);
				boneTextures.Add(texture);
				boneTextureFileNames.Add(texturePath);
				textureIndex = boneTextures.Count - 1;
			}

			return textureIndex;
		}

		int LoadTexture(string absolutePath)
		{
			int textureIndex = boneTextureFileNames.FindIndex(s => s == absolutePath);
			if (textureIndex == -1)
			{
				Texture2D texture;
				Color[] pixels;

				LoadPremultipledAlphaTexture(absolutePath, out texture, out pixels);

				boneTextures.Add(texture);
				boneTextureFileNames.Add(absolutePath);
				boneTexturePixels.Add(pixels);

				textureIndex = boneTextures.Count - 1;
			}

			return textureIndex;
		}

		void Save(string fileName)
		{
			XmlTextWriter textWriter = new XmlTextWriter(fileName, null);

			textWriter.Formatting = Formatting.Indented;

			textWriter.WriteStartDocument();

			textWriter.WriteStartElement("Animation");

			textWriter.WriteStartElement("FrameRate");
            textWriter.WriteString(frameRate.ToString(CultureInfo.InvariantCulture));
			textWriter.WriteEndElement();

			textWriter.WriteStartElement("LoopFrame");
            textWriter.WriteString(loopFrame.ToString(CultureInfo.InvariantCulture));
			textWriter.WriteEndElement();

			foreach(string textureFile in boneTextureFileNames)
			{
				textWriter.WriteStartElement("Texture");
				textWriter.WriteString(Util.CreateRelativePath(textureFile, fileName));
				textWriter.WriteEndElement();
			}

			foreach (Keyframe keyframe in keyframes)
			{
				textWriter.WriteStartElement("Keyframe");
                textWriter.WriteAttributeString("frame", keyframe.FrameNumber.ToString(CultureInfo.InvariantCulture));

				textWriter.WriteAttributeString("trigger", keyframe.Trigger);

                textWriter.WriteAttributeString("vflip", keyframe.FlipVertically.ToString(CultureInfo.InvariantCulture));
                textWriter.WriteAttributeString("hflip", keyframe.FlipHorizontally.ToString(CultureInfo.InvariantCulture));

				foreach (Bone bone in keyframe.Bones)
				{
					textWriter.WriteStartElement("Bone");
					textWriter.WriteAttributeString("name", bone.Name);

					textWriter.WriteStartElement("Hidden");
                    textWriter.WriteString(bone.Hidden.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();

					textWriter.WriteStartElement("TextureFlipHorizontal");
                    textWriter.WriteString(bone.TextureFlipHorizontal.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();

					textWriter.WriteStartElement("TextureFlipVertical");
                    textWriter.WriteString(bone.TextureFlipVertical.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();

					textWriter.WriteStartElement("ParentIndex");
                    textWriter.WriteString(bone.ParentIndex.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();

					textWriter.WriteStartElement("TextureIndex");
                    textWriter.WriteString(bone.TextureIndex.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();

					textWriter.WriteStartElement("Position");
					textWriter.WriteStartElement("X");
                    textWriter.WriteString(bone.Position.X.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();
					textWriter.WriteStartElement("Y");
                    textWriter.WriteString(bone.Position.Y.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();
					textWriter.WriteEndElement();

					textWriter.WriteStartElement("Rotation");
                    textWriter.WriteString(bone.Rotation.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();

					textWriter.WriteStartElement("Scale");
					textWriter.WriteStartElement("X");
                    textWriter.WriteString(bone.Scale.X.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();
					textWriter.WriteStartElement("Y");
                    textWriter.WriteString(bone.Scale.Y.ToString(CultureInfo.InvariantCulture));
					textWriter.WriteEndElement();
					textWriter.WriteEndElement();

					textWriter.WriteEndElement();
				}

				textWriter.WriteEndElement();
			}

			textWriter.WriteEndElement();

			textWriter.WriteEndDocument();

			textWriter.Close();
		}

		void AddUndo()
		{
			dirtyFlag = true;

			List<Keyframe> undoEntry = new List<Keyframe>();

			foreach (Keyframe k in keyframes)
			{
				undoEntry.Add(new Keyframe(k));
			}

			undoStack.Push(undoEntry);
		}

		void AddRedo()
		{
			dirtyFlag = true;

			List<Keyframe> redoEntry = new List<Keyframe>();

			foreach (Keyframe k in keyframes)
			{
				redoEntry.Add(new Keyframe(k));
			}

			redoStack.Push(redoEntry);
		}

		void ClearUndo()
		{
			undoStack.Clear();
		}

		void ClearRedo()
		{
			redoStack.Clear();
		}

		bool CanUndo()
		{
			return undoStack.Count > 0;
		}

		bool CanRedo()
		{
			return redoStack.Count > 0;
		}

		void PerformUndo()
		{
			AddRedo();
			keyframes = undoStack.Pop();
			UpdateCurrentKeyframe(currentKeyframe);
		}

		void PerformRedo()
		{
			AddUndo();
			keyframes = redoStack.Pop();
			UpdateCurrentKeyframe(currentKeyframe);
		}

		Stack<List<Keyframe>> undoStack = new Stack<List<Keyframe>>();
		Stack<List<Keyframe>> redoStack = new Stack<List<Keyframe>>();

		SpriteBatch spriteBatch;
		ContentManager contentManager;
		RenderTarget2D previousRenderTarget;
		RenderTarget2D nextRenderTarget;

		Texture2D blankTexture;
		Texture2D keyframeArrow;
		Texture2D loopArrow;
		Texture2D leftArrow, rightArrow;

        int horizontalLinePosition1 = -1, horizontalLinePosition2 = -1;
        int verticalLinePosition1 = -1, verticalLinePosition2 = -1;

		int leftKeyframe = 0;

		Effect redMaskEffect;
		Effect onionSkinEffect;

		List<Keyframe> keyframes;
		List<int> previouslySelectedBones = new List<int>();

		bool dirtyFlag = false;

		string xmlPath;
		string savePath;
		List<Texture2D> boneTextures;
		List<string> boneTextureFileNames;
		List<Color[]> boneTexturePixels;

		string startupAnimation;

		SpriteFont font;

		int currentKeyframe;
		int currentKeyframeIndex;
		int currentBoneIndex;

		int loopFrame;

		int frameRate = 25;

		bool atExistingKeyframe;
		Keyframe existingKeyframe;
		Keyframe interpolatedKeyframe;
		Keyframe copiedKeyframe;

		bool middleMouseDown = false;
		bool rightMouseDown = false;
		Point lastMousePosition;

		bool currentlyTranslating = false;
		bool currentlyRotating = false;

		BoneInfoForm boneInfoForm = new BoneInfoForm();
		ExportAnimationForm exportAnimationForm = new ExportAnimationForm();

		const int KEYFRAME_BOX_SIZE = 12;
		const int KEYFRAME_BOX_PADDING = 2;
		const int KEYFRAME_BOX_LEFT_MARGIN = 16;
		const int KEYFRAME_BOX_RIGHT_MARGIN = 16;
		const int KEYFRAME_BOX_BOTTOM_MARGIN = 16;
		const int KEYFRAME_LABEL_OFFSET_Y = 42;

        /// <summary>
        /// Will copy the position, rotation, and tex flip information from the currently selected bone to all
        /// keyframes. This is very helpful when you add a new bone to an existing animation and don't want
        /// to have to position it on all keyframes manually to start (or edit the XML directly)
        /// </summary>
        private void copyBoneInformationToAllKeyframesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //First, grab the bone information for the currently selected bone (if the bone is valid)
            if (atExistingKeyframe && existingKeyframe != null & currentBoneIndex != -1)
            {
                //grab the current bone on this keyframe
                Bone bone = existingKeyframe.Bones[currentBoneIndex];

                //now loop through all keyframes, and apply the current bone position and rotation to our bone
                foreach (Keyframe frame in keyframes)
                {
                    frame.Bones[currentBoneIndex].Position = bone.Position;
                    frame.Bones[currentBoneIndex].Rotation = bone.Rotation;
                    frame.Bones[currentBoneIndex].TextureFlipHorizontal = bone.TextureFlipHorizontal;
                    frame.Bones[currentBoneIndex].TextureFlipVertical = bone.TextureFlipVertical;
                }

            }
        }
	}
}
