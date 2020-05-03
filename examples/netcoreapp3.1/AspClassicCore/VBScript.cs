using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

using Dlrsoft.VBScript.Compiler;
using Dlrsoft.VBScript.Hosting;
using Dlrsoft.VBScript.Runtime;
using System.Globalization;

namespace AspClassicCore
{
    public class VBScript
    {
        public VBScript(string filename, string sourcecode)
        {
            SourceCode = new SourceCode(filename, sourcecode);
            SourceMapper = new VBScriptSourceMapper();
            CompiledRelease = null;
            CompiledDebug = null;

            SourceSpan generatedSpan = new SourceSpan(
                new SourceLocation(1, 1, 1),
                new SourceLocation(sourcecode.Length, sourcecode.Split('\r').Length, sourcecode.Split('\r').Last().Length)
            );
            SourceMapper.AddMapping(generatedSpan, new DocSpan(filename, generatedSpan));
        }
        internal VBScript(string filename, string sourcecode, VBScriptSourceMapper sourcemapper)
        {
            SourceCode = new SourceCode(filename, sourcecode);
            SourceMapper = sourcemapper;
            CompiledRelease = null;
            CompiledDebug = null;
        }

        public SourceCode SourceCode { get; private set; }
        internal VBScriptSourceMapper SourceMapper { get; private set; }
        internal CompiledCodeCache CompiledRelease { get; private set; }
        internal CompiledCodeCache CompiledDebug { get; private set; }

        private CompiledCodeCache Compile(bool debug)
        {
            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();
            //setup.DebugMode = true;
            if (debug)
                setup.Options.Add("Trace", true);

            string qualifiedname = typeof(VBScriptContext).AssemblyQualifiedName;
            setup.LanguageSetups.Add(new LanguageSetup(qualifiedname, "vbscript", new[] { "vbscript" }, new[] { ".vbs" }));
            ScriptRuntime dlrRuntime = new ScriptRuntime(setup);
            
            // Don't need to tell the DLR about the assemblies we want to be
            // available, which the SymplLangContext constructor passes to the
            // Sympl constructor, because the DLR loads mscorlib and System by
            // default.
            //dlrRuntime.LoadAssembly(typeof(object).Assembly);
            dlrRuntime.LoadAssembly(typeof(global::Dlrsoft.VBScript.Runtime.BuiltInFunctions).Assembly);

            ScriptEngine engine = dlrRuntime.GetEngine("vbscript");
            TextContentProvider contentProvider = new VBScriptStringContentProvider(SourceCode.Code, SourceMapper);
            ScriptSource scriptSource = engine.CreateScriptSource(contentProvider, SourceCode.Filename, SourceCodeKind.File);
            CompiledCodeCache compiledCode = null;
            try
            {
                compiledCode = new CompiledCodeCache(scriptSource.Compile(), engine);
            }
            catch (VBScriptCompilerException ex)
            {
                compiledCode = new CompiledCodeCache(ex.SyntaxErrors);

                foreach (VBScriptSyntaxError error in ex.SyntaxErrors)
                    OnError?.Invoke(error.ErrorCode, error.ErrorDescription, error.FileName, error.Span.Start.Line, error.Span.Start.Column, error.Span.End.Line, error.Span.End.Column, Stage.Compiletime);
            }
            catch (Exception ex)
            {
                compiledCode = new CompiledCodeCache(ex);

                OnError?.Invoke(-1, ex.Message, null, 0, 0, 0, 0, Stage.Compiletime);
            }

            return compiledCode;
        }
        public bool Run(IDictionary<string, object> state, bool debug)
        {
            CompiledCodeCache compiled = (debug) ? CompiledDebug : CompiledRelease;
            if (compiled is null)
            {
                lock (this)
                {
                    if (compiled is null)
                    {
                        compiled = Compile(debug);

                        if (debug)
                            CompiledDebug = compiled;
                        else
                            CompiledRelease = compiled;
                    }
                }
            }

            if (compiled.CompiledCode is null)
                return false;

            ScriptScope feo = compiled.Engine.CreateScope(); //File Level Expando Object
            TraceHelper trace = null;
            if (debug)
            {
                trace = new TraceHelper();
                feo.SetVariable("__trace", trace);
            }

            foreach (var item in state)
                feo.SetVariable(item.Key, item.Value);

            try
            {
                compiled.CompiledCode.Execute(feo);
            }
            catch (VBScriptRuntimeException ex)
            {
                if (trace is null)
                    OnError?.Invoke(ex.Number, NumberToMessage(ex.Number), SourceCode.Filename, 0, 0, 0, 0, Stage.Runtime);
                else
                    OnError?.Invoke(ex.Number, NumberToMessage(ex.Number), trace.Source, trace.StartLine, trace.StartColumn, trace.EndLine, trace.EndColumn, Stage.Runtime);
            }
            catch (Exception ex)
            {
                if (trace is null)
                    OnError?.Invoke(-1, ex.Message, SourceCode.Filename, 0, 0, 0, 0, Stage.Runtime);
                else
                    OnError?.Invoke(-1, ex.Message, trace.Source, trace.StartLine, trace.StartColumn, trace.EndLine, trace.EndColumn, Stage.Runtime);
            }

            return true;
        }

        public event ErrorHandler OnError;
        private static string NumberToMessage(int errorNr)
        {
            switch (errorNr)
            {
                case 5:
                    return "Invalid procedure call or argument";
                case 6:
                    return "Overflow";
                case 7:
                    return "Out of memory";
                case 9:
                    return "Subscript out of range";
                case 10:
                    return "The Array is of fixed length or temporarily locked";
                case 11:
                    return "Division by zero";
                case 13:
                    return "Type mismatch";
                case 14:
                    return "Out of string space(overflow)";
                case 17:
                    return "cannot perform the requested operation";
                case 28:
                    return "Stack overflow";
                case 35:
                    return "Undefined SUB procedure or Function";
                case 48:
                    return "Error loading DLL";
                case 51:
                    return "Internal error";
                case 52:
                    return "bad file name or number";
                case 53:
                    return "File not found";
                case 54:
                    return "Bad file mode";
                case 55:
                    return "File is already open";
                case 57:
                    return "Device I/ O error";
                case 58:
                    return "File already exists";
                case 61:
                    return "Disk space is full";
                case 62:
                    return "input beyond the end of the file";
                case 67:
                    return "Too many files";
                case 68:
                    return "Device not available";
                case 70:
                    return "Permission denied";
                case 71:
                    return "Disk not ready";
                case 74:
                    return "Cannot rename with different drive";
                case 75:
                    return "Path / file access error";
                case 76:
                    return "Path not found";
                case 91:
                    return "Object variable not set";
                case 92:
                    return "For loop is not initialized";
                case 94:
                    return "Invalid use of Null";
                case 322:
                    return "Could not create the required temporary file";
                case 424:
                    return "Could not find target object";
                case 429:
                    return "ActiveX cannot create object";
                case 430:
                    return "Class does not support Automation";
                case 432:
                    return "File name or class name not found during Automation operation";
                case 438:
                    return "Object doesn't support this property or method";
                case 440:
                    return "Automation error";
                case 445:
                    return "Object does not support this action";
                case 446:
                    return "Object does not support the named arguments";
                case 447:
                    return "Object does not support the current locale";
                case 448:
                    return "Named argument not found";
                case 449:
                    return "parameters are not optional";
                case 450:
                    return "Wrong number of parameters or invalid property assignment";
                case 451:
                    return "is not a collection of objects";
                case 453:
                    return "The specified DLL function was not found";
                case 455:
                    return "Code resource lock error";
                case 457:
                    return "This key already associated with an element of this collection";
                case 458:
                    return "Variable uses an Automation type not supported in VBScript";
                case 462:
                    return "The remote server does not exist or is not available";
                case 481:
                    return "Image is invalid.";
                case 500:
                    return "variable not defined";
                case 501:
                    return "illegal assignment";
                case 502:
                    return "The object is not safe for scripting";
                case 503:
                    return "Object not safe for initializing";
                case 504:
                    return "Object can not create a secure environment";
                case 505:
                    return "invalid or unqualified reference";
                case 506:
                    return "Class/Type is not defined";
                case 507:
                    return "Unexpected error";
                case 1001:
                    return "Insufficient memory";
                case 1002:
                    return "syntax error";
                case 1003:
                    return "Missing ':'";
                case 1005:
                    return "Missing '('";
                case 1006:
                    return "Missing ')'";
                case 1007:
                    return "Missing ']'";
                case 1010:
                    return "Missing Identifier";
                case 1011:
                    return "Missing = ";
                case 1012:
                    return "Missing 'If'";
                case 1013:
                    return "Missing 'To'";
                case 1014:
                    return "Missing 'End'";
                case 1015:
                    return "Missing 'Function'";
                case 1016:
                    return "Missing 'Sub'";
                case 1017:
                    return "Missing 'Then'";
                case 1018:
                    return "Missing 'Wend'";
                case 1019:
                    return "Missing 'Loop'";
                case 1020:
                    return "Missing 'Next'";
                case 1021:
                    return "Missing 'Case'";
                case 1022:
                    return "Missing 'Select'";
                case 1023:
                    return "Missing expression";
                case 1024:
                    return "Missing statement";
                case 1025:
                    return "Missing end of statement";
                case 1026:
                    return "Requires an integer constant";
                case 1027:
                    return "Missing 'While' or 'Until'";
                case 1028:
                    return "Missing 'While, 'Until,' or End of statement";
                case 1029:
                    return "Too many locals or arguments";
                case 1030:
                    return "Identifier Too long";
                case 1031:
                    return "The number of is invalid";
                case 1032:
                    return "Invalid character";
                case 1033:
                    return "Unterminated string constant";
                case 1034:
                    return "Unterminated comment";
                case 1035:
                    return "Nested comment";
                case 1037:
                    return "Invalid use of 'Me' Keyword";
                case 1038:
                    return "'loop' statement is missing 'do'";
                case 1039:
                    return "invalid 'exit' statement";
                case 1040:
                    return "invalid 'for' loop control variable";
                case 1041:
                    return "Name redefined";
                case 1042:
                    return "Must be the first statement line";
                case 1043:
                    return "Cannot be assigned to non - Byval argument";
                case 1044:
                    return "Cannot use parentheses when calling Sub";
                case 1045:
                    return "Requires a literal constant";
                case 1046:
                    return "Missing 'In'";
                case 1047:
                    return "Missing 'Class'";
                case 1048:
                    return "Must be inside a class definition";
                case 1049:
                    return "Missing Let, Set or Get in the property declaration";
                case 1050:
                    return "Missing 'Property'";
                case 1051:
                    return "The Number of parameters must be consistent with the attribute description";
                case 1052:
                    return "cannot have more than one default attribute / method in a class";
                case 1053:
                    return "Class did not initialize or terminate the process parameters";
                case 1054:
                    return "Property Let or Set should have at least one parameter";
                case 1055:
                    return "Error at 'Next'";
                case 1056:
                    return "'Default 'can only be in the' Property ',' Function 'or' Sub 'in the designated";
                case 1057:
                    return "'Default' must be 'Public'";
                case 1058:
                    return "Can only specify the Property Get in the 'Default'";
                case 5016:
                    return "Requires a regular expression object";
                case 5017:
                    return "Regular expression syntax error";
                case 5018:
                    return "The number of words error";
                case 5019:
                    return "Regular expressions is missing ']'";
                case 5020:
                    return "Regular expressions is missing ')'";
                case 5021:
                    return "Character set Cross-border";
                case 32766:
                    return "True";
                case 32767:
                    return "False";
                case 32811:
                    return "Element was not found";
                case 32812:
                    return "The specified date is not available in the current locale's calendar";
                default:
                    return "Unknown error";
            }
        }
    }
}
