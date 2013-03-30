using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace Demina
{
	public class TexturePacker
	{
		public Dictionary<string, string> TextureNames { get { return textureNames; } }

		public TexturePacker(int maxWidth, int maxHeight, int padding)
		{
			this.maxWidth = maxWidth;
			this.maxHeight = maxHeight;
			this.padding = padding;
		}

		public bool PackTextures(List<string> textureFiles, string packedTexture, string textureDictionary, out int maxPacked)
		{
			maxPacked = 0;

			textureBounds.Clear();
			imagePlacement.Clear();
			textureNames.Clear();

			try
			{
				foreach (string file in textureFiles)
				{
					textureBounds[file] = FindBounds(file);
				}
			}
			catch
			{
				return false;
			}

			// sort our files by file size so we place large sprites first
			textureFiles.Sort(
				(f1, f2) =>
				{
					Size b1 = new Size(textureBounds[f1].Width, textureBounds[f1].Height);
					Size b2 = new Size(textureBounds[f2].Width, textureBounds[f2].Height);

					int c = -b1.Width.CompareTo(b2.Width);
					if (c != 0)
						return c;

					c = -b1.Height.CompareTo(b2.Height);
					if (c != 0)
						return c;

					return f1.CompareTo(f2);
				});

			List<string> textureNamesList = new List<string>();

			foreach (string f in textureFiles)
			{
				string name = Path.GetFileNameWithoutExtension(f);
				int count = 1;
				while (true)
				{
					if (!textureNamesList.Contains(name))
					{
						textureNamesList.Add(name);
						textureNames[f] = name;
						break;
					}

					name = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(f), count.ToString());
				}
			}

			outputWidth = maxWidth;
			outputHeight = maxHeight;

			if (!PackImageRectangles(textureFiles, out maxPacked))
			{
				return false;
			}

			Bitmap outputImage = CreateOutputImage(textureFiles);

			outputImage.Save(packedTexture, ImageFormat.Png);

			XmlTextWriter textWriter = new XmlTextWriter(textureDictionary, null);

			textWriter.Formatting = Formatting.Indented;

			textWriter.WriteStartDocument();
			textWriter.WriteStartElement("TextureDictionary");

			textWriter.WriteStartElement("TexturePath");
			textWriter.WriteString(Util.CreateRelativePath(packedTexture, textureDictionary));
			textWriter.WriteEndElement();

			foreach (string file in textureFiles)
			{
				textWriter.WriteStartElement("Texture");
				textWriter.WriteAttributeString("name", textureNames[file]);

				textWriter.WriteStartElement("X");
				textWriter.WriteString(imagePlacement[file].X.ToString());
				textWriter.WriteEndElement();
				textWriter.WriteStartElement("Y");
				textWriter.WriteString(imagePlacement[file].Y.ToString());
				textWriter.WriteEndElement();
				textWriter.WriteStartElement("Width");
				textWriter.WriteString(textureBounds[file].Width.ToString());
				textWriter.WriteEndElement();
				textWriter.WriteStartElement("Height");
				textWriter.WriteString(textureBounds[file].Height.ToString());
				textWriter.WriteEndElement();
				textWriter.WriteStartElement("OriginX");
				textWriter.WriteString(textureBounds[file].OriginX.ToString());
				textWriter.WriteEndElement();
				textWriter.WriteStartElement("OriginY");
				textWriter.WriteString(textureBounds[file].OriginY.ToString());
				textWriter.WriteEndElement();

				textWriter.WriteEndElement();
			}

			textWriter.WriteEndElement();
			textWriter.Close();

			return true;
		}

		TextureBounds FindBounds(string textureFile)
		{
			using (Bitmap bitmap = new Bitmap(textureFile))
			{
				int minX = int.MaxValue, maxX = int.MinValue;
				int minY = int.MaxValue, maxY = int.MinValue;

				for (int y = 0; y < bitmap.Height; y++)
				{
					int x = 0;
					for (; x < bitmap.Width; x++)
					{
						Color pixel = bitmap.GetPixel(x, y);
						if (pixel.A > 0)
						{
							minX = Math.Min(x, minX);
							minY = Math.Min(y, minY);
							maxX = Math.Max(x, maxX);
							maxY = Math.Max(y, maxY);
							break;
						}
					}
					for (int z = bitmap.Width - 1; z > x; z--)
					{
						Color pixel = bitmap.GetPixel(z, y);
						if (pixel.A > 0)
						{
							minX = Math.Min(z, minX);
							minY = Math.Min(y, minY);
							maxX = Math.Max(z, maxX);
							maxY = Math.Max(y, maxY);
							break;
						}
					}
				}

				if (minX == int.MaxValue || maxX == int.MinValue || minY == int.MaxValue || maxY == int.MinValue)
				{
					throw new Exception("Completely transparent image encountered.");
				}

				TextureBounds tb = new TextureBounds();

				tb.X = minX;
				tb.Y = minY;
				tb.Width = maxX - minX + 1;
				tb.Height = maxY - minY + 1;
				tb.OriginX = bitmap.Width / 2 - minX;
				tb.OriginY = bitmap.Height / 2 - minY;

				return tb;
			}
		}

		private Bitmap CreateOutputImage(List<string> files)
		{
			try
			{
				Bitmap outputImage = new Bitmap(outputWidth, outputHeight, PixelFormat.Format32bppArgb);

				// draw all the images into the output image
				foreach (var image in files)
				{
					Rectangle location = imagePlacement[image];
					Bitmap bitmap = Bitmap.FromFile(image) as Bitmap;
					if (bitmap == null)
						return null;

					TextureBounds bounds = textureBounds[image];

					// copy pixels over to avoid antialiasing or any other side effects of drawing
					// the subimages to the output image using Graphics
					for (int x = 0; x < bounds.Width; x++)
						for (int y = 0; y < bounds.Height; y++)
							outputImage.SetPixel(location.X + x, location.Y + y, bitmap.GetPixel(x + bounds.X, y + bounds.Y));
				}

				return outputImage;
			}
			catch
			{
				return null;
			}
		}


		// This method does some trickery type stuff where we perform the TestPackingImages method over and over, 
		// trying to reduce the image size until we have found the smallest possible image we can fit.
		private bool PackImageRectangles(List<string> textureFiles, out int maxPacked)
		{
			maxPacked = 0;

			// create a dictionary for our test image placements
			Dictionary<string, Rectangle> testImagePlacement = new Dictionary<string, Rectangle>();

			// get the size of our smallest image
			int smallestWidth = int.MaxValue;
			int smallestHeight = int.MaxValue;
			foreach (var bounds in textureBounds)
			{
				smallestWidth = Math.Min(smallestWidth, bounds.Value.Width);
				smallestHeight = Math.Min(smallestHeight, bounds.Value.Height);
			}

			// we need a couple values for testing
			int testWidth = outputWidth;
			int testHeight = outputHeight;

			bool shrinkVertical = false;

			// just keep looping...
			while (true)
			{
				// make sure our test dictionary is empty
				testImagePlacement.Clear();

				int packedCount;

				// try to pack the images into our current test size
				if (!TestPackingImages(textureFiles, testWidth, testHeight, testImagePlacement, out packedCount))
				{
					maxPacked = Math.Max(packedCount, maxPacked);
					// if that failed...

					// if we have no images in imagePlacement, i.e. we've never succeeded at PackImages,
					// show an error and return false since there is no way to fit the images into our
					// maximum size texture
					if (imagePlacement.Count == 0)
						return false;

					// otherwise return true to use our last good results
					if (shrinkVertical)
						return true;

					shrinkVertical = true;
					testWidth += smallestWidth + padding + padding;
					testHeight += smallestHeight + padding + padding;
					continue;
				}
				maxPacked = Math.Max(packedCount, maxPacked);

				// clear the imagePlacement dictionary and add our test results in
				imagePlacement.Clear();
				foreach (var pair in testImagePlacement)
					imagePlacement.Add(pair.Key, pair.Value);

				// figure out the smallest bitmap that will hold all the images
				testWidth = testHeight = 0;
				foreach (var pair in imagePlacement)
				{
					testWidth = Math.Max(testWidth, pair.Value.Right);
					testHeight = Math.Max(testHeight, pair.Value.Bottom);
				}

				// subtract the extra padding on the right and bottom
				if (!shrinkVertical)
					testWidth -= padding;
				testHeight -= padding;

				// if the test results are the same as our last output results, we've reached an optimal size,
				// so we can just be done
				if (testWidth == outputWidth && testHeight == outputHeight)
				{
					if (shrinkVertical)
						return true;

					shrinkVertical = true;
				}

				// save the test results as our last known good results
				outputWidth = testWidth;
				outputHeight = testHeight;

				// subtract the smallest image size out for the next test iteration
				if (!shrinkVertical)
					testWidth -= smallestWidth;
				testHeight -= smallestHeight;
			}
		}

		private bool TestPackingImages(List<string> files, int testWidth, int testHeight, Dictionary<string, Rectangle> testImagePlacement, out int packedCount)
		{
			packedCount = 0;

			// create the rectangle packer
			sspack.ArevaloRectanglePacker rectanglePacker = new sspack.ArevaloRectanglePacker(testWidth, testHeight);

			foreach (var image in files)
			{
				// get the bitmap for this file
				Size size = new Size(textureBounds[image].Width, textureBounds[image].Height);

				// pack the image
				Point origin;
				if (!rectanglePacker.TryPack(size.Width + padding, size.Height + padding, out origin))
				{
					return false;
				}

				packedCount++;

				// add the destination rectangle to our dictionary
				testImagePlacement.Add(image, new Rectangle(origin.X, origin.Y, size.Width + padding, size.Height + padding));
			}

			return true;
		}

		Dictionary<string, TextureBounds> textureBounds = new Dictionary<string, TextureBounds>();
		Dictionary<string, Rectangle> imagePlacement = new Dictionary<string, Rectangle>();
		Dictionary<string, string> textureNames = new Dictionary<string, string>();

		int maxWidth, maxHeight, padding;
		int outputWidth, outputHeight;
	}

	struct TextureBounds
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;
		public int OriginX;
		public int OriginY;
	}
}
