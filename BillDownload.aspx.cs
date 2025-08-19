
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using ConnectionLib;

// Requires iTextSharp via NuGet: Install-Package iTextSharp -Version 5.5.13.3
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EBillingWeb
{
    public partial class BillDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string cno = Convert.ToString(Request.QueryString["cno"]);
            if (string.IsNullOrWhiteSpace(cno)) { Response.End(); return; }

            string name = "", units = "", amount = "";
            using (var con = new DBHandler().GetConnection())
            using (var cmd = new SqlCommand("SELECT TOP 1 consumer_number, consumer_name, units_consumed, bill_amount FROM ElectricityBill WHERE consumer_number=@c", con))
            {
                cmd.Parameters.AddWithValue("@c", cno);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        name = Convert.ToString(r["consumer_name"]);
                        units = Convert.ToString(r["units_consumed"]);
                        amount = Convert.ToDecimal(r["bill_amount"]).ToString("N2");
                    }
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 50, 50, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();
                var title = new Paragraph("Electricity Bill") { Alignment = Element.ALIGN_CENTER };
                doc.Add(title);
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("Consumer Number: " + cno));
                doc.Add(new Paragraph("Consumer Name: " + name));
                doc.Add(new Paragraph("Units Consumed: " + units));
                doc.Add(new Paragraph("Bill Amount: ₹" + amount));
                doc.Close();

                byte[] bytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Bill_" + cno + ".pdf");
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.End();
            }
        }
    }
}
