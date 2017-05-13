using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Fuse
{
    public class Product
    {
        private static MySqlConnection conn;
        private long id;
        private string name;
        private string desc;
        private string rsr;
        private double price;
        private double quantity;
        private long eid;
        private DateTime edate;
        private long productType;

        public long ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }
        public string RSR
        {
            get { return rsr; }
            set { rsr = value; }
        }
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        public double Quantity
        {
            get { return quantity; }
            set { quantity = value; }
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
        public long ProductType
        {
            get { return productType; }
            set { productType = value; }
        }
        public Product()
        {
        }
        private static void makeConnection()
        {
            conn = new MySqlConnection(ConnectionString);
        }
        public static string ConnectionString
        {
            get { return Utility.ConnectionString; }
        }

        public static Product ByID(long id)
        {
            makeConnection();
            conn.Open();
            MySqlCommand com = new MySqlCommand("Select * from product where id = ?id", conn);
            com.Parameters.AddWithValue("?id", id);
            Product item = new Product();

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                item.ID = Utility.ToLong(dr["id"]);
                item.Name = Utility.ToString(dr["name"]);
                item.Description = Utility.ToString(dr["description"]);
                item.RSR = Utility.ToString(dr["rsr"]);
                item.Price = Utility.ToDouble(dr["price"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.Quantity = Utility.ToDouble(dr["quantity"]);
                item.ProductType = Utility.ToLong(dr["product_type"]);
            }
            if (item.ID > 0)
                return item; 
            return null;
        }
        public static List<Product> Search(string name)
        {
            List<Product> newList = new List<Product>();

            makeConnection();
            conn.Open();
            MySqlCommand com = new MySqlCommand("Select * from product where name regexp ?name or description regexp ?name or rsr regexp ?name", conn);
            com.Parameters.AddWithValue("?name", name);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                Product item = new Product();
                item.ID = Utility.ToLong(dr["id"]);
                item.Name = Utility.ToString(dr["name"]);
                item.Description = Utility.ToString(dr["description"]);
                item.RSR = Utility.ToString(dr["rsr"]);
                item.Price = Utility.ToDouble(dr["price"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.Quantity = Utility.ToDouble(dr["quantity"]);
                item.ProductType = Utility.ToLong(dr["product_type"]);

                newList.Add(item);
            }
            conn.Close();

            return newList;
        }
        public static List<Product> AllProducts()
        {
            List<Product> newList = new List<Product>();

            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from product", conn);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                Product item = new Product();
                item.ID = Utility.ToLong(dr["id"]);
                item.Name = Utility.ToString(dr["name"]);
                item.Description = Utility.ToString(dr["description"]);
                item.RSR = Utility.ToString(dr["rsr"]);
                item.Price = Utility.ToDouble(dr["price"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.Quantity = Utility.ToDouble(dr["quantity"]);
                item.ProductType = Utility.ToLong(dr["product_type"]);

                newList.Add(item);
            }
            conn.Close();

            return newList;
        }
        public static List<Product> ByProductTypeID(long proType)
        {
            List<Product> newList = new List<Product>();

            makeConnection();
            conn.Open();

            MySqlCommand com = new MySqlCommand("Select * from product where product_type = ?id", conn);
            com.Parameters.AddWithValue("?id", proType);

            MySqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                Product item = new Product();
                item.ID = Utility.ToLong(dr["id"]);
                item.Name = Utility.ToString(dr["name"]);
                item.Description = Utility.ToString(dr["description"]);
                item.RSR = Utility.ToString(dr["rsr"]);
                item.Price = Utility.ToDouble(dr["price"]);
                item.EID = Utility.ToLong(dr["eid"]);
                item.EDate = Utility.ToDateTime(dr["edate"]);
                item.Quantity = Utility.ToDouble(dr["quantity"]);
                item.ProductType = Utility.ToLong(dr["product_type"]);

                newList.Add(item);
            }
            conn.Close();

            return newList;
        }

        public void Insert(long uid)
        {
            using (MySqlConnection conn = new MySqlConnection(Utility.ConnectionString))
            {
                conn.Open();
                try
                {
                    MySqlCommand com = new MySqlCommand("INSERT into product(name, description, rsr, price, quantity, eid, edate, product_type) VALUES (?name, ?desc, ?rsr, ?price, ?qty, ?eid, NOW(), ?protype)", conn);
                    com.Prepare();
                    com.Parameters.AddWithValue("?name", this.Name);
                    com.Parameters.AddWithValue("?desc", this.Description);
                    com.Parameters.AddWithValue("?rsr", this.RSR);
                    com.Parameters.AddWithValue("?price", this.Price);
                    com.Parameters.AddWithValue("?qty", this.Quantity);
                    com.Parameters.AddWithValue("?protype", this.ProductType);
                    com.Parameters.AddWithValue("?eid", uid);
                    com.ExecuteNonQuery();
                }
                catch (Exception ex) { ErrorLog.Log(Utility.ToLong(-1), ex.Message.ToString(), "Product.cs"); }
            }
        }
    }
    public class ProductType
    {
        private long _id;
        private string _type;

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

        public ProductType()
        {
        }

        public static List<ProductType> getTypes()
        {
            List<ProductType> protype = new List<ProductType>();

            using (MySqlConnection conn = new MySqlConnection(Utility.ConnectionString))
            {
                conn.Open();
                MySqlCommand com = new MySqlCommand("Select * from product_type", conn);

                MySqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    ProductType item = new ProductType();
                    item.ID = Utility.ToLong(dr["id"]);
                    item.Type = Utility.ToString(dr["type"]);

                    protype.Add(item);
                }
            }
            return protype;
        }
        public void Insert(long uid)
        {
            using (MySqlConnection conn = new MySqlConnection(Utility.ConnectionString))
            {
                conn.Open();
                try
                {
                    MySqlCommand com = new MySqlCommand("INSERT into product_type(type) VALUES (?type)", conn);
                    com.Prepare();
                    com.Parameters.AddWithValue("?protype", this.Type); 
                    com.ExecuteNonQuery();

                }
                catch (Exception ex) { ErrorLog.Log(Utility.ToLong(-1), ex.Message.ToString(), "ProductType.cs"); }
            }
        }

    }
}
