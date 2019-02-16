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

    string questionXMLPath = "";
    string QuestionFileName = "";//只有檔名，沒有資料夾名稱
    /*
    string _StuCouHWDe_ID = "1381";//We just temporarily hard code it.This ID should be retrieved from a session variable or URL variable.
    string cActivityID = "009";
    */
    string cUserID = "";
    string cQID = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        questionXMLPath = Request["strQID"] + ".xml";
        QuestionFileName = questionXMLPath;

        questionXMLPath = XMLFolder + questionXMLPath;

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
            
            //randomize the Question Organ Number
            randomizeQuestionOrganNo();
            
             */




        }



        /*This block should be implemented in the Hints set timer for the exam page
         * */
        //get the exam timespan from the UI , and convert it to Second format.
        int hardCodeExamTimespanSec = 120000; //now I just hard code it to 15 sec
        Double examTimespan = hardCodeExamTimespanSec * 1.0;
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
        CheckCountDownTimer(deadlineDateTime);




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
        os.StartInfo.FileName = Request.MapPath("CSNamedPipe.exe");
        os.StartInfo.UseShellExecute = false;
        os.StartInfo.RedirectStandardInput = true;
        os.StartInfo.Arguments = hintID;
        os.Start();
        StreamWriter wr = os.StandardInput;
        //os.StandardInput.Close();
        Session["Writer"] = wr;
        Session["Process"] = os;





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
        if (Request["examMode"] == "Yes")
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
            wr.WriteLine("3 " + absoluteKneeXMLFolder + QuestionFileName + " " + strRandomQuestionNo);//send protocol,Data to 3DBuilder.

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

    private void CheckCountDownTimer(DateTime deadlineDateTime)
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
    public void InsertStuIPCAns2DB(string cUserID, string cQID, string _QuesOrdering, string _StudentAnswer, int Num_Of_Question_Submision_Session)
    {
        CsDBOp.InsertStuIPCAns(cUserID, cQID, _QuesOrdering, _StudentAnswer, Num_Of_Question_Submision_Session);
    }
    */

    public void FinishBtn_Click(object sender, EventArgs e)
    {

        FinishBtn_ClickEventHandler();

    }

    private void FinishBtn_ClickEventHandler()
    {

        //commented it for Peter work on storing student's answer to DB
        //Break 3DBuilder connection 
        /*    
        Process os = (Process)Session["Process"];       
        os.Kill();
        */

        //get the cUserID and strQID from the URL parameters
        cUserID = Request["cUserID"];
        cQID = Request["strQID"];



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
        if (Request["examMode"] == "Yes")
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
                TextBox tb = (TextBox)gvScore.Rows[RandomQuestionNum - 1].FindControl("TextBox_Text");
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
            //測試將session回歸0
            // if (Num_Of_Question_Submision_Session > 2)
            //{
            //     Num_Of_Question_Submision_Session = 0;
            // }
            // else
            // {
            Num_Of_Question_Submision_Session++;
            // }

            //foreach (StuAnsM c in StudentAnswerList) //顯示list裡的資料
            //{

            //    Response.Write(c.StudentAnswer + ", " + c.QuesOrdering + " ");
            //}

            CsDBOp.InsertStuIPCAns(cUserID, cQID, StudentAnswer._QuesOrdering, StudentAnswer._StudentAnswer, Num_Of_Question_Submision_Session);//插入學生data至darabase

            //InsertStuIPCAns2DB(cUserID, cQID, StudentAnswer._QuesOrdering, StudentAnswer._StudentAnswer, Num_Of_Question_Submision_Session);//插入學生data至darabase
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
                    (row.FindControl(HFID) as HiddenField).Value = "true";//switch to visible
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
            var tbx = selectedRow.FindControl("TextBox_Text") as TextBox;
            var num = selectedRow.FindControl("TextBox_Number") as Label;
            /////////////////////////////////////////////
            //text exam mode
            //store the Question number
            string QuestionNo;

            //if it's in the exam mode
            if (Request["examMode"] == "Yes")
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

            var num = selectedRow.FindControl("TextBox_Number") as Label; //Index of the selected 3D object

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

}
