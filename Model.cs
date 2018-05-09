﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace RubikCubeImageRender
{
    public class Model
    {
        string name;
        List<Point[]> polygens = new List<Point[]>();
        Dictionary<String, String> arrows = new Dictionary<string, string>();

        public Model(string name)
        {
            this.name = name;
        }

        public string Name { get => name;}

        public void AddPolygen(Point[] polygen)
        {
            polygens.Add(polygen);
        }

        public Point[] GetPolygen(int index)
        {
            if (index < 0 || index >= polygens.Count)
            {
                return null;
            } else
            {
                return polygens[index];
            }
        }

        public int GetPolygenCount()
        {
            return polygens.Count;
        }

        public void AddArrow(string rep, string arrow)
        {
            arrows.Add(rep, arrow);
        }

        public string GetArrow(string rep)
        {
            string result = null;
            arrows.TryGetValue(rep, out result);
            return result;
        }

        public void PrintDebug()
        {
            Console.WriteLine(name + ":" + polygens.Count);
        }
    }
}
