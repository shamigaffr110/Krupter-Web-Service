using System;
using System.Data.SqlClient;
using EB.Data;

namespace ElectricityBillWebApp
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void btnGen_Click(object sender, EventArgs e)
        {
            string upi = "upi://pay?pa=ebill@upi&pn=EBillBoard&am=" + txtAmount.Text + "&tn=" + txtConsumer.Text;
            string script = "showQR('" + upi.Replace("'", "") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "qr", script, true);
            lblMsg.Text = "Scan the QR or use UPI intent: " + upi;
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = DBHandler.GetConnection();
                SqlCommand cmd = new SqlCommand("INSERT INTO Payments(user_id,consumer_number,amount,method,status,txn_ref) VALUES(@u,@c,@a,@m,'Success',@r); UPDATE ElectricityBill SET status='Paid' WHERE consumer_number=@c;", con);
                int uid = 0;
                if (Session["user_id"] != null) uid = Convert.ToInt32(Session["user_id"]);
                cmd.Parameters.AddWithValue("@u", uid);
                cmd.Parameters.AddWithValue("@c", txtConsumer.Text);
                cmd.Parameters.AddWithValue("@a", Convert.ToDouble(txtAmount.Text));
                cmd.Parameters.AddWithValue("@m", ddlMethod.SelectedValue);
                cmd.Parameters.AddWithValue("@r", Guid.NewGuid().ToString().Substring(0,10));
                con.Open(); cmd.ExecuteNonQuery(); con.Close();
                lblMsg.Text = "Payment saved and bill marked as Paid.";
                // Fetch last payment id for this user to link receipt
                SqlCommand q = new SqlCommand("SELECT TOP 1 payment_id FROM Payments WHERE user_id=@u ORDER BY payment_date DESC", con);
                q.Parameters.AddWithValue("@u", uid);
                con.Open(); object pid = q.ExecuteScalar(); con.Close();
                if (pid != null) { lblMsg.Text += " <a href='BillReceipt.aspx?paymentId="+ pid +"'>View Receipt</a>"; }

                // Auto-send PDF receipt email after payment
                try {
                    // fetch payment info
                    SqlCommand da = new SqlCommand(@"SELECT TOP 1 p.payment_id,p.consumer_number,p.amount,p.payment_date,p.method,p.txn_ref,
                                    c.email,e.consumer_name
                                    FROM Payments p
                                    LEFT JOIN Connections c ON c.consumer_number=p.consumer_number
                                    LEFT JOIN ElectricityBill e ON e.consumer_number=p.consumer_number
                                    WHERE p.payment_id=(SELECT MAX(payment_id) FROM Payments WHERE user_id=@u)", con);
                    da.Parameters.AddWithValue("@u", uid);
                    con.Open();
                    SqlDataReader dr = da.ExecuteReader();
                    if (dr.Read())
                    {
                        string email = dr["email"].ToString();
                        string cname = dr["consumer_name"].ToString();
                        string subj = "JBVNL – Payment Receipt Confirmation";
                        string body = "<p>Dear " + cname + ",</p><p>We have received your payment of Rs." + dr["amount"] +
                                      " for Consumer No " + dr["consumer_number"] + ".</p>" +
                                      "<p>Txn Ref: " + dr["txn_ref"] + "<br/>Date: " + Convert.ToDateTime(dr["payment_date"]).ToString("yyyy-MM-dd HH:mm") + "</p>" +
                                      "<p>You can also download your receipt from the portal.</p><p>Thank you for using JBVNL Utility Services.</p>";
                        
                try {
                    SqlCommand da = new SqlCommand(@"SELECT TOP 1 p.payment_id,p.consumer_number,p.amount,p.payment_date,p.method,p.txn_ref,
                                    c.email,e.consumer_name
                                    FROM Payments p
                                    LEFT JOIN Connections c ON c.consumer_number=p.consumer_number
                                    LEFT JOIN ElectricityBill e ON e.consumer_number=p.consumer_number
                                    WHERE p.payment_id=(SELECT MAX(payment_id) FROM Payments WHERE user_id=@u)", con);
                    da.Parameters.AddWithValue("@u", uid);
                    con.Open();
                    SqlDataReader dr = da.ExecuteReader();
                    if (dr.Read())
                    {
                        string email = dr["email"].ToString();
                        string cname = dr["consumer_name"].ToString();
                        int pid2 = Convert.ToInt32(dr["payment_id"]);
                        string subj = "JBVNL – Payment Receipt Confirmation";
                        string body = "<p>Dear " + cname + ",</p><p>We have received your payment of Rs." + dr["amount"] +
                                      " for Consumer No " + dr["consumer_number"] + ".</p>" +
                                      "<p>Txn Ref: " + dr["txn_ref"] + "<br/>Date: " + Convert.ToDateTime(dr["payment_date"]).ToString("yyyy-MM-dd HH:mm") + "</p>" +
                                      "<p>Attached is your official payment receipt PDF.<br/>You can also download it anytime from the portal.</p><p>Thank you for using JBVNL Utility Services.</p>";
                        string[] lines = new string[] {
                            "Payment ID: " + pid2,
                            "Txn Ref: " + dr["txn_ref"],
                            "Consumer: " + dr["consumer_number"] + " - " + cname,
                            "Amount: Rs. " + dr["amount"],
                            "Method: " + dr["method"],
                            "Date: " + Convert.ToDateTime(dr["payment_date"]).ToString("yyyy-MM-dd HH:mm")
                        };
                        byte[] pdf = ElectricityBillWebApp.Classes.TinyPdf.MakeSimpleReceipt("JBVNL – Payment Receipt", lines);
                        ElectricityBillWebApp.Classes.EmailService.SendMail(email, subj, body, null, null, pdf, "Receipt_" + pid2 + ".pdf");
                    }
                    con.Close();
                } catch (Exception ex) { /* ignore email errors */ }
    
                    }
                    con.Close();
                } catch (Exception ex) { /* ignore email errors */ }
    
            }
            catch (Exception ex) { lblMsg.Text = "Error: " + ex.Message; }
        }
    }
}
