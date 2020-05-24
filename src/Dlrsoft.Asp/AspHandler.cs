//////////////////////////////////////////////////////////////////////////////
// Created by: Li Chen
// http://www.dotneteer.com/weblog
// dotneteer@gmail.com
// This is an experimental code. Please do not use in production environment.
// April 22, 2009 V0.1
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Hosting;
#if USE35
using Microsoft.Scripting.Generation;
using Microsoft.Scripting.Ast;
#else
using System.Linq.Expressions;
#endif
using Dlrsoft.VBScript.Hosting;
using Dlrsoft.VBScript.Compiler;
using Dlrsoft.VBScript.Runtime;
using Dlrsoft.Asp.BuiltInObjects;

namespace Dlrsoft.Asp
{
    public class AspHandler : IHttpHandler, IRequiresSessionState
    {
        private static Dictionary<string, Lazy<CompiledPage>> _scriptCache = new Dictionary<string, Lazy<CompiledPage>>();
        private AspHost _aspHost = null;

        public AspHandler()
        {
            AspHostConfiguration config = new AspHostConfiguration();
            config.Assemblies = AspHandlerConfiguration.Assemblies;
            config.Trace = AspHandlerConfiguration.Trace;

            _aspHost = new AspHost(config);
        }

        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string pagePath = context.Request.ServerVariables["PATH_TRANSLATED"];

            Lazy<CompiledPage> cpage = null;
            if (_scriptCache.ContainsKey(pagePath))
            {
                cpage = _scriptCache[pagePath];
                //don't use it if updated
                if (cpage.Value.CompileTime < File.GetLastWriteTime(pagePath))
                    cpage = null;
            }
            
            if (cpage == null)
            {
                try
                {
                    cpage = new Lazy<CompiledPage>(() => _aspHost.ProcessPageFromFile(pagePath), true);
                    _scriptCache[pagePath] = cpage;
                }
                catch (VBScriptCompilerException ex)
                {
                    AspHelper.RenderError(context.Response, ex);
                    return;
                }
            }

            ScriptScope pageScope = _aspHost.CreateScope();
            pageScope.SetVariable("response", new AspResponse(context));
            pageScope.SetVariable("request", new AspRequest(context));
            pageScope.SetVariable("session", new AspSession(context));
            pageScope.SetVariable("server", new AspServer(context));
            pageScope.SetVariable("application", new AspApplication(context));
            AspObjectContext ctx = new AspObjectContext();
            pageScope.SetVariable("objectcontext", ctx);
            pageScope.SetVariable("getobjectcontext", ctx);
            //responseScope.SetVariable("err", new Microsoft.VisualBasic.ErrObject());
            //Used to get the literals
            pageScope.SetVariable("literals", cpage.Value.Literals);

            try
            {
                object o = cpage.Value.Code.Execute(pageScope);
            }
            catch (ThreadAbortException)
            {
                //Do nothing since we get this normally when calling response.redirect
            }
            catch (Exception ex)
            {
                if (_aspHost.Configuration.Trace)
                {
                    TraceHelper th = (TraceHelper)pageScope.GetVariable(VBScript.VBScript.TRACE_PARAMETER);
                    string source = string.Format("{0} ({1},{2})-({3},{4})", th.Source, th.StartLine, th.StartColumn, th.EndLine, th.EndColumn);

                    //string errorFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), @"ASPClassicError.txt");
                    //string errorText = string.Format("Message: {0}\r\nSource: {1}\r\nStackTrace:\r\n{2}", ex.Message, source, ex.StackTrace);
                    //File.WriteAllText(errorFilename, errorText, Encoding.ASCII);

                    throw new VBScriptRuntimeException(ex, source);
                }
                throw;
            }
        }

        #endregion
    }
}
