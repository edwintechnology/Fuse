using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Fuse
{
    public class Utility
    {
        public Utility()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string ToString(object o)
        {
            if (o == DBNull.Value)
                return null;
            else
            return Convert.ToString(o);
        }
        public static short ToShort(object o)
        {
            if (o == DBNull.Value)
                return -1;
            else
            return Convert.ToInt16(o);
        }
        public static int ToInt(object o)
        {
            if (o == DBNull.Value)
                return -1;
            else
            return Convert.ToInt32(o);
        }
        public static long ToLong(object o)
        {
            if (o == DBNull.Value)
                return -1;
            else
            return Convert.ToInt64(o);
        }
        public static string ConnectionString
        {
            get
            {
                return "Server=localhost;Uid=web;Pwd=ArhDew0321;Persist Security Info=True;database=Fusionner";
            }
        }
        public static DateTime ToDateTime(object o)
        {
            if (o == DBNull.Value)
                return DateTime.MinValue;
            else
            return Convert.ToDateTime(o);
        }
        public static double ToDouble(object o)
        {
            if (o == DBNull.Value)
                return -1;
            else
            return Convert.ToDouble(o);
        }
        public static decimal ToDecimal(object o)
        {
            if (o == DBNull.Value)
                return -1;
            else
            return Convert.ToDecimal(o);
        }
        public static bool ToBool(object o)
        {
            if (o == DBNull.Value)
                return false;
            else
            return Convert.ToBoolean(o);
        }
        public static byte ToByte(object o)
        {
            if (o == DBNull.Value)
                return 0;
            else
            return Convert.ToByte(o);
        }
    }
}
