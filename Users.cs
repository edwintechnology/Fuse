using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fuse
{
    public class User
    {
        private long id;
        private long eid;
        private DateTime edate;
        private bool act;
        private string pass;
        private string user;
        private static MySqlConnection conn;
        private bool admin;

        public long ID
        {
            get { return id; }
            set { id = value; }
        }

        public long EID
        {
            get { return eid; }
            set { eid = value; }
        }
        public DateTime EDate
        {
            get { return edate; }
            set { edate = value; }
        }

        public bool Active
        {
            get { return act; }
            set { act = value; }
        }
        public string Username
        {
            get { return user; }
            set { user = value; }
        }
        public string Password
        {
            get { return pass; }
            set { pass = value; }
        }
        public bool Admin
        {
            get { return admin; }
            set { admin = value; }
        }
        public User()
        {
            // default
        }
        public long ByLastInsertedID()
        {
            long id = -1;
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from Users order by id desc limit 1", conn);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                id = Utility.ToLong(dr["id"]);
            }
            conn.Close();
            return id;
        }
        public static User ByID(Int64 id)
        {
            User item = new User();
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from Users where id = ?id", conn);
            com.Parameters.AddWithValue("?id", id);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                item.ID = Utility.ToLong(dr["id"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.Active = Utility.ToBool(dr["active"]);
                item.Username = Utility.ToString(dr["username"]);
                item.Password = Utility.ToString(dr["password"]);
                item.Admin = Utility.ToBool(dr["Admin"]);
            }
            conn.Close();
            if (item != null)
                return item;
            return null;
        }
        public long Insert()
        {
            makeConnection();
            conn.Open();
            try
            {
                MySqlCommand com = new MySqlCommand("INSERT into Users(Username, Password, Active, eid, edate, admin) VALUES (?un, ?pw, ?act, ?eid, ?edate, 0)", conn);
                com.Prepare();
                com.Parameters.AddWithValue("?un", this.Username);
                com.Parameters.AddWithValue("?pw", this.Password);
                com.Parameters.AddWithValue("?act", this.Active);
                com.Parameters.AddWithValue("?eid", this.EID);
                com.Parameters.AddWithValue("?edate", this.EDate);

                com.ExecuteNonQuery();
            }
            catch (Exception ex) { ErrorLog.Log(Utility.ToLong(-1), ex.Message.ToString(), "Users.cs"); return -1; }
            // need some checks here for existances
            return ByLastInsertedID();
        }
        public static User Create(string firstname, string lastname, string username, string password, string email, string state, string country)
        {
            User newUser = new User();
            newUser.Username = username;
            newUser.Password = password;
            newUser.EDate = DateTime.Now;
            newUser.EID = Utility.ToLong(1);
            newUser.Admin = false;
            newUser.Active = true;
            long userid = newUser.Insert();
            UserInfo newInfo = new UserInfo(userid, firstname, lastname, email, state, country);
            newInfo.Insert(userid);

            return User.ByID(userid);
        }
        // need a method for checking username on sigup
        public static bool isTaken(string user)
        {
	        string query = "select count(*) from Users where username = ?user";
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand(query, conn);
            com.Parameters.AddWithValue("?user", user);
            int count = Utility.ToInt(com.ExecuteScalar());

            return (count > 0);
        }
        public static User ByScreenName(string sn)
        {
            User item = new User();

            makeConnection();
            conn.Open();
            MySqlCommand com = new MySqlCommand("Select * from Users where username = ?0", conn);
            com.Parameters.AddWithValue("?0", sn);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                item.ID = Utility.ToLong(dr["id"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.Active = Utility.ToBool(dr["Active"]);
                item.Username = Utility.ToString(dr["Username"]);
                item.Password = Utility.ToString(dr["Password"]);
                item.Admin = Utility.ToBool(dr["Admin"]);
            }
            conn.Close();
            if (item.ID > 0)
                return item;
            else
                return null;
        }
        public static User LoginCheck(string u, string p)
        {
            User item = new User();
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from Users where username = ?0 and password = ?1", conn);
            com.Parameters.AddWithValue("?0", u);
            com.Parameters.AddWithValue("?1", p);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                item.ID = Utility.ToLong(dr["id"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.Active = Utility.ToBool(dr["Active"]);
                item.Username = Utility.ToString(dr["Username"]);
                item.Password = Utility.ToString(dr["Password"]);
                item.Admin = Utility.ToBool(dr["Admin"]);
            }
            conn.Close();
            if (item.ID > 0)
                return item;
            else
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
    public class UserInfo
    {
        private long _id;
        private long _userid;
        private string _fname;
        private string _lname;
        private string _email;
        private string _state;
        private string _country;
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
        public string FirstName
        {
            get { return _fname; }
            set { _fname = value; }
        }
        public string LastName
        {
            get { return _lname; }
            set { _lname = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public UserInfo()
        {
            // default
        }
        public UserInfo(long id, string fname, string lname, string email, string state, string country)
        {
            ID = id;
            FirstName = fname;
            LastName = lname;
            Email = email;
            State = state;
            Country = country;
        }
        private static void makeConnection()
        {
            conn = new MySqlConnection(ConnectionString);
        }
        public static string ConnectionString
        {
            get { return Utility.ConnectionString; }
        }

        public static UserInfo ByUID(long uid)
        {
            UserInfo item = new UserInfo();
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from UserInformation where user_id = ?id", conn);
            com.Parameters.AddWithValue("?id", uid);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                item.ID = Utility.ToLong(dr["id"]);
                item.UserID = Utility.ToLong(dr["user_id"]);
                item.FirstName = Utility.ToString(dr["first_name"]);
                item.LastName = Utility.ToString(dr["last_name"]);
                item.Email = Utility.ToString(dr["email"]);
                item.State = Utility.ToString(dr["state"]);
                item.Country = Utility.ToString(dr["country"]);
            }
            conn.Close();
            if (item.ID > 0)
                return item;
            return null;
        }
        public void Update()
        {
            makeConnection();
            conn.Open();
            try
            {
                MySqlCommand com = new MySqlCommand("UPDATE userinformation SET first_name = ?fn, last_name = ?ln, email = ?email, state = ?ST, country = ?country WHERE user_id = ?uid", conn);
                com.Prepare();
                com.Parameters.AddWithValue("?uid", this.UserID);
                com.Parameters.AddWithValue("?fn", this.FirstName);
                com.Parameters.AddWithValue("?ln", this.LastName);
                com.Parameters.AddWithValue("?email", this.Email);
                com.Parameters.AddWithValue("?ST", this.State);
                com.Parameters.AddWithValue("?country", this.Country);
                com.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                conn.Close();
            }
        }
        public long Insert(long uid)
        {
            makeConnection();
            conn.Open();
            try
            {
                MySqlCommand com = new MySqlCommand("INSERT into UserInformation(user_id, first_name, last_name, email, state, country) VALUES (?uid, ?fn, ?ln, ?email, ?ST, ?country)", conn);
                com.Prepare();
                com.Parameters.AddWithValue("?uid", uid);
                com.Parameters.AddWithValue("?fn", this.FirstName);
                com.Parameters.AddWithValue("?ln", this.LastName);
                com.Parameters.AddWithValue("?email", this.Email);
                com.Parameters.AddWithValue("?ST", this.State);
                com.Parameters.AddWithValue("?country", this.Country);
                com.ExecuteNonQuery();
            }
            catch (Exception ex) { ErrorLog.Log(Utility.ToLong(-1), ex.Message.ToString(), "UserInfo.cs"); return -1; }
            // need some checks here for existances
            UserInfo ui = UserInfo.ByUID(uid);
            return ui.ID;
        }
    }
    public class UserAddress
    {
        private long _id;
        private long _userid;
        private string _addr;
        private string _apt;
        private string _city;
        private string _zip;
        private string _state;
        private string _country;
        private string _phone;

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
        public string Address
        {
            get { return _addr; }
            set { _addr = value; }
        }
        public string Apartment
        {
            get { return _apt; }
            set { _apt = value; }
        }
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public UserAddress()
        {
            // default
        }
        public UserAddress(long uid, string city, string state, string address, string apt, string zip, string phone, string country)
        {
            UserID = uid;
            Address = address;
            City = city;
            State = state;
            Country = country;
            Apartment = apt;
            Zip = zip;
            Phone = phone;
        }
        private static void makeConnection()
        {
            conn = new MySqlConnection(ConnectionString);
        }
        public static string ConnectionString
        {
            get { return Utility.ConnectionString; }
        }

        public static UserAddress ByUID(long uid)
        {
            UserAddress item = new UserAddress();
            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from user_address where user_id = ?id", conn);
            com.Parameters.AddWithValue("?id", uid);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                item.ID = Utility.ToLong(dr["id"]);
                item.UserID = Utility.ToLong(dr["user_id"]);
                item.Address = Utility.ToString(dr["address"]);
                item.Apartment = Utility.ToString(dr["apt"]);
                item.City = Utility.ToString(dr["city"]);
                item.State = Utility.ToString(dr["state"]);
                item.Zip = Utility.ToString(dr["zipcode"]);
                item.Country = Utility.ToString(dr["country"]);
                item.Phone = Utility.ToString(dr["phone"]);
            }
            conn.Close();
            if (item.ID > 0)
                return item;
            return null;
        }
        public void Update()
        {
            makeConnection();
            conn.Open();
            try
            {
                MySqlCommand com = new MySqlCommand("UPDATE user_address SET address = ?addr, apt = ?apt, city = ?city, state = ?ST, zip = ?zip, country = ?country, phone = ?phone WHERE user_id = ?eid", conn);
                com.Prepare();
                com.Parameters.AddWithValue("?addr", this.Address);
                com.Parameters.AddWithValue("?apt", this.Apartment);
                com.Parameters.AddWithValue("?city", this.City);
                com.Parameters.AddWithValue("?phone", this.Phone);
                com.Parameters.AddWithValue("?ST", this.State);
                com.Parameters.AddWithValue("?country", this.Country);
                com.Parameters.AddWithValue("?zip", this.Zip);
                com.Parameters.AddWithValue("?eid", this.UserID);
                com.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                conn.Close();
            }
        }
        public long Insert(long uid)
        {
            makeConnection();
            conn.Open();
            try
            {
                MySqlCommand com = new MySqlCommand("INSERT into user_address(user_id, address, apt, city, state, zip, country, phone) VALUES (?uid, ?addr, ?apt, ?city, ?phone, ?ST, ?country, ?zip)", conn);
                com.Prepare();
                com.Parameters.AddWithValue("?uid", uid);
                com.Parameters.AddWithValue("?addr", this.Address);
                com.Parameters.AddWithValue("?apt", this.Apartment);
                com.Parameters.AddWithValue("?city", this.City);
                com.Parameters.AddWithValue("?phone", this.Phone); 
                com.Parameters.AddWithValue("?ST", this.State);
                com.Parameters.AddWithValue("?country", this.Country);
                com.Parameters.AddWithValue("?zip", this.Zip);
                com.ExecuteNonQuery();
            }
            catch (Exception ex) { ErrorLog.Log(Utility.ToLong(-1), ex.Message.ToString(), "address.cs"); return -1; }
            // need some checks here for existances
            UserInfo ui = UserInfo.ByUID(uid);
            return ui.ID;
        }
    }
}
