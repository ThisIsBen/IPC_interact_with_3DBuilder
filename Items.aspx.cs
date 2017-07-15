using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


public partial class Items : Page
{

   
    //In the near future ,we will get the Path from URL para or other para transmission method.
    string XMLFolder = "D:\\Mirac3DBuilder\\HintsAccounts\\Student\\Mirac\\1161-1450\\";
    string questionXMLPath = "SceneFile_Q1.xml";


    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void  intArray2AString(int[] randomQuestionNo,ref string strRandomQuestionNo)
    {
        string[] result = randomQuestionNo.Select(x => x.ToString()).ToArray();
       
        for (int i=0; i < result.Length; i++)
        {
            strRandomQuestionNo += result[i];
            strRandomQuestionNo += " ";

        }

       


    }


    protected void btnKnee_Click(object sender, EventArgs e)
    {

        //These variables will set by URL parameter or data retrieve from DB in the near future.
        bool ExamMode = false;//ExamMode的中控
        //Randomize the organ number picked by instructor 
        int[] pickedQuestions = { 1, 3, 5 }; //The Question Number of organs  picked by instructor will be retrieved from DB in the near future. 
        string ID_Num="234";//The last 3 digits of student's ID will be retrieved from DB in the near future.  








        //if it's in exam mode.
        if (ExamMode)
        {
           
            int[] randomQuestionNo = RandomQuestionNo.rand(ID_Num, pickedQuestions);


            //send randomized  Question Numbers picked by instructor to IPC.aspx through Session
            Session["randomQuestionNo"] = randomQuestionNo;



            //send randomized  Question Numbers picked by instructor to 3DBuilder .
            string strRandomQuestionNo = "";
            intArray2AString(randomQuestionNo,ref strRandomQuestionNo);

            /*
            //use JS alert() in C#
            ScriptManager.RegisterStartupScript(this,
             typeof(Page),
             "Alert",
             "<script>alert('" + "3 " + XMLFolder + questionXMLPath +" "+ strRandomQuestionNo + "');</script>",
             false);
            */


            
            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine( "3 " + XMLFolder + questionXMLPath +" "+ strRandomQuestionNo);//send protocol,Data to 3DBuilder.

            //!!!!//如何將上方兩個參數傳送到3DBuilder那裏的IPCInterface呢? 上行的WriteLine會寫到哪裡呢?

            //head to IPC.aspx
            //Response.Redirect("IPC.aspx");

            ////head to IPC.aspx in exam mode
            Response.Redirect("IPC.aspx?examMode=Yes");
             

        }

        else//if it's not in exam mode
        {


            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine("3 " + XMLFolder + questionXMLPath);//send protocol,Data to 3DBuilder.

            //!!!!//如何將上方兩個參數傳送到3DBuilder那裏的IPCInterface呢? 上行的WriteLine會寫到哪裡呢?

            //head to IPC.aspx
            //Response.Redirect("IPC.aspx");

            ////head to IPC.aspx in exam mode
            Response.Redirect("IPC.aspx");
        }

       
    }
}