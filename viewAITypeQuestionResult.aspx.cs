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
using System.Collections;
using System.Web.Script.Serialization;








public partial class IPC : CsSessionManager
{
    //private static System.Diagnostics.Process os = new System.Diagnostics.Process();
    // public static System.Diagnostics.Process rd = new System.Diagnostics.Process();
    // private StreamWriter myStreamWriter = null; 


    //In the near future ,we will get the Path from URL para or other para transmission method.
    string XMLFolder = CsDynamicConstants.relativeKneeXMLFolder;
    string absoluteKneeXMLFolder = CsDynamicConstants.absoluteKneeXMLFolder; //"D:\\IPC_interact_with_3DBuilder\\IPC_Questions\\1161-1450\\";//it's only used when we want the 3DBuilder to load the Organ XML

   
    string QuestionFileName = "";//只有檔名與.xml，沒有資料夾名稱
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
    string strQID = "tea1_Q_20190205145709";//Anatomy Mode xml file name
    //string strQID = "tea1_Q_20181210231100";//Surgery Mode xml file name
    string strUserID = "stu5";
    string cActivityID = "1023";//1012
    string cPaperID = "tea120181126201801";// it contains cQiD=tea1_Q_20190205145709,cQID=tea1_Q_20181210231100
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
            //store correct organ list to session
            CorrectOrganNameSession = CorrectOrganNameList;


            //2019/1/28 Ben comment it because we can't bring 3DBuilder to run in foreground
            /*
            //run the CSNamedPipe.exe to communicate with 3DBuilder.exe
            activate3DBuilder();
           
            //2019/1/28 Ben comment it because we can't bring 3DBuilder to run in foreground
            
             //originating from Item.aspx btnKnee_Click()
            //randomize the Question Organ Number 
            randomizeQuestionOrganNo();
            
             */

            
            //wait for the 3DBuilder to respond before show 3D Labels in 3DBuilder
            //Because it takes longer for the 3DBuilder to load all the organs when 
            //the AITypeQuestion is of Surgery Mode or when there are lots of 3D organ that need to be displayed
            System.Threading.Thread.Sleep(100);

            //2019/4/10 Ben commented the function to show 3D Labels in 3DBuilder when AITypeQuestion is loaded.          
            /*
            //Show 3D Labels in 3DBuilder
            ShowOrHide3DLabels_Click();
            */


            //turn off the display of the "Show/Hide" icon column
            //which is used to allow the student to hide or show the 3D organs displayed in the 3DBuilder.
            switchOfDisplayShowHideIconCol("off");



            
           
        }
        //2019/5/6 Ben commented the timer because we don't need timer for viewing the result of the AITypeQuestion.
        //set the remaining time to the timer and check whether time is already up.
        //setUp_and_CheckCountdownTimer();

        //Step 1  Because each student has different order of the organ number, 
        //so we access database to get the student’s order of the organ number 
        //and the student’s answer for the question.
        getAndMarkStudentAnswer();

        //Step 2-1 apply the retrieved student’s question organ number to display the student’s answer in this order.
        applyStudentsQuestionOrganNo();


        //display student's total score of this AITypeQuestion
        LB_StudentScore.Text = "Your score: " + getStudentScoreAndQuestionTotalScore();
            

    }


    //required global variable from displaying the exam result on teacher's page
    /*
     Here on viewAITypeQuestionResult.aspx page, we only calculate and show the current using student's score
    so, we only use ScoreAnalysisList[0] here.
     */
    public List<ScoreAnalysisM> ScoreAnalysisList = new List<ScoreAnalysisM>();
    private List<int[]> QuestionAvg = new List<int[]>();
    private List<string> correctXML = new List<string>();
   
    private List<string> AddRowsName = new List<string>();
    private List<string> AddColsName = new List<string>();
    public string[] QuestionName;
    public List<string[]> MemberQuestionAnswer = new List<string[]>();

    /*Convert the following global variable to session variable*/
    //a correctAnswer/QuestionOrdering Hash table for marking student’s answer
    public Hashtable[] correctAnswerHT;

    

    


    //Step 1  Because each student has different order of the organ number, 
    //so we access database to get the student’s order of the organ number 
    //and the student’s answer for the question.

    private void getAndMarkStudentAnswer()
    {

        // we get the allotment total score of the AITypeQuestion from the exam paper.
        int questionTotalScore = getAllotmentScoreOfAITypeQuestion();

        //In  博宇's implementation
        /*
        string cActivityID_Selector = Request.QueryString["cActivityID"];
         * */
       
        string cQID_Selector = strQID;

        //Step 1-4 
        //Access the field ‘correctAnswer’ in the datatable ‘AITypeQuestionCorrectAnswer’
        //for the correct answer of question with the current cQID, and construct a QuestionOrdering/correctAnswer Hash table for marking student’s answer.

        getCorrectAns_ConstructCorrectAnsHashTable( cQID_Selector);


        
        //In  博宇's implementation
        /*
        dt = CsDBOp.GetAllTBData("StuCouHWDe_IPC", cQID_Selector);
        */
        DataTable dt = CsDBOp.GetAllTBData("AITypeQuestionStudentAnswer", cQID_Selector, strUserID,cActivityID);
        //Get the retrieved data from each row of the retrieved data table.
        
        //count the number of student
        int studentNoCounter = 0;

        //loop through each student that answers this question.
        foreach (DataRow dr in dt.Rows)
        {
            
            



            


            string StudentIDTemp = dr.Field<string>("cUserID");
            string Gradetemp = dr.Field<string>("Grade");

            //若已經批改過這次AI題考試或練習的話，就直接到資料庫抓其成績來顯示即可。
            if (Gradetemp != "" && Gradetemp != null)
            {
                ScoreAnalysisM log_temp = new ScoreAnalysisM(StudentIDTemp, Gradetemp);
                ScoreAnalysisList.Add(log_temp);

            }
            else
            {
                //Step 1-5 Mark student’s answer if it has not been marked
                markStudentAnswer(dr, StudentIDTemp, cQID_Selector, questionTotalScore);
            }

            //Step 1-1 Access the field ‘QuesOrdering’ in the
            //datatable ‘AITypeQuestionStudentAnswer’ for the ordering of 
            //the student’s organ number with the student’s cUserID and the current cQID .


            //Step 1-2 Access the field ‘StudentAnswer’ in the 
            //datatable ‘AITypeQuestionStudentAnswer’ for the student’s answer 
            //with the student’s cUserID and the current cQID .
            getStudentAnswer(dr, studentNoCounter);


            //calculate student's total score
            calStudentTotalScore(studentNoCounter);

            //increase the counter of the number of student
            ++studentNoCounter;

            /*This student's Scores are stored in ScoreAnalysisList[studentNoCounter].Grade[studentNoCounter] */
            /*This student's total score is stored in ScoreAnalysisList[studentNoCounter].studentTotalScore*/
            /*The total score of this AITypeQuestion is stored in  ScoreAnalysisList[studentNoCounter].questionTotalScore*/
            /*This student's Answer  are stored in  ScoreAnalysisList[studentNoCounter].studentAnswerString*/
            /*This student's Answer  are stored in  ScoreAnalysisList[studentNoCounter].questionOrderingString*/
            /*a correctAnswer/QuestionOrdering Hash table for marking student’s answer is stored in correctAnswerHT*/


        }



    }


    private int getAllotmentScoreOfAITypeQuestion()
    {
        DataTable temp = CsDBOp.GetAITypeQuestionScore("Paper_Content", strQID, cPaperID, "9");
        string StrquestionTotalScore = temp.Rows[0][0].ToString();
        return int.Parse(StrquestionTotalScore);
    }



    //Step 1-1 Access the field ‘QuesOrdering’ in the
    //datatable ‘AITypeQuestionStudentAnswer’ for the ordering of 
    //the student’s organ number with the student’s cUserID and the current cQID .


    //Step 1-2 Access the field ‘StudentAnswer’ in the 
    //datatable ‘AITypeQuestionStudentAnswer’ for the student’s answer 
    //with the student’s cUserID and the current cQID .
    private void getStudentAnswer(DataRow dr, int studentNoCounter)
    {
        

        string studentAnswer_ValueStr = dr.Field<string>("StudentAnswer");
        string quesOrdering_ValueStr = dr.Field<string>("QuesOrdering");

        string[] AnswerStr_Question = studentAnswer_ValueStr.Remove(studentAnswer_ValueStr.Length - 1).Split(':');
        string[] QuesOdrStr_Question = quesOrdering_ValueStr.Remove(quesOrdering_ValueStr.Length - 1).Split(':');

        //get the current  student’s answer , and  the current student’s organ number 
        for (int i = 0; i < AnswerStr_Question.Length; i++)
        {
            ScoreAnalysisList[studentNoCounter].studentAnswerString = AnswerStr_Question[i].Split(',');
            ScoreAnalysisList[studentNoCounter].questionOrderingString = QuesOdrStr_Question[i].Split(',');
        }


        
    }

    //Convert C# array to Json for JS  to access it.
    public string convertCsArray2JSONFormat(string[] CsArray)
    {
        return (new JavaScriptSerializer()).Serialize(CsArray);
        //it will serialize the array in the javascript function. 

 
    }



    //Step 1-5 Mark student’s answer if it has not been marked

    private void markStudentAnswer(DataRow dr, string StudentIDTemp, string cQID_Selector, int questionTotalScore)
    {

        string AnsewerTemp = dr.Field<string>("StudentAnswer");
        string QuesOrdering = dr.Field<string>("QuesOrdering");


        //若該學生沒有作答，則直接略過該學生即可。
        if (AnsewerTemp == null)
            return;

        //create a XMLHandler object to access the content in the AITypeQuestion XML file.
        XMLHandler xmlHandler = new  XMLHandler(Server.MapPath(questionXMLPath));

        //get the "NameOrNumberAnsweringMode_Session" of the  AITypeQuestion from the AITypeQuestion XML file
        string NameOrNumberAnsweringMode_Session = xmlHandler.getValueOfSpecificNonNestedTag("NameOrNumberAnsweringMode");


        
        //for marking "NumberAnsweringMode" AITypeQuestion
        if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
        {
            //若尚未批改過這次AI題考試或練習的話，執行批改AI題的function，並將批改後的成績顯示出來。
            ScoreAnalysisM log = new ScoreAnalysisM(StudentIDTemp, AnsewerTemp, QuesOrdering, correctXML, cQID_Selector, questionTotalScore, cActivityID);
            ScoreAnalysisList.Add(log);
            //比對學生作答內容中的XML檔名
            if (log.XMLerror)
            {
                Response.Write("alert('Student answer XML file name does not match Correct answer XML file name')");
                Response.End();
            }
        }
        //for marking "NameAnsweringMode" AITypeQuestion
        else if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
        {
            //若尚未批改過這次AI題考試或練習的話，執行批改AI題的function，並將批改後的成績顯示出來。
            ScoreAnalysisM log = new ScoreAnalysisM(StudentIDTemp, AnsewerTemp, QuesOrdering, correctXML, correctAnswerHT, cQID_Selector, questionTotalScore, cActivityID);
            ScoreAnalysisList.Add(log);
            //比對學生作答內容中的XML檔名
            if (log.XMLerror)
            {
                Response.Write("alert('Student answer XML file name does not match Correct answer XML file name')");
                Response.End();
            }
        }

       
        

        


    }


    //Step 1-4 
    //Access the field ‘correctAnswer’ in the datatable ‘AITypeQuestionCorrectAnswer’
    //for the correct answer of question with the current cQID, and construct a QuestionOrdering/correctAnswer Hash table for marking student’s answer.


    private void getCorrectAns_ConstructCorrectAnsHashTable( string cQID_Selector)
    {

        DataTable dt = CsDBOp.GetAllTBData("AITypeQuestionCorrectAnswer", cQID_Selector);
        //Exception for empty table data
        if (dt.Rows.Count == 0)
            Response.End();


        //Test for cActivityID with input for AITypeQuestionCorrectAnswer(which was IPCExamHWCorrectAnswer in 博宇's implementation)
        QuestionName = dt.Rows[0].Field<string>("QuestionBodyPart").Split(',');
        string TempCA = dt.Rows[0].Field<string>("correctAnswer");
        foreach (string TempCA_fullstr in TempCA.Remove(TempCA.Length - 1).Split(':'))
        {
            //store all the correctAnswer of each organ question in list 'MemberQuestionAnswer'.
            MemberQuestionAnswer.Add(TempCA_fullstr.Split(','));
        }


        //Construct a correctAnswer/QuestionOrdering Hash table for marking student's answer
        string temp_str = dt.Rows[0].Field<string>("correctAnswerOrdering");
        string[] tempCAOstr = temp_str.Remove(temp_str.Length - 1).Split(':');
        correctAnswerHT = new Hashtable[tempCAOstr.Length];
        int index = 0;
        foreach (string tempCAOstr_split in tempCAOstr)
        {
            string[] tempCAOstr_in_split = tempCAOstr_split.Split(',');
            correctXML.Add(tempCAOstr_in_split[0]);
            correctAnswerHT[index] = new Hashtable();
            for (int i = 1; i < tempCAOstr_in_split.Length; i++)
            {
                correctAnswerHT[index].Add(tempCAOstr_in_split[i], MemberQuestionAnswer[index][i]);
            }
            index++;
        }


    }

    //calculate student's total score
    private void calStudentTotalScore(int studentNoCounter)
    {
        int studentTotalScore = 0;
        //skip the first item in ScoreAnalysisList[studentNoCounter].Grade[studentNoCounter], which is the perQuestionTotalScore when add up the total score that the student got.
        //e.g., 20,5,5,1,2->5,5,1,2
        foreach (string scoreOfEachQuestion in ScoreAnalysisList[studentNoCounter].Grade[studentNoCounter].Skip(1))
        {
            
            studentTotalScore += int.Parse(scoreOfEachQuestion);
        }
        ScoreAnalysisList[studentNoCounter].studentTotalScore = studentTotalScore;
        ScoreAnalysisList[studentNoCounter].questionTotalScore = ScoreAnalysisList[studentNoCounter].Grade[studentNoCounter][0];
    }



  
    //Step 4-2 If the student clicks the “View the correct answer in 3D” button, 
    //we will show the “Show/Hide” icon for each organ on the webpage to allow the student to hide or show the 3D organs displayed in the 3DBuilder.
    public void displayShowHideIconCol_BtnClick()
    {

        switchOfDisplayShowHideIconCol("on");

    }


    //hide or show Show/Hide icon
    public void switchOfDisplayShowHideIconCol(string onOrOff)
    {
        if (onOrOff == "on")
        {
            hidden_DisplayShow_HideCol.Value = "Yes";

            /*this part still doesn't work, we can't display the hidden Show/Hide icon column*/
            //display all the “Show/Hide” icon for each organ on the webpage to allow the student to hide or show the 3D organs displayed in the 3DBuilder.
            gvScore.HeaderRow.Cells[4].Visible = true;


            for (int i = 0; i < gvScore.Rows.Count; i++)
            {
                gvScore.Rows[i].Cells[4].Visible = true;
            }

        }



        if (onOrOff == "off")
        {
            hidden_DisplayShow_HideCol.Value = "No";

            /*this part still doesn't work, we can't display the hidden Show/Hide icon column*/
            //display all the “Show/Hide” icon for each organ on the webpage to allow the student to hide or show the 3D organs displayed in the 3DBuilder.
            gvScore.HeaderRow.Cells[4].Visible = false;


            for (int i = 0; i < gvScore.Rows.Count; i++)
            {
                gvScore.Rows[i].Cells[4].Visible = false;
            }
        }
      

    }
    //get the parameters in URL and store there value in global var.
    private void retrieveURLParameters()
    {
        //set the variable questionXMLPath with the parameter strQID in URL if it is provided.
        if (Request.QueryString["strQID"] != null && Request.QueryString["strQID"] != "")
        {
            strQID = Request.QueryString["strQID"];
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


        //set the variable cPaperID with the parameter cPaperID in URL if it is provided.
        if (Request.QueryString["cPaperID"] != null && Request.QueryString["cPaperID"] != "")
        {
            cPaperID = Request.QueryString["cPaperID"];
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
            //Response.Write("<script>alert('考試時間已結束');location.href='ALHomePage.aspx?strQID=" + strQID + "&strUserID=" + strUserID + "&cActivityID=" + cActivityID + "'</script>");

            //remind the student that the exam is over, and redirect back to the ALHomePage.aspx, the first page when answering the AITypeQuestion. 
            Response.Write("<script>alert('The exam time is already over.');location.href='ALHomePage.aspx?strQID=" + strQID + "&strUserID=" + strUserID + "&cActivityID=" + cActivityID + "'</script>");
            
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
    
    
    //Step 2-1 apply the retrieved student’s question organ number to display the student’s answer in this order.
    //originating from Item.aspx btnKnee_Click()
    private void applyStudentsQuestionOrganNo()
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
        
        //int[] pickedQuestions = { 1, 3, 5 }; //The Question Number of organs  picked by instructor will be retrieved from DB in the near future. 


        //////////////////////////////////////////////////////////////////////////







        //if it's in exam mode.
        if (ExamMode)
        {


           

            int[] intQuestionOrderingString = new int[ScoreAnalysisList[0].questionOrderingString.Length-1];

            for (int x = 0; x < intQuestionOrderingString.Length; x++)
            {

                intQuestionOrderingString[x] = Convert.ToInt32(ScoreAnalysisList[0].questionOrderingString[x+1]);

            }
           

            RandomQuestionNoSession = intQuestionOrderingString;



            //send randomized  Question Numbers picked by instructor to 3DBuilder .
            string strRandomQuestionNo = "";
            intArray2AString(RandomQuestionNoSession, ref strRandomQuestionNo);

            /*
            //use JS alert() in C#
            ScriptManager.RegisterStartupScript(this,
             typeof(Page),
             "Alert",
             "<script>alert('" + "3 " + XMLFolder + questionXMLPath +" "+ strRandomQuestionNo + "');</script>",
             false);
            */


            //2019/4/10 Ben commented it because currently we don't need to send the randomized question number to 3DBuilder.
            /*
            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine("3 " + absoluteKneeXMLFolder + QuestionFileName + "_" + strRandomQuestionNo);//send protocol,Data to 3DBuilder.
            */
          


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


   

    //activate CSNamedPipe.exe automatically, and wait for the user to activate the 3DBuilder.
    public void btn_setUpCSNamedPipe_Onclick(object sender, EventArgs e)
    {
        //temporarily we only activate CSNamedPipe.exe, and manually activate 3DBuilder
        //run CSNamedPipe.exe
        runCSNamedPipe();

       

    }


    //Step 5 Display the correct  organ name of the organs in 3DBuilder  
    //that the student didn’t answer correctly.

    public void btn_connectTo3DBuilder_Onclick(object sender, EventArgs e)
    {



        //initiate 3DBuilder:  Set Mode to Practice Mode in 3DBuilder for initialization
        setModeIn3DBuilderForInit();


        //wait for the 3DBuilder to respond
        System.Threading.Thread.Sleep(10);


       
        //compose a string that contains student's question ordering of the organs 
        string strStuQuestionOrdering="";

        //convert the student's question ordering of the organs from an array of string to a string
        for (int x = 0; x < ScoreAnalysisList[0].questionOrderingString.Length-1; x++)
        {

            strStuQuestionOrdering += ScoreAnalysisList[0].questionOrderingString[x + 1]+" ";

        }

        //Step 5-2 Send the organ XML file of the AITypeQuestion to the 3DBuilder.
        //Step 5-4 Send students question ordering of the organs to the 3DBuilder to display the organs in the corresponding order.
        loadOrganXMLIn3DBuilderForExamMode(strStuQuestionOrdering);

        displayShowHideIconCol_BtnClick();

        //display the show all btn
        ShowOrHideAll.Style.Add("display", "block");
        lb_showHiddenOrgans.Style.Add("display", "block");
    }


    // Step 5-1 Send initiation command to the 3DBuilder.
    //initiate 3DBuilder:  Set Mode to Practice Mode in 3DBuilder for initialization
    private void setModeIn3DBuilderForInit()
    {
        //originating from ALHomePage.aspx
        sendMsg23DBuilder("1 2");

    }

    //Step 5-2 Send the organ XML file of the AITypeQuestion to the 3DBuilder.
    //Step 5-4 Send students ordering of the organs to the 3DBuilder to display the organs in the corresponding order.
    //make 3DBuilder load the target Organ XML file and display it
    //and send the randomized organ question number to the organs displayed in 3DBuilder 
    private void loadOrganXMLIn3DBuilderForExamMode(string strRandomQuestionNo)
    {

        sendMsg23DBuilder("3 " + absoluteKneeXMLFolder + QuestionFileName + "_" + strRandomQuestionNo);//send protocol,Data to 3DBuilder.

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

    public void btnBack_Click(object sender, EventArgs e)
    {
       
        //kill the corresponding running CsNamedPipe.exe process which is created when the teacher clicks "connect to 3DBuilder" to edit the AITypeQuestion in 3DBuilder.
        killCorrespondingCSNamedPipe();
         
      


    }

    private void killCorrespondingCSNamedPipe()
    {
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
        NameOrNumberAnsweringMode_Session = xmlHandler.getValueOfSpecificNonNestedTag("NameOrNumberAnsweringMode");

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
        }

        else if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
        {
            for (int i = 0; i < gvScore.Rows.Count; i++)
            {


                var organIndicator = gvScore.Rows[i].FindControl("TB_OrganIndicator") as Label;

                organIndicator.Text = (i + 1).ToString();
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
            string strTB1;//暫儲存學生每格答案
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
                TextBox tb = (TextBox)gvScore.Rows[RandomQuestionNum - 1].FindControl("TB_AnsweringField");
                strTB1 = tb.Text.Trim();
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

            CsDBOp.InsertStuIPCAns(strUserID, questionXMLPath, StudentAnswer._QuesOrdering, StudentAnswer._StudentAnswer, Num_Of_Question_Submision_Session,cActivityID);//插入學生data至darabase

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



    }


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

            //generated random question number with Peter's function
            int[] randQuestionNoList = RandomQuestionNoSession;

            // Convert the row index stored in the CommandArgument
            // property to an Integer.
            int index = Convert.ToInt32(e.CommandArgument);

            // Get the last name of the selected author from the appropriate
            // cell in the GridView control.
            GridViewRow selectedRow = gvScore.Rows[index];
            var tbx = selectedRow.FindControl("TB_AnsweringField") as TextBox;
            var num = selectedRow.FindControl("TB_OrganIndicator") as Label;
            /////////////////////////////////////////////
            //text exam mode
            //store the Question number
            string QuestionNo;

            //if it's in the exam mode

            //2019/4/9 Ben commented for using default URL value if no paras provided.
            //if (Request["examMode"] == "Yes")
            if (examMode == "Yes")
            {

                QuestionNo = (Array.IndexOf(randQuestionNoList, Int32.Parse(num.Text)) + 1).ToString();
                // QuestionNo = index.ToString();

            }

            //if it's not in the exam mode
            else
            {
                QuestionNo = num.Text;
            }
            /////////////////////////////


            //get the corresponding correct organ name 
            // var answer = CorrectOrganNameSession[Convert.ToInt32(QuestionNo) - 1];

            var answer = CorrectOrganNameSession[Convert.ToInt32(QuestionNo)];


            string input = tbx.Text.Replace(" ", "_");
            //Bent 2017 test
            string contact = "5 " + input + " " + QuestionNo + " " + answer.ToString();
            //string contact = "5 " + input + " "  + answer.Value.ToString();
            sendMsg23DBuilder(contact);

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

            var num = selectedRow.FindControl("TB_OrganIndicator") as Label; //Index of the selected 3D object

            //get the corresponding correct organ name 
            var answer = CorrectOrganNameSession[Convert.ToInt32(num.Text) - 1];//The correct name of selected 3D object 

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

            string contact = "6 " + HideOrShow + " " + answer.ToString(); //send "6 Hide realOrganName" to 3DBuilder

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



    public string getStudentScoreAndQuestionTotalScore()
    {

        return ScoreAnalysisList[0].studentTotalScore.ToString()+"/"+ScoreAnalysisList[0].questionTotalScore;

    
    }

}
