// Define a class to receive parsed values
using CommandLine;

namespace RubikCubeImageRender
{
    class Options
    {
        [Option("model", Default = "models.cfg", HelpText = "Input model file")]
        public string ModelFile { get; set; }

        [Option("data", Default = "rubik.dat", HelpText = "Input data file")]
        public string DataFile { get; set; }

        [Option("output", Default = null, HelpText = "Image output folder")]
        public string OutputFolder { get; set; }
    }
}