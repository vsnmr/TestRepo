using System;
using System.Collections.Generic;
using System.Linq;
using EBillApp.Models;
using EBillApp.Repository;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace EBillApp.Repository
{
    public class Data : IData
    {
        public string ConString { get; set; }
        public Data()
        {
            ConString = ConfigurationManager.ConnectionStrings["AngularProjectDB"].ConnectionString;
        }
        public void SaveBillDetails(BillDetail detail)
        {
            SqlConnection con = new SqlConnection(ConString);
            try
            {
                detail.TotalAmount = detail.Items.Sum(i => i.Price * i.Quantity);
                con.Open();
                SqlCommand cmd = new SqlCommand("USP_SaveBillDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customerName", detail.CustomerName);
                cmd.Parameters.AddWithValue("@MobileNumber", detail.MobileNumber);
                cmd.Parameters.AddWithValue("@customerAddress", detail.CustomerAddress);
                cmd.Parameters.AddWithValue("@totalAmount", detail.TotalAmount);

                SqlParameter outputPara = new SqlParameter();
                outputPara.DbType = DbType.Int32;
                outputPara.Direction = ParameterDirection.Output;
                outputPara.ParameterName = "@id";
                cmd.Parameters.Add(outputPara);
                cmd.ExecuteNonQuery();
                int id = int.Parse(outputPara.Value.ToString());
                if (detail.Items.Count > 0)
                {
                    SaveBillitems(detail.Items, con, id);
                }
            }
            catch
            {

            }
            finally
            {
                con.Close();
            }

        }
        public void SaveBillitems(List<Items> items, SqlConnection con, int id)
        {
            try
            {
                string qry = "insert into tbl_BillItems (ProductName,Price,Quantity,billId)values";
                foreach (var item in items)
                {
                    qry += String.Format("('{0}',{1},{2},{3}),", item.ProductName, item.Price, item.Quantity, id);
                }
                qry = qry.Remove(qry.Length - 1);
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        public List<BillDetail> GetAllDetails()
        {
            List<BillDetail> list = new List<BillDetail>();
            SqlConnection con = new SqlConnection(ConString);
            BillDetail details;

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("USP_GetAllEBillDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    details = new BillDetail();
                    details.Id = int.Parse(reader["Id"].ToString());
                    details.CustomerName = reader["customerName"].ToString();
                    details.MobileNumber = reader["MobileNumber"].ToString();
                    details.CustomerAddress = reader["customerAddress"].ToString();
                    details.TotalAmount = int.Parse(reader["totalAmount"].ToString());
                    list.Add(details);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public BillDetail GetDetail(int id)
        {
            SqlConnection con = new SqlConnection(ConString);
            BillDetail details = new BillDetail();
            Items items;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("USP_GetBillDetailsByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {

                    details.Id = int.Parse(reader["BillId"].ToString());
                    details.CustomerName = reader["customerName"].ToString();
                    details.MobileNumber = reader["MobileNumber"].ToString();
                    details.CustomerAddress = reader["customerAddress"].ToString();
                    details.TotalAmount = int.Parse(reader["totalAmount"].ToString());
                    items = new Items();
                    items.Id = int.Parse(reader["ItemId"].ToString());
                    items.ProductName = reader["productName"].ToString();
                    items.Price = int.Parse(reader["price"].ToString());
                    items.Quantity = int.Parse(reader["quantity"].ToString());
                    details.Items.Add(items);
                }


            }
            catch
            {

            }
            finally
            {
                con.Close();
            }
            return details;
        }
    }
}