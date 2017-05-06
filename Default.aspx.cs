using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;


public partial class _Default : System.Web.UI.Page
{
    public static Process os = new Process();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ALButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hintID.Value))
        {

            //Process[] processes = Process.GetProcessById
            //File.Copy(Server.MapPath("~") + @"\CSNamedPipe.exe", Server.MapPath("~") + @"\" + TBX_Input.Value + ".exe");
            
            os.StartInfo.WorkingDirectory = Request.MapPath("~/");
            os.StartInfo.FileName = Request.MapPath("CSNamedPipe.exe");
            os.StartInfo.UseShellExecute = false;
            os.StartInfo.RedirectStandardInput = true;
            os.StartInfo.Arguments = hintID.Value;
            os.Start();
            StreamWriter wr = os.StandardInput;
            //os.StandardInput.Close();
            Session["Writer"] = wr;
            Session["Process"] = os;
            hintID.Value = "";
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>alert('"+os.Id+"');</script>");
            Response.Redirect("ALHomePage.aspx");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>alert('請輸入帳號於欄位內');</script>");

        }

        
    }
}