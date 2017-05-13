using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fuse
{
    public class Transaction
    {
        public long _id;
        public long _userid;
        public DateTime _date;
        public double _balance;
        public long _account_id;
        public long _transaction_type_id;
        public long _currency_id;
        public long _eid;
        public string _desc;
        public DateTime _edate;

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
        public string AccountName
        {
            get { return Account.ByID(this.AccountID).Name; }
        }
        public long AccountID
        {
            get { return _account_id; }
            set { _account_id = value; }
        }
        public string TransactionName
        {
            get { return this.TransactionTypeObj.Type; }
        }
        public bool TransactionDirection
        {
            get { return this.TransactionTypeObj.Direction; }
        }
        public TransactionType TransactionTypeObj
        {
            get { return TransactionType.ByID(this.TransactionTypeID); }
        }
        public long TransactionTypeID
        {
            get { return _transaction_type_id; }
            set { _transaction_type_id = value; }
        }
        public long CurrencyID
        {
            get { return _currency_id; }
            set { _currency_id = value; }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        public double Amount
        {
            get { return _balance; }
            set { _balance = value; }
        }
        public string Description
        {
            get { return _desc; }
            set { _desc = value; }
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

        public Transaction()
        {
        }

        public static List<Transaction> ByUserID_Account(long userid, long accountid)
        {
            List<Transaction> newList = new List<Transaction>();


            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from Transactions where user_id = ?id and account_id = ?acc", conn);
            com.Parameters.AddWithValue("?id", userid);
            com.Parameters.AddWithValue("?acc", accountid);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                Transaction item = new Transaction();
                item.ID = Utility.ToLong(dr["id"]);
                item.UserID = Utility.ToLong(dr["user_id"]);
                item.AccountID = Utility.ToLong(dr["account_id"]);
                item.TransactionTypeID = Utility.ToLong(dr["transaction_type_id"]);
                item.Date = Utility.ToDateTime(dr["date"]);
                item.CurrencyID = Utility.ToLong(dr["currency_id"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.Amount = Utility.ToDouble(dr["amount"]);
                item.Description = Utility.ToString(dr["desc"]);

                newList.Add(item);
            }
            conn.Close();

            return newList;
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
    public class TransactionType
    {
        public long _id;
        public string _type;
        public bool _plus_minus;
        private static MySqlConnection conn;

        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public bool Direction
        {
            get { return _plus_minus; }
            set { _plus_minus = value; }
        }

        public TransactionType() { }
        public static List<TransactionType> All()
        {
            List<TransactionType> newList = new List<TransactionType>();

            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from Transaction_type", conn);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                TransactionType item = new TransactionType();
                item.ID = Utility.ToLong(dr["id"]);
                item.Type = Utility.ToString(dr["type"]);
                item.Direction = Utility.ToBool(dr["plus_minus"]);

                newList.Add(item);
            }
            conn.Close();

            return newList;
        }
        public static TransactionType ByID(long id)
        {
            TransactionType item = new TransactionType();
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from transaction_type where id = ?id", conn);
            com.Parameters.AddWithValue("?id", id);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                item.ID = Utility.ToLong(dr["id"]);
                item.Type = Utility.ToString(dr["type"]);
                item.Direction = Utility.ToBool(dr["plus_minus"]);
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
    public class GunTransaction
    {
        public long _id;
        public long _userid;
        public double _total;
        public long _product_id;
        public string _status;
        public DateTime _edate;
        public DateTime _cdate;

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

        public long ProductID
        {
            get { return _product_id; }
            set { _product_id = value; }
        }
        public string ProductName
        {
            get { return Product.ByID(ProductID).Name; }
        }
        public DateTime CDate
        {
            get { return _cdate; }
            set { _cdate = value; }
        }
        public double Total
        {
            get { return _total; }
            set { _total = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public DateTime EDate
        {
            get { return _edate; }
            set { _edate = value; }
        }

        public GunTransaction()
        {
        }

        public static List<GunTransaction> ByUserID(long userid)
        {
            List<GunTransaction> newList = new List<GunTransaction>();

            using (MySqlConnection conn = new MySqlConnection(Utility.ConnectionString))
            {
                conn.Open();

                MySqlCommand com = new MySqlCommand("Select * from gun_transactions where user_id = ?id order by edate desc", conn);
                com.Parameters.AddWithValue("?id", userid);

                MySqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    GunTransaction item = new GunTransaction();
                    item.ID = Utility.ToLong(dr["id"]);
                    item.UserID = Utility.ToLong(dr["user_id"]);
                    item.ProductID = Utility.ToLong(dr["product_id"]);
                    item.CDate = Utility.ToDateTime(dr["cdate"]);
                    item.EDate = Utility.ToDateTime(dr["edate"]);
                    item.Total = Utility.ToDouble(dr["total"]);
                    item.Status = Utility.ToString(dr["status"]);

                    newList.Add(item);
                }
                conn.Close();
            }
            return newList;
        }
        public static List<GunTransaction> AllOrders()
        {
            List<GunTransaction> newList = new List<GunTransaction>();

            using (MySqlConnection conn = new MySqlConnection(Utility.ConnectionString))
            {
                conn.Open();

                MySqlCommand com = new MySqlCommand("Select * from gun_transactions order by edate desc", conn);

                MySqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    GunTransaction item = new GunTransaction();
                    item.ID = Utility.ToLong(dr["id"]);
                    item.UserID = Utility.ToLong(dr["user_id"]);
                    item.ProductID = Utility.ToLong(dr["product_id"]);
                    item.CDate = Utility.ToDateTime(dr["cdate"]);
                    item.EDate = Utility.ToDateTime(dr["edate"]);
                    item.Total = Utility.ToDouble(dr["total"]);
                    item.Status = Utility.ToString(dr["status"]);

                    newList.Add(item);
                }
                conn.Close();
            }
            return newList;
        }

        public void Insert(long uid)
        {
            using (MySqlConnection conn = new MySqlConnection(Utility.ConnectionString))
            {
                conn.Open();
                try
                {
                    MySqlCommand com = new MySqlCommand("INSERT into gun_transactions(product_id, status, user_id, edate, total) VALUES (?pid, 'Pending', ?uid, NOW(), ?tot)", conn);
                    com.Prepare();
                    com.Parameters.AddWithValue("?uid", uid);
                    com.Parameters.AddWithValue("?pid", this.ProductID);
                    com.Parameters.AddWithValue("?tot", this.Total);
                    com.ExecuteNonQuery();
                }
                catch (Exception ex) { ErrorLog.Log(Utility.ToLong(-1), ex.Message.ToString(), "Transation.cs"); }

            }
        }
    }
}
