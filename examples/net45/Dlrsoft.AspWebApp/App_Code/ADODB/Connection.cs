using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AspWebApp.App_Code.ADODB
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
            Provider.ConnectionString = connectionString;
            Provider.Open();
        }

        public RecordSet Execute(string sql)
        {
            SqlCommand command = Provider.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            SqlDataReader reader = command.ExecuteReader();
            return new RecordSet(reader);
        }

        public void Close()
        {
            Provider.Close();
        }
    }

    public class RecordSet
    {
        internal RecordSet(SqlDataReader reader)
        {
            Reader = reader;

            var columns = new List<string>(reader.FieldCount);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columns.Add(reader.GetName(i));
            }
            Fields = columns;

            int index = 0;
            Columns = Fields.ToDictionary(item => item, item => index++);
        }
        private SqlDataReader Reader;

        public IReadOnlyList<string> Fields { get; }
        private Dictionary<string, int> Columns { get; }

        public object this[int index]
        {
            get
            {
                return Reader.GetValue(index);
            }
        }
        public object this[string fieldName]
        {
            get
            {
                return Reader.GetValue(Columns[fieldName]);
            }
        }

        public bool EOF => !Reader.Read();
        public void MoveNext()
        {
        }

        public void Close()
        {
            Reader.Close();
        }
    }
}
