using System;
using System.Data.SqlClient;
using Dlrsoft.Asp;

namespace ADODB
{
    public class ADODBHooks : Hooks<ADODBHooks>
    {
        public virtual string ConnectionOpen(string connectionString) { return connectionString; }
        public virtual string ExecuteQuery(string query) { return query; }
        public virtual void RecordSetOpen() { }
        public virtual void RecordSetClose() { }
        public virtual void ConnectionClose() { }
    }
}
