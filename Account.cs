using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fuse
{
    public class Account
    {
        public long _id;
        public long _userid;
        public string _name;
        public double _balance;
        public long _account_type_id;
        public long _currency_id;
        public long _eid;
        public DateTime _edate;
        public long _mid;
        public DateTime _mdate;
        private static MySqlConnection conn;

        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public long UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public double Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }
        public long AccountTypeID
        {
            get { return _account_type_id; }
            set { _account_type_id = value; }
        }
        public long CurrencyID
        {
            get { return _currency_id; }
            set { _currency_id = value; }
        }
        public long EID
        {
            get { return _eid; }
            set { _eid = value; }
        }
        public DateTime EDate
        {
            get { return _edate; }
            set { _edate = value; }
        }
        public long MID
        {
            get { return _mid; }
            set { _mid = value; }
        }
        public DateTime MDate
        {
            get { return _mdate; }
            set { _mdate = value; }
        }

        public Account()
        {
        }
        public static List<Account> ByUserID(long userid)
        {
            List<Account> newList = new List<Account>();

            
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from account where user_id = ?id", conn);
            com.Parameters.AddWithValue("?id", userid);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                Account item = new Account();
                item.ID = Utility.ToLong(dr["id"]);
                item.UserID = Utility.ToLong(dr["user_id"]);
                item.Name = Utility.ToString(dr["name"]);
                item.Balance = Utility.ToDouble(dr["balance"]);
                item.AccountTypeID = Utility.ToLong(dr["account_type_id"]);
                item.CurrencyID = Utility.ToLong(dr["currency_id"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.MID = Utility.ToLong(dr["mid"]);
                item.MDate = Utility.ToDateTime(dr["mdate"]);

                newList.Add(item);
            }
            conn.Close();

            return newList;
        }
        public static Account ByID(long id)
        {
            Account item = new Account();
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from account where id = ?id", conn);
            com.Parameters.AddWithValue("?id", id);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                item.ID = Utility.ToLong(dr["id"]);
                item.UserID = Utility.ToLong(dr["user_id"]);
                item.Name = Utility.ToString(dr["name"]);
                item.Balance = Utility.ToDouble(dr["balance"]);
                item.AccountTypeID = Utility.ToLong(dr["account_type_id"]);
                item.CurrencyID = Utility.ToLong(dr["currency_id"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.MID = Utility.ToLong(dr["mid"]);
                item.MDate = Utility.ToDateTime(dr["mdate"]);
            }
            conn.Close();
            if (item.ID > 0)
                return item;
            return null;
        }
        private static void makeConnection()
        {
            conn = new MySqlConnection(ConnectionString);
        }
        public static string ConnectionString
        {
            get { return Utility.ConnectionString; }
        }
    }
}
