using System;
using System.Collections.Generic;
using System.Text;

namespace AspClassicCore
{
    public delegate void ErrorHandler(int errorNumber, string errorMessage, string filename, int startLine, int startColumn, int endLine, int endColumn, Stage stage);
}
