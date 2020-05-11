using System;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;

namespace Dlrsoft.Asp
{
    public abstract class Hooks<T>
        where T : Hooks<T>, new()
    {
        public static T Instance = Initialize();

        protected static T Initialize()
        {
            T hook = null;

            string handlerDef = WebConfigurationManager.AppSettings[typeof(T).Name];
            if (!string.IsNullOrEmpty(handlerDef))
            {
                string[] handlerDefParts = handlerDef.Split(',').Select(item => item.Trim()).ToArray();
                Assembly executing = null;
                if (handlerDefParts.Length > 1)
                    executing = AppDomain.CurrentDomain.GetAssemblies().First(item => item.GetName().Name == handlerDefParts[1]);
                else
                    executing = Assembly.GetCallingAssembly();

                Type type = executing.GetType(handlerDefParts.First(), false, false);
                if (type != null)
                    hook = (T)Activator.CreateInstance(type);
            }

            return hook ?? new T();
        }
    }

    public class ServerHooks : Hooks<ServerHooks>
    {
        public virtual object CreateObject(string literal) { return null; }
    }
}
