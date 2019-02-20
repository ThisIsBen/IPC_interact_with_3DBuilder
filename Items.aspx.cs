using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


public partial class Items : CsSessionManager
{
   
    //In the near future ,we will get the IPC_Question_OriginXMLPath from URL para or other para transmission method.
    //this XML file contains all organs of a certain body part.
    string IPC_QuestionXMLFolder = CsDynamicConstants.relativeKneeXMLFolder;
    //In the near future ,we will get the Path from URL para or other para transmission method.
    //string XMLFolder = "D:\\Mirac3DBuilder\\HintsAccounts\\Student\\Mirac\\1161-1450\\";
    string absoluteKneeXMLFolder = CsDynamicConstants.absoluteKneeXMLFolder;

    //Ben temp hard-code variables:
    //string questionXMLPath = "SceneFile_Q1";
    string questionXMLPath = "tea1_Q_20181225162522";//AITypeQuestion in Anatomy mode xml file name
    //string questionXMLPath = "tea1_Q_20181225165447";//AITypeQuestion in Surgery mode xml file name with 3 question organs
    //string questionXMLPath = "tea1_Q_20190205145709";//AITypeQuestion in Surgery mode xml file name with 4 question organs

    //string questionXMLPath = "tea1_Q_20181210231100"; //surgery mode xml file name
    string studentUserID="stu2";
    //end Ben temp hard-code variables



    //These 2 variables will set by URL parameter or data retrieve from DB in the near future.
    ///////////////////////////////////////////////////////////////////////////////
    bool ExamMode = true;//ExamMode的中控
    string ID_Num = "234";//The last 3 digits of student's ID will be retrieved from DB in the near future. 
    //int[] pickedQuestions = { 1, 3, 5 }; //The Question Number of organs  picked by instructor will be retrieved from DB in the near future. 


    //////////////////////////////////////////////////////////////////////////


    protected void Page_Load(object sender, EventArgs e)
    {

        //set the variable questionXMLPath with the parameter strQID in URL if it is provided.
        if (Request.QueryString["strQID"] != null && Request.QueryString["strQID"] != "")
        {
            questionXMLPath = Request.QueryString["strQID"];
        }

        //set the variable studentUserID with the parameter cUserID in URL if it is provided.
        if (Request.QueryString["cUserID"] != null && Request.QueryString["cUserID"] != "")
        {
            studentUserID = Request.QueryString["cUserID"];
        }


        //set the variable ExamMode with the parameter ExamMode in URL if it is provided.
        if (Request.QueryString["ExamMode"] != null && Request.QueryString["ExamMode"] != "")
        {
            ExamMode =Convert.ToBoolean( Request.QueryString["ExamMode"] );
        }

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

    //get the picked Question number from Question XML file
    public int[] getPickedQuestionNumber()
    {
        ////Get The Question Number of organs  picked by instructor////////// 
        //read in the XML files that contains all organs of a certain body part. e.g., Knee 
        XMLHandler xmlHandler = new XMLHandler(Server.MapPath(IPC_QuestionXMLFolder + questionXMLPath + ".xml"));

        

        //get the number of the Organs whose Question tag are marked "Yes".
        return xmlHandler.getPickedQuestionNumber();
       
    }



    protected void btnKnee_Click(object sender, EventArgs e)
    {

        

        //Get The Question Number of organs  picked by instructor ( from Question XML file)
        int[] pickedQuestions = getPickedQuestionNumber();
     
       
       

       






        //if it's in exam mode.
        if (ExamMode)
        {
            //randomize the  Question Numbers picked by instructor using student's ID as seed.
            int[] randomQuestionNo = RandomQuestionNo.rand(ID_Num, pickedQuestions);


            //send randomized  Question Numbers picked by instructor to IPC.aspx through Session
            RandomQuestionNoSession = randomQuestionNo;



            //send randomized  Question Numbers picked by instructor to 3DBuilder .
            string strRandomQuestionNo = "";
            intArray2AString(randomQuestionNo,ref strRandomQuestionNo);

            /*
            //use JS alert() in C#
            ScriptManager.RegisterStartupScript(this,
             typeof(Page),
             "Alert",
             "<script>alert('" + "3 " + XMLFolder + questionXMLPath+ ".xml" +" "+ strRandomQuestionNo + "');</script>",
             false);
            */


            
            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine( "3 " + absoluteKneeXMLFolder + questionXMLPath + ".xml"+" "+ strRandomQuestionNo);//send protocol,Data to 3DBuilder.
            
            //!!!!//如何將上方兩個參數傳送到3DBuilder那裏的IPCInterface呢? 上行的WriteLine會寫到哪裡呢?

            //head to IPC.aspx
            //Response.Redirect("IPC.aspx");

            ////head to IPC.aspx in exam mode
            Response.Redirect("IPC.aspx?examMode=Yes&strQID=" + questionXMLPath + "&cUserID="+studentUserID);
             

        }

        else//if it's not in exam mode
        {


            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine("3 " + absoluteKneeXMLFolder + questionXMLPath);//send protocol,Data to 3DBuilder.

            //!!!!//如何將上方兩個參數傳送到3DBuilder那裏的IPCInterface呢? 上行的WriteLine會寫到哪裡呢?

            //head to IPC.aspx
            //Response.Redirect("IPC.aspx");

            ////head to IPC.aspx in exam mode
            Response.Redirect("IPC.aspx&strQID=" + questionXMLPath + "&cUserID="+studentUserID);
        }

       
    }
}