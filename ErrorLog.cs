using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fuse
{
    public class ErrorLog
    {
        public static void Log(long userid, string exception, string page)
        {
            using (MySqlConnection conn = new MySqlConnection(Utility.ConnectionString))
            {
                conn.Open();
                try
                {
                    MySqlCommand com = new MySqlCommand("INSERT into errorlog(exception, page, eid, edate) VALUES (?ex, ?page, ?uid, NOW())", conn);
                    com.Prepare();
                    com.Parameters.AddWithValue("?ex", exception);
                    com.Parameters.AddWithValue("?page", page);
                    com.Parameters.AddWithValue("?uid", userid);
                    com.ExecuteNonQuery();

                }
                catch { }
                finally
                {
                    conn.Close();
                }
            }
        }

    }
}
