// note:  This file makes heavy use of code from the SpriteSheetPacker project
//   http://spritesheetpacker.codeplex.com/
//   which itself uses code form the Nuclex Framework
//   http://nuclexframework.codeplex.com/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Drawing.Imaging;

namespace Demina
{
	public partial class AnimationPacker : Form
	{
		public AnimationPacker()
		{
			InitializeComponent();
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			if (addFilesDialog.ShowDialog() == DialogResult.OK)
			{
				foreach (string f in addFilesDialog.FileNames)
				{
					if (f.EndsWith(".anim"))
					{
						if (!animationFiles.Contains(f))
						{
							animationFiles.Add(f);
							filesListBox.Items.Add(f);
						}
					}
				}
			}
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			List<string> filesToRemove = new List<string>();

			foreach (var f in filesListBox.SelectedItems)
				filesToRemove.Add((string)f);

			animationFiles.RemoveAll(filesToRemove.Contains);
			filesToRemove.ForEach(f => filesListBox.Items.Remove(f));
		}

		private void textureButton_Click(object sender, EventArgs e)
		{
			saveFileDialog.Filter = "PNG Files|*.png";
			saveFileDialog.FileName = "";

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				textureTextBox.Text = saveFileDialog.FileName;
			}
		}

		private void dictionaryButton_Click(object sender, EventArgs e)
		{
			saveFileDialog.Filter = "Texture Dictionary Files|*.tdict";
			saveFileDialog.FileName = "";

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				dictionaryTextBox.Text = saveFileDialog.FileName;
			}
		}

		private void buildButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textureTextBox.Text) || !textureTextBox.Text.EndsWith(".png")
				|| string.IsNullOrEmpty(dictionaryTextBox.Text) || !dictionaryTextBox.Text.EndsWith(".tdict"))
			{
				// TODO: show error message!
				return;
			}

			List<string> textureFiles = new List<string>();
			TexturePacker texturePacker = new TexturePacker(MAXIMUM_WIDTH, MAXIMUM_HEIGHT, PADDING);

			foreach (string f in animationFiles)
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(f);

				XmlNodeList textureNodes = xmlDocument.SelectNodes("/Animation/Texture");
				foreach (XmlNode node in textureNodes)
				{
					string texturePath = node.InnerText;

					if (!Path.IsPathRooted(texturePath))
					{
						texturePath = Path.Combine(Path.GetDirectoryName(f), texturePath);
						texturePath = Path.GetFullPath(texturePath);
					}

					if (!textureFiles.Contains(texturePath))
						textureFiles.Add(texturePath);
				}
			}

			int texturesPacked;

			if (!texturePacker.PackTextures(textureFiles, textureTextBox.Text, dictionaryTextBox.Text, out texturesPacked))
			{
				// TODO: error message
				return;
			}

			foreach (string animFile in animationFiles)
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(animFile);

				XmlNodeList textureNodes = xmlDocument.SelectNodes("/Animation/Texture");

				foreach (XmlNode node in textureNodes)
				{
					XmlAttribute att = node.Attributes["dictionary"];
					if (att != null)
					{
						att.Value = Util.CreateRelativePath(dictionaryTextBox.Text, animFile);
					}
					else
					{
						att = xmlDocument.CreateAttribute("dictionary");
						att.Value = Util.CreateRelativePath(dictionaryTextBox.Text, animFile);
						node.Attributes.Append(att);
					}

					string texturePath = node.InnerText;

					if (!Path.IsPathRooted(texturePath))
					{
						texturePath = Path.Combine(Path.GetDirectoryName(animFile), texturePath);
						texturePath = Path.GetFullPath(texturePath);
					}

					att = node.Attributes["name"];
					if (att != null)
					{
						att.Value = texturePacker.TextureNames[texturePath];
					}
					else
					{
						att = xmlDocument.CreateAttribute("name");
						att.Value = texturePacker.TextureNames[texturePath];
						node.Attributes.Append(att);
					}

				}

				xmlDocument.Save(animFile);
			}
		}

		List<string> animationFiles = new List<string>();

		const int MAXIMUM_WIDTH = 2048;
		const int MAXIMUM_HEIGHT = 2048;
		const int PADDING = 2;
	}
}
