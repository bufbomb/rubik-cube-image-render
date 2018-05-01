using System;
using System.Collections.Generic;
using System.Drawing;

namespace RubikCubeImageRender
{
    public class RubikDataProcessor
    {
        Dictionary<String, Model> models;

        static Dictionary<Char, Color> colorMap = new Dictionary<char, Color>();

        // Make it configurable
        const int DEFAULT_SIZE = 100;
        static bool inited = false;
        static void initColorMap()
        {
            if (!inited)
            {
                colorMap['R'] = Color.Red;
                colorMap['G'] = Color.FromArgb(0, 0xDD, 0);
                colorMap['B'] = Color.FromArgb(0x00, 0x77, 0xFF);
                colorMap['O'] = Color.FromArgb(0xFF, 0xA5, 0);
                colorMap['Y'] = Color.Yellow;
                colorMap['W'] = Color.White;
                colorMap['X'] = Color.Gray;
                inited = true;
            }
        }

        public RubikDataProcessor(Dictionary<String, Model> models)
        {
            this.models = models;
            initColorMap();
        }

        public void Process(string metaLine, string colorLine)
        {
            string[] metaData = metaLine.Split(',');
            if (metaData.Length < 2)
            {
                throw new Exception("Invalid data");
            }
            string filename = metaData[0].Trim();
            string modelName = metaData[1].Trim();
            int size = DEFAULT_SIZE;
            if (metaData.Length >= 3)
            {
                size = Int32.Parse(metaData[2]);
            }
            Model model = models[modelName];
            if (model == null)
            {
                throw new Exception("Invalid model name");
            }
            drawAndSave(filename, model, colorLine, size);
        }

        static void drawAndSave(string filename, Model model, string colorCode, int size)
        {
            // Create a Bitmap object from a file.
            Bitmap bitmap = new Bitmap(size, size);
            Graphics graphics = Graphics.FromImage(bitmap);
            Pen boardPen = new Pen(Color.Black, size / 100);

            for (int i = 0; i < colorCode.Length; i++)
            {
                Char colorChar = colorCode[i];
                Color color = colorMap[colorChar];
                Point[] points = model.GetPolygen(i);
                PointF[] scaledPoints = GetScaledPoints(points, size);
                graphics.FillPolygon(new SolidBrush(color), scaledPoints);
            }

            for (int i = 0; i < model.GetPolygenCount(); i++)
            {
                Point[] points = model.GetPolygen(i);
                PointF[] scaledPoints = GetScaledPoints(points, size);
                graphics.DrawPolygon(boardPen, scaledPoints);
            }

            // Save
            bitmap.Save(filename);
        }

        static PointF[] GetScaledPoints(Point[] points, int size)
        {
            PointF[] scaledPoints = new PointF[points.Length];
            for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
            {
                Point point = points[pointIndex];
                PointF scaledPoint = new PointF(point.X * (size / 200.0f), point.Y * (size / 200.0f));
                scaledPoints[pointIndex] = scaledPoint;
            }
            return scaledPoints;
        }
    }
}