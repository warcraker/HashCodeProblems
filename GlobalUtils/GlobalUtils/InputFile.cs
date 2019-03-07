using HashCode.Common;
using System.IO;

namespace GlobalUtils
{
    public class InputFile
    {
        public string FileName { get; }
        public string FullPath { get; }
        public string[] FileLines
        {
            get
            {
                if (this.fileLines == null)
                {
                    this.fileLines = Utils.GetFileLines(this.FullPath);
                }

                return this.fileLines;
            }
        }

        private string[] fileLines = null;

        public InputFile (string fullPath)
        {
            this.FullPath = fullPath;

            FileInfo info = new FileInfo(fullPath);
            this.FileName = info.Name;
        }
    }
}
