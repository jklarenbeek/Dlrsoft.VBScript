using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ADODB
{
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

            EOF = !Reader.Read();
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

        public bool EOF { get; private set; }
        public void MoveNext()
        {
            EOF = !Reader.Read();
        }

        public void Close()
        {
            ADODBHooks.Instance.RecordSetClose();
            Reader.Close();
        }
    }
}
