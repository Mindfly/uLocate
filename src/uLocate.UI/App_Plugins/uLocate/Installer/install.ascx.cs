using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uLocate;

namespace uLocate.UI.App_Plugins.uLocate.Installer
{
    public partial class install : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool Result = Data.Helper.InitializeDatabase();

            if (Result == true)
            {
                this.lblResult.Text = "Successfully installed uLocate Database tables.";
            }
            else
            {
                this.lblResult.Text = "Error installing uLocate Database tables. Check the umbracoLog for details.";
            }
        }
    }
}