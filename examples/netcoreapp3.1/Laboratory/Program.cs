using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using AspClassicCore;

namespace Laboratory
{
    class Program
    {
        private const string TEST_ASP_RESOURCE_NAME = nameof(Laboratory) + ".test.asp";
        private const string TEST_VBS_RESOURCE_NAME = nameof(Laboratory) + ".test.vbs";

        static void Main(string[] args)
        {
            TestVBScript();
            TestASPPage();

            Console.WriteLine("\r\nType any key to continue..");
            Console.ReadKey(true);
        }

        private static void TestVBScript()
        {
            Dictionary<string, object> state = new Dictionary<string, object>()
            {
                { "Response", Console.Out },
            };

            VBScript script = new VBScript(TEST_VBS_RESOURCE_NAME, TestVBS.Value);
            script.OnError += ErrorHandler;
            script.Run(state, true);
        }
        private static void TestASPPage()
        {
            Dictionary<string, object> state = new Dictionary<string, object>()
            {
                { "Response", new ResponseProxy()}
            };

            ASPScript script = new ASPScript(Environment.CurrentDirectory, Environment.CurrentDirectory + @"\test.asp", TestASP.Value);
            script.OnServerSideInclude += Script_OnServerSideInclude;
            script.OnError += ErrorHandler;
            script.Run(state, true);
        }

        private static void Script_OnServerSideInclude(string filename, IncludeSource include)
        {
            if (filename == "WhaaaAnIclude.vbs")
            {
                include.FullFileName = @"C:\WhaaaAnIclude.vbs";
                include.SourceCode   = @"err.Raise 5";
            }
        }
        static void ErrorHandler(int errorNumber, string errorMessage, string filename, int startLine, int startColumn, int endLine, int endColumn, Stage stage)
        {
            Console.WriteLine($"Error:       {errorNumber} - {errorMessage}");
            Console.WriteLine($"Source File: {filename}");
            Console.WriteLine($"From:        Line {startLine}, Column {startColumn}");
            Console.WriteLine($"Till:        Line {endLine}, Column {endColumn}");
        }

        private static Lazy<string> TestASP = new Lazy<string>(delegate()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(TEST_ASP_RESOURCE_NAME))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }, true);
        private static Lazy<string> TestVBS = new Lazy<string>(delegate ()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(TEST_VBS_RESOURCE_NAME))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }, true);
    }

    public class ResponseProxy
    {
        public void Write(string content)
        {
            Console.Write(content);
        }
        public void WriteLine(string content)
        {
            Console.WriteLine(content);
        }
    }
}
