using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AspClassicCore
{
    public static class AspClassicMiddleWare
    {
        static public void UseAspClassic(this IApplicationBuilder app)
        {
            IHostingEnvironment hostingEnvironment = (IHostingEnvironment)app.ApplicationServices.GetService(typeof(IHostingEnvironment));
            Configuration config = new Configuration(hostingEnvironment);

            app.Use(config.Handler);
        }

        private class Configuration
        {
            internal Configuration(IHostingEnvironment hostingEnvironment)
            {
                HostingEnvironment = hostingEnvironment;
            }

            public IHostingEnvironment HostingEnvironment;

            public string MapPath(string virtualPath)
            {
                return Path.GetFullPath(HostingEnvironment.WebRootPath + @"\" + virtualPath.Replace(@"/", @"\"));
            }

            public Task Handler(HttpContext context, Func<Task> task)
            {
                if (context.Request.Path.Value.EndsWith(".asp", StringComparison.InvariantCultureIgnoreCase))
                {
                    string hostingroot = HostingEnvironment.WebRootPath;
                    string scriptpath = MapPath(context.Request.Path);
                    string content = File.ReadAllText(scriptpath);

                    StringBuilder errors = null;

                    ASPScript script = new ASPScript(hostingroot, scriptpath, content);
                    script.OnError += delegate (int errorNumber, string errorMessage, string filename, int startLine, int startColumn, int endLine, int endColumn, Stage stage)
                    {
                        if (errors is null)
                            errors = new StringBuilder();

                        errors.AppendLine($"Error:       {errorNumber} - {errorMessage}");
                        errors.AppendLine($"Source File: {filename}");
                        errors.AppendLine($"From:        Line {startLine}, Column {startColumn}");
                        errors.AppendLine($"Till:        Line {endLine}, Column {endColumn}");
                    };

                    IDictionary<string, object> state = new Dictionary<string, object>()
                    {
                        { "Response", new AspResponse(context.Response) },
                        { "Request", context.Request },
                        { "Session", context.Session },
                        { "Application", HostingEnvironment },
                    };

                    script.Run(state, true);

                    if (errors is null)
                        return Task.CompletedTask;

                    return context.Response.WriteAsync(errors.ToString());
                }
                else
                {
                    return task.Invoke();
                }
            }
        }
    }
}
