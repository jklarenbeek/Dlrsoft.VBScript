using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ADODB
{
    public class Connection
    {
        public Connection()
        {
            Provider = new SqlConnection();
        }
        private SqlConnection Provider;

        public void Open(string connectionString)
        {
            string cs = ADODBHooks.Instance.ConnectionOpen(connectionString);

            Provider.ConnectionString = cs;
            Provider.Open();
        }

        public RecordSet Execute(string sql)
        {
            string query = ADODBHooks.Instance.ExecuteQuery(sql);

            SqlCommand command = Provider.CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            SqlDataReader reader = command.ExecuteReader();

            ADODBHooks.Instance.RecordSetOpen();

            return new RecordSet(reader);
        }

        public void Close()
        {
            ADODBHooks.Instance.ConnectionClose();

            Provider.Close();
        }
    }
}
