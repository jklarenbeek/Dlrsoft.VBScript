using System;
using Dlrsoft.Asp;

namespace AspWebApp.App_Code
{
    public class ServerHooksImpl : ServerHooks
    {
        public override object CreateObject(string literal)
        {
            if (literal == "ADODB.Connection")
                return new ADODB.Connection();

            return null;
        }
    }
}
