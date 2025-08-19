
using System;
using System.Configuration;

namespace EBillingWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == ConfigurationManager.AppSettings["AdminUser"]
                && txtPass.Text == ConfigurationManager.AppSettings["AdminPass"])
            {
                Session["auth"] = true;
                Response.Redirect("Bills.aspx");
            }
            else
            {
                lblMsg.Text = "Invalid login";
            }
        }
    }
}
