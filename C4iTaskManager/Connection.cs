using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C4iTaskManager
{
    public class Connection
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SAITSQLEXPRESS;Initial Catalog=TaskManager;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectionString);
            return con;
        }
    }
}
