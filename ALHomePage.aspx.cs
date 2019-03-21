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
    public static Process os = new Process();

    //set the default value of each parameters that are retrieved from URL.

     
    //string questionXMLPath = "tea1_Q_20181210231100"; //surgery mode xml file name //it's included in exam paper cPaperID=tea120181122231914 and cPaperID=tea120181126201801
    string questionXMLPath = "tea1_Q_20181225144451";
    string studentUserID = "stu2";
    string examMode = "Yes";//ExamMode的中控, we set its default value to Yes



    protected void Page_Load(object sender, EventArgs e)
    {
        //int osID = Int32.Parse(Session["osId"].ToString());
        //os = System.Diagnostics.Process.GetProcessById(osID);
        //os.StartInfo.RedirectStandardInput = true;
        //os.StartInfo.UseShellExecute = false;

        //check if the ASP Application Variable works
        Response.Write("The num of users online=" + Application["visitors"].ToString()); 

        if (!IsPostBack)
        {
            //temporarily we only activate CSNamedPipe.exe, and manually activate 3DBuilder before clicking the button btnCheck
            //run CSNamedPipe.exe
            runCSNamedPipe();
        }

    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {


        //get the parameters in URL and store there value in global var.
        retrieveURLParameters();



        StreamWriter wr = (StreamWriter)Session["Writer"];
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>alert('" + os.StartInfo.RedirectStandardInput + os.Id + "');</script>"); 
        //StreamWriter wr = os.StandardInput;
      
        wr.WriteLine("1 2");
        //wr.WriteLine("1 2");

        Response.Redirect("Items.aspx?examMode=" + examMode + "&strQID=" + questionXMLPath + "&strUserID=" + studentUserID);
    }

    //get the parameters in URL and store there value in global var.
    private void retrieveURLParameters()
    {
        //set the variable questionXMLPath with the parameter strQID in URL if it is provided.
        if (Request.QueryString["strQID"] != null && Request.QueryString["strQID"] != "")
        {
            questionXMLPath = Request.QueryString["strQID"];
        }

        //set the variable studentUserID with the parameter strUserID in URL if it is provided.
        if (Request.QueryString["strUserID"] != null && Request.QueryString["strUserID"] != "")
        {
            studentUserID = Request.QueryString["strUserID"];
        }


        //set the variable ExamMode with the parameter ExamMode in URL if it is provided.
        if (Request.QueryString["ExamMode"] != null && Request.QueryString["ExamMode"] != "")
        {
            examMode = Request.QueryString["ExamMode"];
        }

        
    }


    //run the CSNamedPipe.exe
    private void runCSNamedPipe()
    {
        //originating from Default.aspx
        //Process os = new Process();
        string hintID = "5555";//this hintID is hard-coded by 昇宏學長

        os.StartInfo.WorkingDirectory = Request.MapPath("~/");
        os.StartInfo.FileName = Request.MapPath("App_Code/CSNamedPipe/bin/Debug/CSNamedPipe.exe");
        os.StartInfo.UseShellExecute = false;
        os.StartInfo.RedirectStandardInput = true;
        /*
        os.StartInfo.Arguments = hintID;
        */
        //pass Hints's userID to CSNamedPipe.exe as the name of the namedPipe.
        os.StartInfo.Arguments = studentUserID;
        os.Start();
        StreamWriter wr = os.StandardInput;
        //os.StandardInput.Close();
        Session["Writer"] = wr;
        Session["Process"] = os;
        





    }
}