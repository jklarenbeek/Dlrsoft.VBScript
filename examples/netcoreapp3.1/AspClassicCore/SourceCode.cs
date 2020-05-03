using System;
using System.Collections.Generic;
using System.Text;

namespace AspClassicCore
{
    public class SourceCode
    {
        internal SourceCode(string filename, string code)
        {
            Filename = filename;
            Code = code;
        }

        public string Filename { get; private set; }
        public string Code { get; private set; }
    }
}
