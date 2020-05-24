using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Hosting;
#if USE35
using Microsoft.Scripting.Generation;
using Microsoft.Scripting.Ast;
#else
using System.Linq.Expressions;
#endif
using System.Dynamic;
using Dlrsoft.VBScript.Hosting;
using Dlrsoft.VBScript.Compiler;
using Dlrsoft.Asp.BuiltInObjects;
using System.Configuration;
using System.Web.Configuration;
using System.Linq;
using Dlrsoft.VBScript.Runtime;

namespace Dlrsoft.Asp
{
    public class AspHost
    {
        private ScriptRuntime _runtime;
        private ScriptEngine _engine;
        //private bool _debug;
        private AspHostConfiguration _config;

        public AspHost(AspHostConfiguration config)
        {
            if (RuntimeHelpers.CreateObjectCallback == null)
            {
                lock (typeof(AspHost))
                {
                    if (RuntimeHelpers.CreateObjectCallback == null) // prevent race condition
                    {
                        RuntimeHelpers.CreateObjectCallback = delegate (string literal)
                        {
                            // raise event handler, if handler returns object, return object here
                            object instance = ServerHooks.Instance.CreateObject(literal);
                            if (instance != null)
                                return instance;

                            //return _context.Server.CreateObject(literal);
                            Type t = Type.GetTypeFromProgID(literal);
                            if (t == null)
                            {
                                throw new Exception(string.Format("Cannot create server object ''{0}", literal));
                            }

                            return Activator.CreateInstance(t);
                        };
                    }
                }
            }

            _config = config;

            //Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            //SystemWebSectionGroup systemWeb = configuration.GetSectionGroup("system.web") as SystemWebSectionGroup;

            //if (systemWeb != null)
            //{
            //    _debug = systemWeb.Compilation.Debug;
            //}

            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();
            //if (_debug)
            //{
            //    setup.Options["FullFrames"] = ScriptingRuntimeHelpers.True;
            //    setup.Options["Debug"] = ScriptingRuntimeHelpers.True;
            //}

            if (config.Trace)
            {
                setup.Options["Trace"] = ScriptingRuntimeHelpers.True;
            }

            string qualifiedname = typeof(VBScriptContext).AssemblyQualifiedName;
            setup.LanguageSetups.Add(new LanguageSetup(
                qualifiedname, "vbscript", new[] { "vbscript" }, new[] { ".vbs" }));
            _runtime = new ScriptRuntime(setup);
            //_runtime.LoadAssembly(typeof(global::Dlrsoft.VBScript.Runtime.BuiltInFunctions).Assembly);

            List<Assembly> inject = ServerHooks.Instance.InjectClasses().Select(item => item.Type.Assembly).Distinct().ToList();
            foreach (Assembly assembly in inject)
                _runtime.LoadAssembly(assembly);

            if (config != null && config.Assemblies != null)
            {
                HashSet<Assembly> injected = new HashSet<Assembly>(inject);

                foreach (Assembly a in config.Assemblies)
                {
                    if (!injected.Contains(a))
                    {
                        _runtime.LoadAssembly(a);
                        injected.Add(a);
                    }
                }
            }
            _engine = _runtime.GetEngine("vbscript");
        }

        public CompiledPage ProcessPageFromFile(string pagePath)
        {
            AspPageDom page = new AspPageDom();
            page.processPage(pagePath);
            return CompilePage(page);
        }

        public CompiledPage ProcessPageFromString(string includePath, string pageString)
        {
            AspPageDom page = new AspPageDom();
            page.processPage(includePath, pageString);
            return CompilePage(page);
        }

        public ScriptScope CreateScope()
        {
            ScriptScope scope = _engine.CreateScope();

            if (_config.Trace)
            {
                scope.SetVariable(VBScript.VBScript.TRACE_PARAMETER, new Dlrsoft.VBScript.Runtime.TraceHelper());
            }

            return scope;
        }

        public AspHostConfiguration Configuration
        {
            get { return _config; }
        }

        private CompiledPage CompilePage(AspPageDom page)
        {
            ScriptSource src = _engine.CreateScriptSource(new VBScriptStringContentProvider(page.Code, page.Mapper), page.PagePath, SourceCodeKind.File);
            CompiledCode compiledCode = src.Compile();
            return new CompiledPage(compiledCode, page.Literals);
        }
    }
}
