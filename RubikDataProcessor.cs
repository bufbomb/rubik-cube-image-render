using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RubikCubeImageRender
{
    public class RubikDataProcessor
    {
        enum ArrowFlag {
            DEFAULT = 0,
            EXTEND_START = 1,
            SHRINK_START = 2,
        }

        Dictionary<String, Model> models;
        string outputFolder;

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

        public RubikDataProcessor(Dictionary<String, Model> models, string outputFolder)
        {
            this.models = models;
            this.outputFolder = outputFolder;
            initColorMap();
        }

        public void Process(string metaLine, string dataLine)
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
            string[] data = dataLine.Split('|');
            string colorString = data[0].Trim();
            string optString = data.Length == 2 ? data[1].Trim() : null;

            if (outputFolder != null)
            {
                filename = outputFolder + "/" + filename;
                Directory.CreateDirectory(outputFolder);
            }
            drawAndSave(filename, model, colorString, size, optString);
        }

        static void drawAndSave(string filename, Model model, string colorCode, int size, string optString)
        {
            // Create a Bitmap object from a file.
            Bitmap bitmap = new Bitmap(size, size);
            Graphics graphics = Graphics.FromImage(bitmap);
            int defaultStrike = size / 100;
            for (int i = 0; i < colorCode.Length; i++)
            {
                Char colorChar = colorCode[i];
                if (colorChar == '_')
                {
                    continue;
                }
                Color color = colorMap[colorChar];
                Model.Polygen polygen = model.GetPolygen(i);
                Point[] points = polygen.Points;
                PointF[] scaledPoints = GetScaledPoints(points, size);
                graphics.FillPolygon(new SolidBrush(color), scaledPoints);
                int strike = polygen.GetStrikeSize(defaultStrike);
                if (strike > 0)
                {
                    graphics.DrawPolygon(new Pen(Color.Black, strike), scaledPoints);
                }
            }

            if (optString != null)
            {
                Pen arrowPen = new Pen(Color.Black, defaultStrike);

                string modelArrow = model.GetArrow(optString);
                if (modelArrow != null)
                {
                    optString = modelArrow;
                }
                string[] optStringArray = optString.Split(',');
                foreach (string optS in optStringArray)
                {
                    String arrow = null;
                    ArrowFlag flag = ArrowFlag.DEFAULT;
                    if (optS.Contains("--->"))
                    {
                        arrow = "--->";
                        flag = ArrowFlag.EXTEND_START;
                    } else if (optS.Contains("-->"))
                    {
                        arrow = "-->";
                        flag = ArrowFlag.DEFAULT;
                    } else if (optS.Contains("->"))
                    {
                        arrow = "->";
                        flag = ArrowFlag.SHRINK_START;
                    } else
                    {
                        throw new Exception("failed to parse data file.");
                    }

                    string[] numStr = optS.Split(new string[] { arrow }, StringSplitOptions.RemoveEmptyEntries);
                    int first = int.Parse(numStr[0]);
                    int second = int.Parse(numStr[1]);
                    Model.Polygen firstPolygen = model.GetPolygen(first - 1);
                    Point[] points = firstPolygen.Points;
                    PointF[] scaledPoints = GetScaledPoints(points, size);
                    PointF start = GetCenter(scaledPoints);
                    Model.Polygen secondPolygen = model.GetPolygen(second - 1);
                    points = secondPolygen.Points;
                    scaledPoints = GetScaledPoints(points, size);
                    PointF end = GetCenter(scaledPoints);
                    drawArrow(start, end, graphics, arrowPen, size, flag);
                }
            }

            // Save
            bitmap.Save(filename);
        }

        static void drawArrow(PointF start, PointF end, Graphics g, Pen pen, int size, ArrowFlag flag)
        {
            int arrowSize = size / 100 * 6;
            double radians = 0.0;
            if (start.X == end.X)
            {
                radians = Math.PI / 2 * (end.Y > start.Y ? 1 : -1);
            } else
            {
                radians = Math.Atan2(end.Y - start.Y, end.X - start.X);
            }
            int adjust = (flag == ArrowFlag.DEFAULT ? 0 : (flag == ArrowFlag.EXTEND_START ? 1 : -1)) * arrowSize;
            g.DrawLine(
                pen, 
                (float)(start.X - adjust * Math.Cos(radians)),
                (float)(start.Y - adjust * Math.Sin(radians)),
                end.X,
                end.Y);
            PointF[] arrow = new PointF[] {
                new PointF((float)(end.X - Math.Sin(radians) * arrowSize / 2), (float)(end.Y + Math.Cos(radians) * arrowSize / 2)),
                new PointF((float)(end.X + Math.Sin(radians) * arrowSize / 2), (float)(end.Y - Math.Cos(radians) * arrowSize / 2)),
                new PointF((float)(end.X + Math.Cos(radians) * arrowSize / 2), (float)(end.Y + Math.Sin(radians) * arrowSize / 2))
            };
            g.FillPolygon(new SolidBrush(Color.Black), arrow);
        }

        static PointF GetCenter(PointF[] points)
        {
            PointF p = new PointF();
            float x = 0;
            float y = 0;
            foreach (PointF point in points)
            {
                x += point.X;
                y += point.Y;
            }
            p.X = x / points.Length;
            p.Y = y / points.Length;
            return p;
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