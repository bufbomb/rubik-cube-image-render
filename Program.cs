using CommandLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RubikCubeImageRender
{
    class Program
    {
        static Dictionary<Char, Color> colorMap = new Dictionary<char, Color>();
        
        // Make it configurable
        static int width = 100;
        static int height = 100;

        static void initColorMap()
        {
            colorMap['R'] = Color.Red;
            colorMap['G'] = Color.FromArgb(0, 0xDD, 0);
            colorMap['B'] = Color.FromArgb(0x00, 0x77, 0xFF);
            colorMap['O'] = Color.FromArgb(0xFF, 0xA5, 0);
            colorMap['Y'] = Color.Yellow;
            colorMap['W'] = Color.White;
            colorMap['X'] = Color.Gray;
        }

        static void drawAndSave(string filename, Model model, string colorCode)
        {
            // Create a Bitmap object from a file.
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            Pen boardPen = new Pen(Color.Black, 1);

            for (int i = 0; i < colorCode.Length; i++)
            {
                Char colorChar = colorCode[i];
                Color color = colorMap[colorChar];
                Point[] points = model.GetPolygen(i);
                PointF[] scaledPoints = GetScaledPoints(points);
                graphics.FillPolygon(new SolidBrush(color), scaledPoints);
            }

            for (int i = 0; i < model.GetPolygenCount(); i++)
            {
                Point[] points = model.GetPolygen(i);
                PointF[] scaledPoints = GetScaledPoints(points);
                graphics.DrawPolygon(boardPen, scaledPoints);
            }

            // Save
            bitmap.Save(filename);
        }

        static PointF[] GetScaledPoints(Point[] points)
        {
            PointF[] scaledPoints = new PointF[points.Length];
            for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
            {
                Point point = points[pointIndex];
                PointF scaledPoint = new PointF(point.X * (width / 200.0f), point.Y * (height / 200.0f));
                scaledPoints[pointIndex] = scaledPoint;
            }
            return scaledPoints;
        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts));
        }   

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            initColorMap();
            Dictionary<String, Model> models = ConfigLoader.readModels(opts.ModelFile);
            using (StreamReader sr = new StreamReader(opts.DataFile))
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
