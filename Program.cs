using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RubikCubeImageRender
{
    class Program
    {
        static Dictionary<Char, Color> colorMap = new Dictionary<char, Color>();

        static void initColorMap()
        {
            colorMap['R'] = Color.Red;
            colorMap['G'] = Color.LawnGreen;
            colorMap['B'] = Color.BlueViolet;
            colorMap['O'] = Color.Orange;
            colorMap['Y'] = Color.Yellow;
            colorMap['W'] = Color.White;
            colorMap['X'] = Color.Gray;
        }

        static void drawAndSave(string filename, Model model, string colorCode)
        {
            int width = 400;
            int height = 400;

            // Create a Bitmap object from a file.
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);


            for (int i = 0; i < colorCode.Length; i++)
            {
                Char colorChar = colorCode[i];
                Color color = colorMap[colorChar];
                Point[] points = model.GetPolygen(i);
                Point[] scaledPoints = new Point[points.Length];
                for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
                {
                    Point point = points[pointIndex];
                    Point scaledPoint = new Point(point.X * (width / 200), point.Y * (height / 200));
                    scaledPoints[pointIndex] = scaledPoint;
                }
                graphics.FillPolygon(new SolidBrush(color), scaledPoints);
            }

            // Save
            bitmap.Save(filename);
        }

        static void Main(string[] args)
        {
            initColorMap();
            Dictionary<String, Model> models = ConfigLoader.readModels();
            using (StreamReader sr = new StreamReader("rubik.dat"))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string colorCode = line;
                    string[] metaData = sr.ReadLine().Split(',');
                    if (metaData.Length != 2)
                    {
                        throw new Exception("Invalid data");
                    }
                    string filename = metaData[0].Trim();
                    string modelName = metaData[1].Trim();
                    Console.WriteLine(modelName);
                    Console.WriteLine(modelName.Equals("model_1"));
                    Model model = models[modelName];
                    if (model == null)
                    {
                        throw new Exception("Invalid model name");
                    }
                    drawAndSave(filename, model, colorCode);
                    line = sr.ReadLine();
                }
            }
            
        }
    }
}
