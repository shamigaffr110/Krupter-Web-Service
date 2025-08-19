
using System;

namespace EBillingWeb
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void btnGo_Click(object sender, EventArgs e)
        {
            int n = 0;
            try { n = Convert.ToInt32(txtN.Text.Trim()); } catch { n = 0; }
            ElectricityBoard eb = new ElectricityBoard();
            var list = eb.Generate_N_BillDetails(n);
            grid.DataSource = list;
            grid.DataBind();
        }
    }
}
