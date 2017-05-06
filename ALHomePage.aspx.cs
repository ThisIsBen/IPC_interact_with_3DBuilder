using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;


public partial class ALHomePage : System.Web.UI.Page
{
    //public static Process os = PreviousPage.os;
    protected void Page_Load(object sender, EventArgs e)
    {
        //int osID = Int32.Parse(Session["osId"].ToString());
        //os = System.Diagnostics.Process.GetProcessById(osID);
        //os.StartInfo.RedirectStandardInput = true;
        //os.StartInfo.UseShellExecute = false;
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        StreamWriter wr = (StreamWriter)Session["Writer"];
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>alert('" + os.StartInfo.RedirectStandardInput + os.Id + "');</script>"); 
        //StreamWriter wr = os.StandardInput;
      
        wr.WriteLine("1 2");
        //wr.WriteLine("1 2");
        
        Response.Redirect("Items.aspx");
    }
}