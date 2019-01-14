using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;


 
   

public partial class _Default : System.Web.UI.Page
{
    public static Process os = new Process();

    [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

   
    [DllImport("USER32.DLL")]
    //static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab); doesn't work
    public static extern bool SetForegroundWindow(IntPtr hWnd);


    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        var proc1 = new ProcessStartInfo();
        string anyCommand="ipconfig";
        proc1.UseShellExecute = true;

        proc1.WorkingDirectory = @"C:\Windows\System32";

        proc1.FileName = @"C:\Windows\System32\cmd.exe";
        proc1.Verb = "runas";
        proc1.Arguments = "/c " + anyCommand;
        //proc1.WindowStyle = ProcessWindowStyle.Hidden;
        Process.Start(proc1);
         * */

        
        //this works
        /*
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.Verb = "runas";
        startInfo.WorkingDirectory = Request.MapPath("~/");
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.CreateNoWindow = true;
        startInfo.FileName = Request.MapPath("3DBuilder.lnk");
        //startInfo.FileName = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\installutil.exe";
        //startInfo.Arguments = "D:\\Projects\\MyNewService\\bin\\Release\\MyNewService.exe";
        process.StartInfo = startInfo;
        bool processStarted = process.Start();
        */

        
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

        //Run a process as Administrator with C# programmatically 
        startInfo.Verb = "runas";

        //run 3DBuilder
        startInfo.WorkingDirectory = Request.MapPath("~/");
        startInfo.FileName = Request.MapPath("3DBuilder.lnk");

        //display the window of the 3DBuilder
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = true;
        
        //start running 3DBuilder
        process.StartInfo = startInfo;
        bool processStarted = process.Start();
         
        //bring 3DBuilder to foreground
        //bringToFront("3DBuilder MFC Application");
        bringToFront(process);
         
        

        //from forum
        /*
        System.Diagnostics.Process proc = new System.Diagnostics.Process();

       

        proc.StartInfo.UseShellExecute = false;
        //Run a process as Administrator with C# programmatically 
        proc.StartInfo.Verb = "runas";
        proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        

        proc.StartInfo.WorkingDirectory = Request.MapPath("~/");
        proc.StartInfo.FileName = Request.MapPath("3DBuilder.lnk");

        proc.Start();
        */
        

    }
    public static void bringToFront(System.Diagnostics.Process process)
   // public static void bringToFront(string title)
    {
        // Get a handle to the Calculator application.
        //IntPtr handle = FindWindow(null, title);
        
        IntPtr handle = process.MainWindowHandle;

        // Verify that Calculator is a running process.
        if (handle == IntPtr.Zero)
        {
            return;
        }
        

        // Make Calculator the foreground application
        SetForegroundWindow(handle);
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