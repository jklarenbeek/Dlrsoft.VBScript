using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dlrsoft.VBScript.Compiler;
using Microsoft.Scripting.Hosting;

namespace AspClassicCore
{
    internal class CompiledCodeCache
    {
        internal CompiledCodeCache(CompiledCode compiledCode, ScriptEngine engine)
        {
            CompiledCode = compiledCode;
            Engine = engine;
            Errors = null;
            Exception = null;
        }
        internal CompiledCodeCache(IEnumerable<VBScriptSyntaxError> errors)
        {
            CompiledCode = null;
            Engine = null;
            Errors = errors.ToList();
            Exception = null;
        }
        internal CompiledCodeCache(Exception exception)
        {
            CompiledCode = null;
            Engine = null;
            Errors = null;
            Exception = exception;
        }

        public CompiledCode CompiledCode { get; private set; }
        public ScriptEngine Engine { get; private set; }
        public IReadOnlyList<VBScriptSyntaxError> Errors { get; private set; }
        public Exception Exception { get; private set; }
    }
}
