using System;
using ADODB;
using Dlrsoft.Asp;

namespace AspWebApp.App_Code
{
    public class ADODBHooksImpl : ADODBHooks
    {
        public override string ConnectionOpen(string connectionString)
        {
            if (connectionString.ToLowerInvariant().Contains("provider=microsoft.jet.oledb.4.0") && connectionString.ToLowerInvariant().Contains("authors.mdb"))
                return "Data Source=192.168.1.200;Initial Catalog=Authors;User Id=sa;Password=circles-arrows;";

            throw new NotSupportedException($"The connection '{connectionString}' is not supported.");
        }
    }
}
