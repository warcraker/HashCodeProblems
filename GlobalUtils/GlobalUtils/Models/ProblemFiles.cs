using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalUtils.Models
{
    public class ProblemFiles
    {
        public IEnumerable<InputFile> InputFiles { get; }
        public string TemporalPath { get; }
        public string OutputPath { get; }

        public ProblemFiles(string rootPath)
        {
            string inputFolderPath = Path.Combine(rootPath, "Input");
            this.InputFiles = Directory.EnumerateFiles(inputFolderPath, "*.in")
                .Select(path => new InputFile(path))
                .ToArray();

            this.TemporalPath = Path.Combine(rootPath, "Temp");
            this.OutputPath = Path.Combine(rootPath, "Output");
        }
    }
}
