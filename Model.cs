using System;
using System.Collections.Generic;
using System.Drawing;

namespace RubikCubeImageRender
{
    public class Model
    {

        public enum EdgeType
        {
            NO_EDGE,
            THIN_EDGE,
            NORMAL_EDGE,
            THICK_EDGE
        }

        public class Polygen
        {
            readonly Point[] points;
            readonly EdgeType edgeType;

            public Polygen(Point[] points) : this(points, EdgeType.NORMAL_EDGE)
            {
                ;
            }

            public Polygen(Point[] points, EdgeType type)
            {
                this.points = points;
                this.edgeType = type;
            }

            public Point[] Points
            {
                get
                {
                    return points;
                }
            }

            public int GetStrikeSize(int defaultSize)
            {
                switch (edgeType)
                {
                    case EdgeType.NO_EDGE:
                        return 0;
                    case EdgeType.THIN_EDGE:
                        return 1;
                    case EdgeType.THICK_EDGE:
                        return defaultSize + (defaultSize >> 1);
                    default:
                        return defaultSize;
                }
            }
        }



        string name;
        List<Polygen> polygens = new List<Polygen>();
        Dictionary<String, String> arrows = new Dictionary<string, string>();

        public Model(string name)
        {
            this.name = name;
        }

        public string Name { get => name;}

        public void AddPolygen(Point[] points)
        {
            polygens.Add(new Polygen(points));
        }

        public void AddPolygen(Point[] points, EdgeType type)
        {
            polygens.Add(new Polygen(points, type));
        }

        public Polygen GetPolygen(int index)
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
