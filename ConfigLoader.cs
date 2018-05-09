using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RubikCubeImageRender
{
    public class ConfigLoader
    {
        static void constructModel(Model model, List<string> pointsLine, List<string> arrowsLine)
        {
            List<Point> points = new List<Point>();
            foreach (string line in pointsLine)
            {
                String[] pointStrings = line.Split(';');
                for (int i = 0; i < pointStrings.Length; i++)
                {
                    String pointString = pointStrings[i];
                    String[] pos = pointString.Split(',');
                    if (pos.Length != 2)
                    {
                        throw new Exception("Invalid model config file. at " + line);
                    }
                    try
                    {
                        int x = Int32.Parse(pos[0]);
                        int y = Int32.Parse(pos[1]);
                        points.Add(new Point(x, y));
                    }
                    catch
                    {
                        throw new Exception("Invalid model config file. at " + line);
                    }
                }
                model.AddPolygen(points.ToArray());
                points.Clear();
            }
            foreach (string line in arrowsLine)
            {
                string[] arrowStrings = line.Split('=');
                if (arrowStrings.Length != 2)
                {
                    throw new Exception("Invalid arrow line");
                }
                string rep = arrowStrings[0].Trim();
                string data = arrowStrings[1].Trim();
                model.AddArrow(rep, data);
            }
        }

        static Model readModel(StreamReader sr)
        {
            String modelName = null;
            String line = sr.ReadLine();
            if (line == null)
            {
                return null;
            }
            modelName = line.Trim();
            Model model = new Model(modelName);
            List<string> pointsLine = new List<string>();
            List<string> arrowsLine = new List<string>();
            bool readArrow = false;
            while (true)
            {
                line = sr.ReadLine();
                if (line.StartsWith("end"))
                {
                    break;
                } else if (line.Equals("arrow"))
                {
                    readArrow = true;
                } else if (readArrow)
                {
                    arrowsLine.Add(line);
                } else
                {
                    pointsLine.Add(line);
                }
            }
            constructModel(model, pointsLine, arrowsLine);
            return model;
        }

        public static Dictionary<String, Model> readModels(string filename)
        {
            Dictionary<String, Model> models = new Dictionary<string, Model>();
            using (StreamReader sr = new RubikDataReader(filename))
            {
                Model model = readModel(sr);
                while (model != null)
                {
                    models.Add(model.Name, model);
                    model = readModel(sr);
                }
            }
            return models;
        }
    }
}
