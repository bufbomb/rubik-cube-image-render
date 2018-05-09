using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubikCubeImageRender
{
    public class RubikDataReader : StreamReader
    {
        public RubikDataReader(Stream stream) : base(stream)
        {
        }

        public RubikDataReader(string path) : base(path)
        {
        }

        public RubikDataReader(Stream stream, bool detectEncodingFromByteOrderMarks) : base(stream, detectEncodingFromByteOrderMarks)
        {
        }

        public RubikDataReader(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        public RubikDataReader(string path, bool detectEncodingFromByteOrderMarks) : base(path, detectEncodingFromByteOrderMarks)
        {
        }

        public RubikDataReader(string path, Encoding encoding) : base(path, encoding)
        {
        }

        public RubikDataReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks) : base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
        }

        public RubikDataReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks) : base(path, encoding, detectEncodingFromByteOrderMarks)
        {
        }

        public RubikDataReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize) : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
        }

        public RubikDataReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize) : base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
        }

        public RubikDataReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen) : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
        {
        }

        public override string ReadLine()
        {
            string line = base.ReadLine();
            while (line != null)
            {
                if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                {
                    line = base.ReadLine();
                } else
                {
                    return line;
                }
            }
            return null;
        }
    }
}
