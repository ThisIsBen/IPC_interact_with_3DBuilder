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

       /*
        //run 宗霖 program to activate 3DBuilder indirectly
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.Verb = "runas";
        startInfo.WorkingDirectory = Request.MapPath("~/");
        
        //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //startInfo.CreateNoWindow = true;
        
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = true;

        startInfo.FileName = Request.MapPath("testopenfile.lnk");
        //startInfo.FileName = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\installutil.exe";
        //startInfo.Arguments = "D:\\Projects\\MyNewService\\bin\\Release\\MyNewService.exe";
        process.StartInfo = startInfo;
        bool processStarted = process.Start();
        */

    }

    protected void ALButton_Click(object sender, EventArgs e)
    {
        ///*Ben test
        if (!string.IsNullOrEmpty(hintID.Value))
        {

            //Process[] processes = Process.GetProcessById
            //File.Copy(Server.MapPath("~") + @"\CSNamedPipe.exe", Server.MapPath("~") + @"\" + TBX_Input.Value + ".exe");
            os.StartInfo.Verb = "runas";
            os.StartInfo.WorkingDirectory = Request.MapPath("~/");
            os.StartInfo.FileName = Request.MapPath("App_Code/CSNamedPipe/bin/Debug/CSNamedPipe.exe");
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
        // * */

        /*Ben test get the existing running CSNamedPipe
            // Get all instances of Notepad running on the local computer.
            // This will return an empty array if notepad isn't running.
            Process[] localByName = Process.GetProcessesByName("CSNamedPipe");


        
             StreamWriter wr = localByName[0].StandardInput;
            //os.StandardInput.Close();
        
            Session["Writer"] = wr;
            Session["Process"] = localByName[0];
            hintID.Value = "";
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>alert('"+os.Id+"');</script>");
            Response.Redirect("ALHomePage.aspx");
         */

    }
}