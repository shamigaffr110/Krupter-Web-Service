
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EBillingWeb
{
    public partial class Bills : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["auth"] == null) Response.Redirect("Login.aspx");
            }
        }

        protected void btnPrepare_Click(object sender, EventArgs e)
        {
            phRows.Controls.Clear();
            lblPrepMsg.Text = "";
            int n = 0;
            try { n = Convert.ToInt32(txtCount.Text.Trim()); } catch { n = 0; }
            if (n <= 0) { lblPrepMsg.Text = "Please enter a valid number (> 0)"; return; }

            for (int i = 1; i <= n; i++)
            {
                Panel row = new Panel { CssClass = "card" };
                row.Controls.Add(new Literal { Text = "<b>Bill " + i + "</b><br/>" });

                row.Controls.Add(new Literal { Text = "<label>Consumer Number</label>" });
                row.Controls.Add(new TextBox { ID = "cno" + i });

                row.Controls.Add(new Literal { Text = "<label>Consumer Name</label>" });
                row.Controls.Add(new TextBox { ID = "cname" + i });

                row.Controls.Add(new Literal { Text = "<label>Units Consumed</label>" });
                row.Controls.Add(new TextBox { ID = "units" + i });

                phRows.Controls.Add(row);
            }
            btnSaveAll.Visible = true;
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            BillValidator val = new BillValidator();
            ElectricityBoard board = new ElectricityBoard();

            int count = 0;
            try { count = Convert.ToInt32(txtCount.Text.Trim()); } catch { count = 0; }
            if (count <= 0) { lblMsg.Text = "Nothing to save"; return; }

            List<ElectricityBill> saved = new List<ElectricityBill>();

            for (int i = 1; i <= count; i++)
            {
                string cno = (phRows.FindControl("cno" + i) as TextBox)?.Text.Trim();
                string cname = (phRows.FindControl("cname" + i) as TextBox)?.Text.Trim();
                string utext = (phRows.FindControl("units" + i) as TextBox)?.Text.Trim();

                int units = 0;
                try { units = Convert.ToInt32(utext); } catch { lblMsg.Text = "Units must be a number (row " + i + ")"; return; }

                string msg = val.ValidateUnitsConsumed(units);
                if (!string.IsNullOrEmpty(msg)) { lblMsg.Text = msg + " (row " + i + ")"; return; }

                try
                {
                    ElectricityBill eb = new ElectricityBill();
                    eb.ConsumerNumber = cno; // may throw FormatException
                    eb.ConsumerName = cname;
                    eb.UnitsConsumed = units;
                    board.CalculateBill(eb);
                    board.AddBill(eb);
                    saved.Add(eb);
                }
                catch (FormatException fx) { lblMsg.Text = fx.Message; return; }
                catch (Exception ex) { lblMsg.Text = "Error: " + ex.Message; return; }
            }

            lblMsg.Text = "Saved " + saved.Count + " bills successfully!";
        }

        protected void btnFetch_Click(object sender, EventArgs e)
        {
            int n = 0;
            try { n = Convert.ToInt32(txtLastN.Text.Trim()); } catch { n = 0; }
            ElectricityBoard board = new ElectricityBoard();
            var list = board.Generate_N_BillDetails(n);
            grdBills.DataSource = list;
            grdBills.DataBind();
        }
    }
}
