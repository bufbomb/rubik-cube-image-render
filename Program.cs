using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace RubikCubeImageRender
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts));
        }   

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            Dictionary<String, Model> models = ConfigLoader.readModels(opts.ModelFile);
            RubikDataProcessor processor = new RubikDataProcessor(models);
            using (StreamReader sr = new StreamReader(opts.DataFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string colorCode = sr.ReadLine();
                    processor.Process(line, colorCode);
                }
            }
        }
    }
}
