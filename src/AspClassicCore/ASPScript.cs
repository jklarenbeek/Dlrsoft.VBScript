using Dlrsoft.VBScript.Compiler;
using Microsoft.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AspClassicCore
{
    public class ASPScript
    {
        public ASPScript(string websiteroot, string filename, string sourcecode)
        {
            SourceCode code = new SourceCode(filename, sourcecode);
            EntryPoint = code;
            this.sourcecode.Add(code);
            ProcessedScript = null;
        }

        public string WebSiteRoot { get; private set; }
        public SourceCode EntryPoint { get; private set; }
        public IReadOnlyList<SourceCode> SourceCode => sourcecode;
        private List<SourceCode> sourcecode = new List<SourceCode>();

        internal VBScript ProcessedScript { get; private set; }
        internal AspPageDom AspPage { get; private set; }

        private VBScript Process()
        {
            VBScriptSourceMapper mapper = new VBScriptSourceMapper();
            string code = EntryPoint.Code;

            // ASP Magic
            AspPage = new AspPageDom();
            AspPage.processPage(EntryPoint.Filename, EntryPoint.Code, WebSiteRoot);

            // Compile
            VBScript script = new VBScript(EntryPoint.Filename, AspPage.Code, AspPage.Mapper);
            script.OnError += delegate (int errorNumber, string errorMessage, string filename, int startLine, int startColumn, int endLine, int endColumn, Stage stage)
            {
                OnError?.Invoke(errorNumber, errorMessage, filename, startLine, startColumn, endLine, endColumn, stage);
            };

            return script;
        }

        public void Run(IDictionary<string, object> state, bool debug)
        {
            if (ProcessedScript is null)
            {
                lock (this)
                {
                    if (ProcessedScript is null)
                    {
                        ProcessedScript = Process();
                    }
                }
            }

            state.Add("literals", AspPage.Literals);

            ProcessedScript.Run(state, debug);
        }

        public event ServerSideIncludeHandler OnServerSideInclude;
        public event ErrorHandler OnError;
    }
}
