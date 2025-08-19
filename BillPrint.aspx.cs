
using System;
using System.Data.SqlClient;
using ConnectionLib;

namespace EBillingWeb
{
    public partial class BillPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            string cno = Convert.ToString(Request.QueryString["cno"]);
            if (string.IsNullOrWhiteSpace(cno)) return;

            using (var con = new DBHandler().GetConnection())
            using (var cmd = new SqlCommand("SELECT TOP 1 consumer_number, consumer_name, units_consumed, bill_amount FROM ElectricityBill WHERE consumer_number=@c", con))
            {
                cmd.Parameters.AddWithValue("@c", cno);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        lblCno.Text = "Consumer Number: " + Convert.ToString(r["consumer_number"]);
                        lblCname.Text = "Consumer Name: " + Convert.ToString(r["consumer_name"]);
                        lblUnits.Text = "Units Consumed: " + Convert.ToString(r["units_consumed"]);
                        lblAmount.Text = "Bill Amount: ₹" + Convert.ToDecimal(r["bill_amount"]).ToString("N2");
                    }
                }
            }
        }
    }
}
