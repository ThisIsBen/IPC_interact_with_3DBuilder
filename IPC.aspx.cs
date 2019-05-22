using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Xml;
using System.Data;
using System.Threading;
using System.Data.Entity;








public partial class IPC : CsSessionManager
{
    //private static System.Diagnostics.Process os = new System.Diagnostics.Process();
    // public static System.Diagnostics.Process rd = new System.Diagnostics.Process();
    // private StreamWriter myStreamWriter = null; 


    //In the near future ,we will get the Path from URL para or other para transmission method.
    string XMLFolder = CsDynamicConstants.relativeKneeXMLFolder;
    string absoluteKneeXMLFolder = CsDynamicConstants.absoluteKneeXMLFolder; //"D:\\IPC_interact_with_3DBuilder\\IPC_Questions\\1161-1450\\";//it's only used when we want the 3DBuilder to load the Organ XML

   
    string QuestionFileName = "";//只有檔名，沒有資料夾名稱
    /*
    string _StuCouHWDe_ID = "1381";//We just temporarily hard code it.This ID should be retrieved from a session variable or URL variable.
    string cActivityID = "009";
    */

    //2019/4/8 Ben set the default Value of each URL paras
    /*
    string strUserID = "";
    string questionXMLPath = "";
    string cActivityID = "";
    */

    //set the default value of each parameters that are retrieved from URL.
    string questionXMLPath = ""; //surgery mode xml file name
    string strQID = "tea1_Q_20181210231100";//Surgery Mode
    string strUserID = "stu2";
    string cActivityID = "1023";
    string examMode = "Yes";
   

    protected void Page_Load(object sender, EventArgs e)
    {
        //get the parameters in URL and store there value in global var.
        retrieveURLParameters();


        QuestionFileName = strQID + ".xml";

        questionXMLPath = XMLFolder + QuestionFileName;

        Page.MaintainScrollPositionOnPostBack = true;
        if (!IsPostBack)
        {
            
           


            gvScore.AllowPaging = false;
            gvScore.AllowSorting = true;
            gvScore.AutoGenerateColumns = false;
            gvScore.Style.Add("width", "100%");
            gvScore.PageIndex = 0;

            DataSet ds = new DataSet();
            //ds.ReadXml(Server.MapPath("SceneFile13.xml")); //must synchronized with the XML file in Items.aspx.cs:  wr.WriteLine("3 D:\\Mirac3DBuilder\\HintsAccounts\\Student\\Mirac\\1161-1450\\SceneFile13.xml");//send protocol,Data to 3DBuilder.
            ds.ReadXml(Server.MapPath(questionXMLPath)); //must synchronized with the XML file in Items.aspx.cs:  wr.WriteLine("3 D:\\Mirac3DBuilder\\HintsAccounts\\Student\\Mirac\\1161-1450\\SceneFile13.xml");//send protocol,Data to 3DBuilder.

            //in Items.aspx.cs

            //DataRow dtRow = dtScore.NewRow();
            //dtRow["Seq"] = "1";
            //dtScore.Rows.Add(dtRow);
            gvScore.DataSource = ds.Tables["Organ"];
            //gvScore.DataMember = 
            gvScore.DataBind();
            gvScore.HeaderRow.TableSection = TableRowSection.TableHeader;


            // display organ name or organ number to be the organ indicator based on the value of <NameOrNumberAnsweringMode_Session> in the AITypeQuestion XML file.
            displayOrganNameOrNumber();

            //to contain the content read from organ XML file.
            List<string> CorrectOrganNameList = new List<string>();

            // For each correct organ name in the table, 
            //add it to a list called CorrectOrganNameList .
            foreach (DataRow row in ds.Tables["Organ"].Rows)
            {

                CorrectOrganNameList.Add(row["Name"].ToString());

            }

            //set up the mapping of correct organ name and organ number according to the NameOrNumberAnsweringMode.
            setUpOrganName_NumberMapping(CorrectOrganNameList);

           


            //2019/1/28 Ben comment it because we can't bring 3DBuilder to run in foreground
            /*
            //run the CSNamedPipe.exe to communicate with 3DBuilder.exe
            activate3DBuilder();
           
            //2019/1/28 Ben comment it because we can't bring 3DBuilder to run in foreground
            
             //originating from Item.aspx btnKnee_Click()
            //randomize the Question Organ Number 
            randomizeQuestionOrganNo();
            
             */

            
            

            //2019/4/10 Ben commented the function to show 3D Labels in 3DBuilder when AITypeQuestion is loaded.
            /*
             * 
             //wait for the 3DBuilder to respond before show 3D Labels in 3DBuilder
            //Because it takes longer for the 3DBuilder to load all the organs when 
            //the AITypeQuestion is of Surgery Mode or when there are lots of 3D organ that need to be displayed
            System.Threading.Thread.Sleep(100);
             
            //Show 3D Labels in 3DBuilder
            ShowOrHide3DLabels_Click();
            */

        }

        //set the remaining time to the timer and check whether time is already up.
        setUp_and_CheckCountdownTimer();

       



    }
    //set up the mapping of correct organ name and organ number according to the NameOrNumberAnsweringMode.
    private void setUpOrganName_NumberMapping(List<string> CorrectOrganNameList)
    {
        if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
        {

            //store correct organ list to session
            CorrectOrganNameSession = CorrectOrganNameList;
        }

           //We make use of a dictionary session variable to store the mapping of organ number and the randomized organ name.
        else if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
        {
            //clear the mapping of organ number and the randomized organ name in a dictionary session variable when it's of Number Answering Mode
            NumberAnsweringMode_RandOrganNoNameMapping_Session = null;

            //create the mapping of organ number and the randomized organ name in a dictionary session variable when it's of Number Answering Mode
            for (int i = 0; i < NumberAnsweringMode_WholeRandOrganNo_Session.Length; i++)
            {
                NumberAnsweringMode_RandOrganNoNameMapping_Session.Add(NumberAnsweringMode_WholeRandOrganNo_Session[i], CorrectOrganNameList[i]);

            }


        }

    }


    //get the parameters in URL and store there value in global var.
    private void retrieveURLParameters()
    {
        //set the variable questionXMLPath with the parameter strQID in URL if it is provided.
        if (Request.QueryString["strQID"] != null && Request.QueryString["strQID"] != "")
        {
            strQID = Request.QueryString["strQID"] ;
        }
        

        //set the variable studentUserID with the parameter strUserID in URL if it is provided.
        if (Request.QueryString["strUserID"] != null && Request.QueryString["strUserID"] != "")
        {
            strUserID = Request.QueryString["strUserID"];
        }


        

        //set the variable cActivityID with the parameter cActivityID in URL if it is provided.
        if (Request.QueryString["cActivityID"] != null && Request.QueryString["cActivityID"] != "")
        {
            cActivityID = Request.QueryString["cActivityID"];
        }

        //set the variable examMode with the parameter examMode in URL if it is provided.
        if (Request.QueryString["examMode"] != null && Request.QueryString["examMode"] != "")
        {
            examMode = Request.QueryString["examMode"];
        }

      
    }


    //set the remaining time to the timer and check whether time is already up.
    private void setUp_and_CheckCountdownTimer()
    {
        //2019/4/8 Ben commented for using default URL value if no paras provided.
        /*
        strUserID = Request["strUserID"];
        string questionXMLPath = Request["strQID"];
         * 
         
        cActivityID = Request["cActivityID"];
         * 
        */
        int remainingTimeSec = CsDBOp.getExamRemainingTime(cActivityID); //now I just hard code it to 15 sec


        if (remainingTimeSec < 0 && remainingTimeSec != -1)
        {
            //Response.Write("<script>alert('考試時間已結束')</script>");
            /*
            Response.Write("<script>alert('考試時間已結束');location.href='ALHomePage.aspx?strQID=" + strQID + "&strUserID=" + strUserID + "&cActivityID=" + cActivityID + "'</script>");
            */
            //remind the student that the exam is over, and redirect back to Paper_DisplayForORCS.aspx to view the exam paper. 
            Response.Write("<script>alert('The exam time is already over.');location.href='" + Previous_Page_URL_Session + "'</script>");
            
        }

        else if (remainingTimeSec == -1)
        {
            Response.Write("<script>alert('找不到資料');location.href='ALHomePage.aspx?strQID=" + strQID + "&strUserID=" + strUserID + "&cActivityID=" + cActivityID + "' </script>");
        }
        else
        { 
            
        Double examTimespan = remainingTimeSec * 1.0;
        DateTime deadlineDateTime = DateTime.Now.AddSeconds(examTimespan); // return Datetime format
        /*psedo code:
         * store deadlineDateTime to DB
         * */
        /*This block should be implemented in the Hints set timer for the exam page
        * */



        /*psedo code
         * retrieve deadlineDateTime from DB,which is stored in the datetime format
         * */
        //check count down timer to decide whether time is already up.
        CheckCountdownTimer(deadlineDateTime);
    }
}

    private void activate3DBuilder()
    {
        //run CSNamedPipe.exe
        runCSNamedPipe();



        //run 3DBuilder.exe
        run3DBuilder();

        /*Ben test
       //originating from ALHomePage.aspx
       StreamWriter wr = (StreamWriter)Session["Writer"];
       //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>alert('" + os.StartInfo.RedirectStandardInput + os.Id + "');</script>"); 
       //StreamWriter wr = os.StandardInput;

       wr.WriteLine("1 2");
        */


    }

    private void runCSNamedPipe()
    {
        //originating from Default.aspx
        Process os = new Process();
        string hintID = "5555";//this hintID is hard-coded by 昇宏學長

        os.StartInfo.WorkingDirectory = Request.MapPath("~/");
        os.StartInfo.FileName = Request.MapPath("App_Code/CSNamedPipe/bin/Debug/CSNamedPipe.exe");
        os.StartInfo.UseShellExecute = false;
        os.StartInfo.RedirectStandardInput = true;
        /*
        os.StartInfo.Arguments = hintID;
        */
        //pass Hints's userID to CSNamedPipe.exe as the name of the namedPipe.
        os.StartInfo.Arguments = strUserID;

        os.Start();
        StreamWriter wr = os.StandardInput;
        //os.StandardInput.Close();
        Session["Writer"] = wr;
        Session["Process"] = os;


        //get process ID of the CSNamedPipe, and store it in a session var so that we can kill the CSNamedPipe process after the user finishes using the connection with 3DBuilder
        Session["ProcessID"] = os.Id.ToString();
        



    }

    private void run3DBuilder()
    {


        //this works but 3DBuilder will be run in background

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.Verb = "runas";
        startInfo.WorkingDirectory = Request.MapPath("~/");
        /*
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.CreateNoWindow = true;
        */
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = true;

        startInfo.FileName = Request.MapPath("3DBuilder.lnk");
        //startInfo.FileName = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\installutil.exe";
        //startInfo.Arguments = "D:\\Projects\\MyNewService\\bin\\Release\\MyNewService.exe";
        process.StartInfo = startInfo;
        bool processStarted = process.Start();





    }

    //originating from Item.aspx btnKnee_Click()
    private void randomizeQuestionOrganNo()
    {

        //These 2 variables will set by URL parameter or data retrieve from DB in the near future.
        ///////////////////////////////////////////////////////////////////////////////

        bool ExamMode = false;//ExamMode的中控

        //2019/4/9 Ben commented for using default URL value if no paras provided.
        //if (Request["examMode"] == "Yes")
        if (examMode == "Yes")
        {
            ExamMode = true;//ExamMode的中控
        }
        string ID_Num = "234";//The last 3 digits of student's ID will be retrieved from DB in the near future. 
        //int[] pickedQuestions = { 1, 3, 5 }; //The Question Number of organs  picked by instructor will be retrieved from DB in the near future. 


        //////////////////////////////////////////////////////////////////////////

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
            intArray2AString(randomQuestionNo, ref strRandomQuestionNo);

            /*
            //use JS alert() in C#
            ScriptManager.RegisterStartupScript(this,
             typeof(Page),
             "Alert",
             "<script>alert('" + "3 " + XMLFolder + questionXMLPath +" "+ strRandomQuestionNo + "');</script>",
             false);
            */



            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine("3 " + absoluteKneeXMLFolder + QuestionFileName + "_" + strRandomQuestionNo);//send protocol,Data to 3DBuilder.

            //!!!!//如何將上方兩個參數傳送到3DBuilder那裏的IPCInterface呢? 上行的WriteLine會寫到哪裡呢?

            //head to IPC.aspx
            //Response.Redirect("IPC.aspx");

            ////head to IPC.aspx in exam mode
            //Response.Redirect("IPC.aspx?examMode=Yes");


        }

        else//if it's not in exam mode
        {


            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine("3 " + absoluteKneeXMLFolder + QuestionFileName);//send protocol,Data to 3DBuilder.

            //!!!!//如何將上方兩個參數傳送到3DBuilder那裏的IPCInterface呢? 上行的WriteLine會寫到哪裡呢?

            //head to IPC.aspx
            //Response.Redirect("IPC.aspx");

            ////head to IPC.aspx in exam mode
            //Response.Redirect("IPC.aspx");
        }


    }

    private void intArray2AString(int[] randomQuestionNo, ref string strRandomQuestionNo)
    {
        string[] result = randomQuestionNo.Select(x => x.ToString()).ToArray();

        for (int i = 0; i < result.Length; i++)
        {
            strRandomQuestionNo += result[i];
            strRandomQuestionNo += " ";

        }




    }


    //get the picked Question number from Question XML file
    private int[] getPickedQuestionNumber()
    {
        ////Get The Question Number of organs  picked by instructor////////// 
        //read in the XML files that contains all organs of a certain body part. e.g., Knee 
        XMLHandler xmlHandler = new XMLHandler(Server.MapPath(questionXMLPath));

      
        //get the number of the Organs whose Question tag are marked "Yes".
        return xmlHandler.getPickedQuestionNumber();

    }

    private void CheckCountdownTimer(DateTime deadlineDateTime)
    {

        int serverSideRemainingTimeSec = Convert.ToInt32((deadlineDateTime - DateTime.Now).TotalSeconds);

        //if time is already up, force submit the AI type exam paper. 
        if (serverSideRemainingTimeSec <= 0)
        {
            //call the Finish button event handler to force submit the AI type exam paper.
            FinishBtn_ClickEventHandler();
        }
        else //let the front end JS count down timer to count down.
        {
            //pass the server Side Remaining Time (Sec) to front end through hidden field
            hidden_serverSideRemainingTimeSec.Value = serverSideRemainingTimeSec.ToString();


        }

    }


    //send message through CSNamedPipe.exe to the corresponding 3DBuilder.
    public void sendMsg23DBuilder(string contact)
    {
        /*

        //send cmd1
        try
        {
            using (StreamWriter wr = (StreamWriter)Session["Writer"])
            {
                //send cmd2
                wr.WriteLine(contact);//!!!!!send update msg to 3DBuilder
            }// the streamwriter WILL be closed and flushed here, even if an exception is thrown.
           
            //wr.Flush();
        }
        catch(Exception e)
        {

        }
      */


        //send cmd1
        try
        {

            StreamWriter wr = (StreamWriter)Session["Writer"];
            //StreamWriter wr = new StreamWriter((StreamWriter)Session["Writer"]);
            //StreamWriter wr = new StreamWriter((Stream )Session["Writer"], Encoding.UTF8, 4096, true);
            //send cmd2
            wr.WriteLine(contact);//!!!!!send update msg to 3DBuilder

            // the streamwriter WILL be closed and flushed here, even if an exception is thrown.

            //wr.Flush();
        }
        catch (Exception e)
        {

        }

    }

    protected void StartIPC_Click(object sender, EventArgs e)
    {


    }
    protected void StartRemoteApp_ServerClick(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        /*String str = TBX_Input.Value.ToString();
        //string str = CreateRandomCode(10);
        StreamWriter wr = os.StandardInput;
        wr.WriteLine(str);
        TBX_Input.Value = "";*/

        //StreamWriter wr = os.StandardInput;
        //wr.WriteLine("1 2");
        //wr.WriteLine("3 0");

    }
    /*
    public void InsertStuIPCAns2DB(string strUserID, string questionXMLPath, string _QuesOrdering, string _StudentAnswer, int Num_Of_Question_Submision_Session)
    {
        CsDBOp.InsertStuIPCAns(strUserID, questionXMLPath, _QuesOrdering, _StudentAnswer, Num_Of_Question_Submision_Session);
    }
    */

    public void FinishBtn_Click(object sender, EventArgs e)
    {
        //do what should be done when the user clicks submit button
        FinishBtn_ClickEventHandler();

        //kill the corresponding running CsNamedPipe.exe process which is created when the teacher clicks "connect to 3DBuilder" to edit the AITypeQuestion in 3DBuilder.
        killCorrespondingCSNamedPipe();
           

    }

    private void killCorrespondingCSNamedPipe()
    {
        /*
        //kill the corresponding running CsNamedPipe.exe process which is created when the teacher clicks "connect to 3DBuilder" to edit the AITypeQuestion in 3DBuilder.
        Process os = (Process)Session["Process"];

        //kill the corresponding running CsNamedPipe.exe process if it exists.
        if (os != null)
        {
            os.Kill();
        }
         * */

        //kill the corresponding running CsNamedPipe.exe process which is created when the teacher clicks "connect to 3DBuilder" to edit the AITypeQuestion in 3DBuilder.
        //kill process with processID
        Process[] procList = Process.GetProcesses();

        for (int i = 0; i < procList.Length; i++)
        {
            string pid = procList[i].Id.ToString();
            if (string.Equals(pid, Session["ProcessID"]))
            {
                procList[i].Kill();
            }
        }
    }


    // display organ name or organ number to be the organ indicator based on the value of <NameOrNumberAnsweringMode_Session> in the AITypeQuestion XML file.
    public void displayOrganNameOrNumber()
    {
      
        XMLHandler xmlHandler = new XMLHandler(Server.MapPath(questionXMLPath));

        //get the "NameOrNumberAnsweringMode_Session" of the  AITypeQuestion from the AITypeQuestion XML file
        //NameOrNumberAnsweringMode_Session = xmlHandler.getValueOfSpecificNonNestedTag("NameOrNumberAnsweringMode");

        if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
        {

            //get the organ name from AITypeQuestion XML file.
            List<string> strAllOrganName = xmlHandler.getValuesOfEachSpecificTag("Name");


            for (int i = 0; i < gvScore.Rows.Count; i++)
            {


                //var organIndicator = gvScore.Rows[i].FindControl("TB_OrganIndicator") as Label;
                var organIndicator = ((Label)gvScore.Rows[i].FindControl("TB_OrganIndicator"));
                organIndicator.Text = strAllOrganName[i];
            }

          
            //Hide the "Show/Hide Icon" column when "Number Answering Mode" is being used.
            //We hide it to avoid cheating by hiding all the organs that are not set to be the question.
            switchOfDisplayShowHideIconCol("off");
            

        }

        else if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
        {
            for (int i = 0; i < gvScore.Rows.Count; i++)
            {


                var organIndicator = gvScore.Rows[i].FindControl("TB_OrganIndicator") as Label;

                organIndicator.Text = (i+1).ToString();
            }
        }

    }
     



    private void FinishBtn_ClickEventHandler()
    {

        //commented it for Peter work on storing student's answer to DB
        //Break 3DBuilder connection 
        /*    
        Process os = (Process)Session["Process"];       
        os.Kill();
        */
        //2019/4/8 Ben commented for using default URL value if no paras provided.
        /*
        //get the strUserID and strQID from the URL parameters
        strUserID = Request["strUserID"];
        string questionXMLPath = Request["strQID"];
        */


        //Begin: The followings are  for temporary use ,and should be removed before push to github

        //get the Question Number of organs picked by instructor sent from the front end (These Question Numbers are passed by using a hidden field).
        string[] str_pickedQuestions = hidden_pickedQuestions.Value.ToString().Split(",".ToCharArray());
        int[] pickedQuestions = Array.ConvertAll<string, int>(str_pickedQuestions, int.Parse);

        string _QuesOrdering = "";

        /*
        int[] pickedQuestions = { 1, 3, 5 }; //The Question Number of organs  picked by instructor
        RandomQuestionNoSession = pickedQuestions;        
       */
        //End: The followings are  for temporary use ,and should be removed before push to github
        /*
        String[] values = new String[10];
        values[0] = this.gvScore.Rows[0].Cells[0].Text.ToString();
        values[1] = this.gvScore.Rows[0].Cells[1].Text.ToString();
        values[2] = this.gvScore.Rows[0].Cells[2].Text.ToString();
        values[3] = this.gvScore.Rows[0].Cells[3].Text.ToString();
        values[4] = this.gvScore.Rows[0].Cells[4].Text.ToString();
        values[5] = this.gvScore.Rows[1].Cells[0].Text.ToString();
        values[6] = this.gvScore.Rows[1].Cells[1].Text.ToString();
        values[7] = this.gvScore.Rows[1].Cells[2].Text.ToString();
        values[8] = this.gvScore.Rows[1].Cells[3].Text.ToString();
        values[9] = this.gvScore.Rows[1].Cells[4].Text.ToString();
      
        Session["values"] = values;
        */
        //store student's answer to DB
        if (examMode == "Yes")
        {


            StuAnsM StudentAnswer = new StuAnsM();
            int RandomQuestionNum;//將題號一個一個取出來
            string strTB1="";//暫儲存學生每格答案
            /*
            List<string> StudentAnswerList = new List<string>();//to store student's answer.
            */

            //trim space of QuestionFileName e.g., "   SceneFile_Q12.xml   "=>"SceneFile_Q12.xml"
            QuestionFileName = QuestionFileName.Trim();
            StudentAnswer._QuesOrdering += XMLFolder + QuestionFileName + ",";
            StudentAnswer._StudentAnswer += XMLFolder + QuestionFileName + ",";

            for (int i = 0; i < RandomQuestionNoSession.Length; i++)
            {

                RandomQuestionNum = RandomQuestionNoSession[i];

                if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
                {
                    TextBox tb = (TextBox)gvScore.Rows[RandomQuestionNum - 1].FindControl("TB_AnsweringField");
                    strTB1 = tb.Text.Trim();
                }
                else if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
                {
                    TextBox tb = (TextBox)gvScore.Rows[i].FindControl("TB_AnsweringField");
                    strTB1 = tb.Text.Trim();
                }
                
                if (i == (RandomQuestionNoSession.Length - 1))
                {
                    _QuesOrdering = RandomQuestionNum.ToString();
                    StudentAnswer._StudentAnswer += strTB1;
                }
                else
                {
                    _QuesOrdering = RandomQuestionNum + ",";
                    StudentAnswer._StudentAnswer += strTB1 + ",";
                }
                StudentAnswer._QuesOrdering += _QuesOrdering;
            }
            StudentAnswer._QuesOrdering += ":";
            StudentAnswer._StudentAnswer += ":";

             /*測試將學生答案寫入資料庫用*/
             //reset session variable Num_Of_Question_Submision_Session to 1
             //because currently we only allow one AITypeQuestion to contains only one body part
            /* 因要測試將學生答案寫入資料庫，因此暫時使用此行來reset session var Num_Of_Question_Submision_Session*/
            Num_Of_Question_Submision_Session =0;
            /*測試將學生答案寫入資料庫用*/

            //increase the Num_Of_Question_Submision_Session to indicate that we should update, not insert the student answer to the datatable because the student's record has already existed.            
            Num_Of_Question_Submision_Session++;
             

             
            //foreach (StuAnsM c in StudentAnswerList) //顯示list裡的資料
            //{

            //    Response.Write(c.StudentAnswer + ", " + c.QuesOrdering + " ");
            //}


            //2019/5/8 Ben commented to test Number Answering Mode
            
            //Insert the student's ID, student's answer, question order of this AITypeQuestion to the NewVestionDB/AITypeQuestionStudentAnswer datatable
            CsDBOp.InsertStuIPCAns(strUserID, strQID, StudentAnswer._QuesOrdering, StudentAnswer._StudentAnswer, Num_Of_Question_Submision_Session, cActivityID);//插入學生data至darabase
            



            //InsertStuIPCAns2DB(strUserID, questionXMLPath, StudentAnswer._QuesOrdering, StudentAnswer._StudentAnswer, Num_Of_Question_Submision_Session);//插入學生data至darabase
            ///////////////////////////////////////
            //DataTable dt = CsDBOp.GetStuIPCAns();
            //StuAnsM Stu_correct_papers = new StuAnsM();
            ////get the retrieved data from each row of the retrieved data table.
            //foreach (DataRow dr in dt.Rows)
            //{
            //    //get the value of field StuCouHWDe_ID from ScoreDetailTB table
            //    Response.Write("<script>console.log(" + dr.Field<string>("StuCouHWDe_ID") + ");</script> ");

            //    //get the value of field Grade from ScoreDetailTB table

            //    IList<string> names = dr.Field<string>("Grade").Split(',').ToList<string>();
            //    //Response.Write("<script>console.log(" + names + ");</script> ");

            //    Response.Write("<script>console.log(" + names[0] + ");</script> ");

            //    //
            //}
        }
        /* foreach (GridViewRow row in gvScore.Rows)
         {
            // TextBox tb = (TextBox)row.FindControl("TextBox_Text");

         }
        */
        ///your code ......
        ///
        //Peter needs to get content of the first RandomQuestionNoSession.Length TH TextBoxes in the GridView called gvScore. 
        //and store them to a List called StudentAnswerList
        //List<string> StudentAnswerList = new List<string>();//to store student's answer.
        //StudentAnswerList.Add(strTB1);

        //RandomQuestionNoSession.Length ==3
        //C# GridView gvScore
        //gvScore.Rows

        /*removes all the objects stored in a Session. 
         If you do not call the Abandon method explicitly, the server removes these objects and destroys the session when the session times out.
         It also raises events like Session_End.*/
        Session.Abandon();


        //redirect back to the exam paper
        redirectBack2ExamPaper();
       
    }

    private void redirectBack2ExamPaper()
    {
        //redirect back to the exam paper
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "history.go(-3);", true);



    }

    //hide or show Show/Hide icon column
    public void switchOfDisplayShowHideIconCol(string onOrOff)
    {
        if (onOrOff == "on")
        {
            

            /*this part still doesn't work, we can't display the hidden Show/Hide icon column*/
            //display all the “Show/Hide” icon for each organ on the webpage to allow the student to hide or show the 3D organs displayed in the 3DBuilder.
            gvScore.HeaderRow.Cells[3].Visible = true;


            for (int i = 0; i < gvScore.Rows.Count; i++)
            {
                //gvScore.Rows[i].Cells[3].Visible = true;
                gvScore.Rows[i].Cells[3].Attributes.Add("style", "display:block");
            }

        }



        if (onOrOff == "off")
        {
           

            /*this part still doesn't work, we can't display the hidden Show/Hide icon column*/
            //display all the “Show/Hide” icon for each organ on the webpage to allow the student to hide or show the 3D organs displayed in the 3DBuilder.
            gvScore.HeaderRow.Cells[3].Visible = false;


            for (int i = 0; i < gvScore.Rows.Count; i++)
            {
                //gvScore.Rows[i].Cells[3].Visible = false;
                gvScore.Rows[i].Cells[3].Attributes.Add("style", "display:none");
            }
        }


    }




    //switch visibility icon for All rows or a specific row
    public String switchVisible_Invisible(GridViewRow selectedRow, string HFID, GridView gvScore)
    {
        bool BooLInOrVisible = false;

        if (selectedRow != null)//hide or show the selected organ.
        {
            var HFInOrVisible = selectedRow.FindControl(HFID) as HiddenField;
            BooLInOrVisible = Convert.ToBoolean(HFInOrVisible.Value);
        }


        String HideOrShow = "";
        if (BooLInOrVisible)//if current state is visible
        {
            //means its visible before being clicked
            if (selectedRow != null)
            {
                (selectedRow.FindControl(HFID) as HiddenField).Value = "false";//switch to invisible
                HideOrShow = "hide";
            }

        }
        else//if current state is invisible
        {
            if (selectedRow != null)
            {
                (selectedRow.FindControl(HFID) as HiddenField).Value = "true";//switch to visible
                HideOrShow = "show";
            }

            else
            {
                foreach (GridViewRow row in gvScore.Rows)
                {
                    //When the student clicks the “ShowAll” button, 
                    //the organs hidden by the teacher should not be shown.
                    if ((row.FindControl(HFID) as HiddenField).Value != "disableByTeacher")
                    {
                        //only show the organs that are hidden by the student.
                        (row.FindControl(HFID) as HiddenField).Value = "true";//switch to visible

                    }
                }
                HideOrShow = "showAll";
            }

        }
        return HideOrShow;
    }


    protected void ShowOrHideAll_Click(object sender, EventArgs e)
    {



        //switch visibility icon All rows .
        String HideOrShow = switchVisible_Invisible(null, "InOrVisible", gvScore);
        string contact = "7 " + HideOrShow; //send "6 Hide realOrganName" to 3DBuilder  


        /*
         string a = "abcdd";
        
         //use JS alert() in C#
         ScriptManager.RegisterStartupScript(
          this,
          typeof(Page),
          "Alert",
          "<script>alert('" + a + "');</script>",
          false);
         /////
        */

        //send hide 3D organ msg to 3DBuilder
        //string contact = "7 " + HideOrShow;//send "7 Hide realOrganName" to 3DBuilder
        //string contact = "6 " + HideOrShow ;



        //string contact = "6 hide Left Popliteal Vein";

        sendMsg23DBuilder(contact);

        // Thread.Sleep(1000); //Delay 1秒




    }


    //protected void ShowOrHideAll_Click(object sender, EventArgs e)
    protected void ShowOrHide3DLabels_Click()
    {



        //switch visibility icon All rows .
        //String hideOrShow3DLabels = switchVisible_Invisible(null, "InOrVisible", gvScore);
        string contact = "8 " + "Show_3D_Labels"; //send "6 Hide realOrganName" to 3DBuilder  

        //Hide_3D_Labels

        /*
         string a = "abcdd";
        
         //use JS alert() in C#
         ScriptManager.RegisterStartupScript(
          this,
          typeof(Page),
          "Alert",
          "<script>alert('" + a + "');</script>",
          false);
         /////
        */

        //send hide 3D organ msg to 3DBuilder
        //string contact = "7 " + HideOrShow;//send "7 Hide realOrganName" to 3DBuilder
        //string contact = "6 " + HideOrShow ;



        //string contact = "6 hide Left Popliteal Vein";

        sendMsg23DBuilder(contact);

        // Thread.Sleep(1000); //Delay 1秒




    }


    //submit current edition to 3DBuilder
    protected void gvScore_RowCommand(Object sender, GridViewCommandEventArgs e)
    {

        // If multiple ButtonField column fields are used, use the
        // CommandName property to determine which button was clicked.
        if (e.CommandName == "Submit")
        {
                //store the msg that will be sent to the 3DBuilder.
                string msgFor3DBuilder = "";

                //if the AITypeQuestion is set to be of "Number Answering Mode"
                if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
                {
                    // Convert the row index stored in the CommandArgument
                    // property to an Integer.
                    int index = Convert.ToInt32(e.CommandArgument);
                    // Get the last name of the selected author from the appropriate
                    // cell in the GridView control.
                    GridViewRow selectedRow = gvScore.Rows[index];
                    var answeringField = selectedRow.FindControl("TB_AnsweringField") as TextBox;
                    var organIndicator = selectedRow.FindControl("TB_OrganIndicator") as Label;

                    string givenOrganName = organIndicator.Text;
                    string studentOrganNumberAnswer = answeringField.Text;
                    //If the student hasn’t answered the organ number, we will prompt the student to answer it before clicking the “Show/Hide” icon of the question organ.
                    if (studentOrganNumberAnswer == "")
                    {

                   
                        //If we use Response.Write to insert JS code, 
                        //JS Bundles/MsAjax error: PRM_ParserErrorDetails will occur on the frontend.
                        /*
                        Response.Write("<script>alert('Please enter the answer before clicking the 'Show/Hide' icon.');</script>");
                        */


                        //We use ScriptManager to inset JS code can 
                        //avoid the JS Bundles/MsAjax error: PRM_ParserErrorDetails 
                        ScriptManager.RegisterStartupScript(this,
                         typeof(Page),
                         "Alert",
                         "<script>alert('Please enter the answer before clicking the \"Submit\" icon.');</script>",
                         false);


                       
                        //no need to do the rest of the work of the function if the student click the "Show/Hide" icon before answering the organ number
                        return;
                    }


                    //if what the student entered is not numeric, we show the warning and return at once.
                    else if (!checkStringIsNumeric(studentOrganNumberAnswer) )
                    {
                        ScriptManager.RegisterStartupScript(this,
                         typeof(Page),
                         "Alert",
                         "<script>alert('What you entered is not numeric. Please enter it properly again.');</script>",
                         false);

                        return;
                    }

                    //Here the first para is the same as the third para.
                    string correctOrganName = givenOrganName;


                    //Replace the " " space in student's answer with "_" so that the 3DBuilder can extract and display the student's answer correctly
                    givenOrganName = givenOrganName.Replace(" ", "_");

                    //Update student answer to the 3D label in the 3DBuilder
                    msgFor3DBuilder = "5 " + givenOrganName + " " + studentOrganNumberAnswer + " " + correctOrganName;
         

                }

                //if the AITypeQuestion is set to be of "Name Answering Mode"
                else if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
                {
                    //generated random question number with Peter's function
                    int[] randQuestionNoList = RandomQuestionNoSession;

                    // Convert the row index stored in the CommandArgument
                    // property to an Integer.
                    int index = Convert.ToInt32(e.CommandArgument);

                    // Get the last name of the selected author from the appropriate
                    // cell in the GridView control.
                    GridViewRow selectedRow = gvScore.Rows[index];
                    var answeringField = selectedRow.FindControl("TB_AnsweringField") as TextBox;
                    var organIndicator = selectedRow.FindControl("TB_OrganIndicator") as Label;
                    /////////////////////////////////////////////
                    //text exam mode
                    //store the Question number
                    string QuestionNo;

                    //if it's in the exam mode

                    //2019/4/9 Ben commented for using default URL value if no paras provided.
                    //if (Request["examMode"] == "Yes")
                    if (examMode == "Yes")
                    {

                        QuestionNo = (Array.IndexOf(randQuestionNoList, Int32.Parse(organIndicator.Text)) + 1).ToString();
                        // QuestionNo = index.ToString();

                    }

                    //if it's not in the exam mode
                    else
                    {
                        QuestionNo = organIndicator.Text;
                    }
                    /////////////////////////////


                    //get the corresponding correct organ name 
                    // var answer = CorrectOrganNameSession[Convert.ToInt32(QuestionNo) - 1];

                    var correctOrganName = CorrectOrganNameSession[Convert.ToInt32(QuestionNo)];

                    //Replace the " " space in student's answer with "_" so that the 3DBuilder can extract and display the student's answer correctly
                    string studentOrganNameAnswer = answeringField.Text.Replace(" ", "_");
            
                    //Update student answer to the 3D label in the 3DBuilder
                    msgFor3DBuilder = "5 " + studentOrganNameAnswer + " " + QuestionNo + " " + correctOrganName.ToString();
                    //string contact = "5 " + input + " "  + answer.Value.ToString();
            
            }

            //send the message to the 3DBuilder.
            sendMsg23DBuilder(msgFor3DBuilder);

        }




        //For invisible
        //// If multiple ButtonField column fields are used, use the
        // CommandName property to determine which invisible button was clicked.
        if (e.CommandName == "InvisibleAndVisible")
        {

            // Convert the row index stored in the CommandArgument
            // property to an Integer.
            int index = Convert.ToInt32(e.CommandArgument);




            // Get the last name of the selected author from the appropriate
            // cell in the GridView control.
            GridViewRow selectedRow = gvScore.Rows[index];

            //to store the correct organ name
            var correctOrganName="";

            if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
            {

                var num = selectedRow.FindControl("TB_OrganIndicator") as Label; //Index of the selected 3D object

                //get the corresponding correct organ name 
                correctOrganName = CorrectOrganNameSession[Convert.ToInt32(num.Text) - 1];//The correct name of selected 3D object 
            }

            
            //Step 5-1 When the student clicks the “Show/Hide” icon of a question organ, we will hide the corresponding organ according to the number entered by the student.
            //If the student hasn’t answered the organ number, we will prompt the student to answer it before clicking the “Show/Hide” icon of the question organ.
            else if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
            {
                TextBox tb = (TextBox)gvScore.Rows[index].FindControl("TB_AnsweringField");
                string TB_AnsweringField_Content = tb.Text.Trim();

                //do sanity check of student's input.              
                //If the student hasn’t answered the organ number, we will prompt the student to answer it before clicking the “Show/Hide” icon of the question organ.
                if (TB_AnsweringField_Content == "")
                {
                    //If we use Response.Write to insert JS code, 
                    //JS Bundles/MsAjax error: PRM_ParserErrorDetails will occur on the frontend.
                    /*
                    Response.Write("<script>alert('Please enter the answer before clicking the 'Show/Hide' icon.');</script>");
                    */

                    //We use ScriptManager to inset JS code can 
                    //avoid the JS Bundles/MsAjax error: PRM_ParserErrorDetails 
                    ScriptManager.RegisterStartupScript(this,
                     typeof(Page),
                     "Alert",
                     "<script>alert('Please enter the organ number before clicking the \"Show/Hide\" icon.');</script>",
                     false);

                    //no need to do the rest of the work of the function if the student click the "Show/Hide" icon before answering the organ number
                    return;
                }
                
                //if what the student entered is not numeric, we show the warning and return at once.
                else if (!checkStringIsNumeric(TB_AnsweringField_Content) && TB_AnsweringField_Content != "Non Answer Row")
                {
                    ScriptManager.RegisterStartupScript(this,
                     typeof(Page),
                     "Alert",
                     "<script>alert('What you entered is not numeric. Please enter it properly again.');</script>",
                     false);

                    return;
                }

                



                else{
                


                        //if the student wants to hide a 'Non Answer Row' organ,
                        //we hide the correponding 3D organ in the 3DBuilder by getting its organ name from TB_OrganIndicator of that row.
                        if (TB_AnsweringField_Content=="Non Answer Row")
                        {
                            var organIndicator = selectedRow.FindControl("TB_OrganIndicator") as Label;

                            correctOrganName = organIndicator.Text;
                        }

                        //look up the corresponding organ name of the organ number entered by the student.    
                        else
                        {
                            int studentNumberAnswer = Int32.Parse(TB_AnsweringField_Content);
                            correctOrganName = NumberAnsweringMode_RandOrganNoNameMapping_Session[studentNumberAnswer].ToString();


                        }

                   }

                
            }


            //switch visibility icon of the selected row .
            String HideOrShow = switchVisible_Invisible(selectedRow, "InOrVisible", null);

            /*
             //use JS alert() in C#
             ScriptManager.RegisterStartupScript(this,
              typeof(Page),
              "Alert",
              "<script>alert('" + (selectedRow.Controls[3]).ImageUrl + "');</script>",
              false);
             
             //send hide 3D organ msg to 3DBuilder
             */

            string contact = "6 " + HideOrShow + " " + correctOrganName.ToString(); //send "6 Hide realOrganName" to 3DBuilder

            sendMsg23DBuilder(contact);



        }
        if (e.CommandName == "relay")
        {
            //switch visibility icon All rows .
            String HideOrShow = switchVisible_Invisible(null, "InOrVisible", gvScore);
            string contact = "7 " + HideOrShow; //send "6 Hide realOrganName" to 3DBuilder  



            sendMsg23DBuilder(contact);

        }

    }

    //check if a string is numeric
    private bool checkStringIsNumeric(string str)
    {
        int n;
        bool isNumeric = int.TryParse(str, out n);
        return isNumeric;
    }



    /// <summary>產生亂數字串</summary>
    /// <param name="Number">字元數</param>
    /// <returns></returns>
    public string CreateRandomCode(int Number)
    {
        string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
        string[] allCharArray = allChar.Split(',');
        string randomCode = "";
        int temp = -1;

        Random rand = new Random();
        for (int i = 0; i < Number; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            }
            int t = rand.Next(36);
            if (temp != -1 && temp == t)
            {
                return CreateRandomCode(Number);
            }
            temp = t;
            randomCode += allCharArray[t];
        }
        return randomCode;
    }

}
