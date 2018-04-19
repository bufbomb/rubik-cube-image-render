using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RubikCubeImageRender
{
    public class ConfigLoader
    {
        static void readPoints(Model model, StreamReader sr)
        {
            List<Point> points = new List<Point>();
            String line = sr.ReadLine();
            while (line != null)
            {
                if (line.Equals("end"))
                {
                    break;
                }
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
                line = sr.ReadLine();
            }
        }

        static Model readModel(StreamReader sr)
        {
            String modelName = null;
            String line = sr.ReadLine();
            while (line != null)
            {
                modelName = line.Trim();
                if (modelName.Equals(""))
                {
                    line = sr.ReadLine();
                }
                else
                {
                    break;
                }
            }
            if (String.IsNullOrEmpty(modelName))
            {
                return null;
            }

            Model model = new Model(modelName);
            readPoints(model, sr);
            return model;
        }

        public static Dictionary<String, Model> readModels()
        {
            Dictionary<String, Model> models = new Dictionary<string, Model>();
            using (StreamReader sr = new StreamReader("models.cfg"))
            {
                Model model = readModel(sr);
                while (model != null)
                {
                    models.Add(model.Name, model);
                    model.PrintDebug();
                    model = readModel(sr);
                }
            }
            return models;
        }
    }
}
