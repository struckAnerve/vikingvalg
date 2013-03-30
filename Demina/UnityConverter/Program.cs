using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace UnityConverter
{
    class Program
    {
        struct Rectangle
        {
            public int x;
            public int y;
            public int width;
            public int height;

            public Rectangle(int x, int y, int w, int h)
            {
                this.x = x;
                this.y = y;
                width = w;
                height = h;
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage:\nUnityConverter input_animation texture_dictionary output_xml");
                return;
            }

            XmlDocument inputDocument = new XmlDocument();
            inputDocument.Load(args[0]);

            // texture dictionary code copied directly from the spritesheetpacker codeplex site
            Dictionary<string, Rectangle> spriteSourceRectangles = new Dictionary<string, Rectangle>();

            using (StreamReader reader = new StreamReader(args[1]))
            {
                // while we're not done reading...
                while (!reader.EndOfStream)
                {
                    // get a line
                    string line = reader.ReadLine();

                    // split at the equals sign
                    string[] sides = line.Split('=');

                    // trim the right side and split based on spaces
                    string[] rectParts = sides[1].Trim().Split(' ');

                    // create a rectangle from those parts
                    Rectangle r = new Rectangle(
                       int.Parse(rectParts[0]),
                       int.Parse(rectParts[1]),
                       int.Parse(rectParts[2]),
                       int.Parse(rectParts[3]));

                    // add the name and rectangle to the dictionary
                    spriteSourceRectangles.Add(sides[0].Trim(), r);
                }
            }

            XmlNode animationNode = inputDocument.SelectSingleNode("Animation");
            XmlNodeList textureNodes = animationNode.SelectNodes("Texture");
            foreach (XmlNode node in textureNodes)
            {
                Rectangle rect = spriteSourceRectangles[Path.GetFileNameWithoutExtension(node.InnerText)];
                XmlElement rectElement = inputDocument.CreateElement("TextureRect");
                XmlAttribute attribute = inputDocument.CreateAttribute("x");
                attribute.Value = rect.x.ToString();
                rectElement.Attributes.Append(attribute);
                attribute = inputDocument.CreateAttribute("y");
                attribute.Value = rect.y.ToString();
                rectElement.Attributes.Append(attribute);
                attribute = inputDocument.CreateAttribute("width");
                attribute.Value = rect.width.ToString();
                rectElement.Attributes.Append(attribute);
                attribute = inputDocument.CreateAttribute("height");
                attribute.Value = rect.height.ToString();
                rectElement.Attributes.Append(attribute);
                animationNode.AppendChild(rectElement);
            }

            XmlNodeList rotationNodes = animationNode.SelectNodes("Keyframe/Bone");
            foreach (XmlNode node in rotationNodes)
            {
                XmlNode radianNode = node.SelectSingleNode("Rotation");
                XmlNode angleNode = inputDocument.CreateElement("RotationDegrees");
                angleNode.InnerText = (180.0f / 3.14159f * float.Parse(radianNode.InnerText)).ToString();
                node.AppendChild(angleNode);
            }

            inputDocument.Save(args[2]);
        }
    }
}
