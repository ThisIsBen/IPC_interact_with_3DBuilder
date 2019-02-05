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
using System.Xml.Linq;
using System.Text;


public partial class IPC: System.Web.UI.Page
{
    

    /*Temporary hard-code variable*/
    //QuestionBodyPart is already retrieved from URL parameter now.
    //string QuestionBodyPart = "Stomach";

    //In the near future ,we will get the Path from URL para or other para transmission method.
    string XMLFolder = CsDynamicConstants.relativeKneeXMLFolder;
    /*Temporary hard-code variable*/

    //store which body part is being used in this question
    string QuestionBodyPart = "";

    //store which OriginalBodyOrgan xml  file should be loaded according to which body part is being used in this question
    string completeBodyPartOrgansXMLPath = "";

    string questionXMLPath = "";

    //11/9 set string cActivityID = "006" temporarily
    //string cActivityID = "010";
   
    //use cQID to replace cActivityID after the integration with Hints system
    string cQID = "";
    

   


   
    protected void Page_Load(object sender, EventArgs e)
    {
        

        Page.MaintainScrollPositionOnPostBack = true;


        

        if (!IsPostBack)
        {

          

            
           

            gvScore.AllowPaging = false;
            gvScore.AllowSorting = true;
            gvScore.AutoGenerateColumns = false;
            gvScore.Style.Add("width", "100%");
            gvScore.PageIndex = 0;

            DataSet ds = new DataSet();

            //decide which body part organ xml file should be loaded according to which body part is being used in this question
            decide_QuestionBodyPartOrganXML();

            ds.ReadXml(Server.MapPath(completeBodyPartOrgansXMLPath)); //must synchronized with the XML file in Items.aspx.cs:  wr.WriteLine("3 D:\\Mirac3DBuilder\\HintsAccounts\\Student\\Mirac\\1161-1450\\SceneFile13.xml");//send protocol,Data to 3DBuilder.
            //in Items.aspx.cs

            //DataRow dtRow = dtScore.NewRow();
            //dtRow["Seq"] = "1";
            //dtScore.Rows.Add(dtRow);
            gvScore.DataSource = ds.Tables["Organ"];
            //gvScore.DataMember = 
            gvScore.DataBind();
            gvScore.HeaderRow.TableSection = TableRowSection.TableHeader;


            //if the instructor wants to check or modify the existing AITypeQuestion, 
            //we retrieve the existing question description and display it for the instructor to modify.
            if (Request.QueryString["viewContent"] != null && Request.QueryString["viewContent"] == "Yes")
            {
                AITypeQuestionDescription.InnerText = CsDBOp.getAITypeQuestion_QuestionDescription(Request.QueryString["strQID"]);

            }
            //add an invisible row which contains a btn to activate ShowAll 
            
        }
    }

    //decide which body part organ xml file should be loaded according to which body part is being used in this question
    private void decide_QuestionBodyPartOrganXML()
    {
            //2018011030 use the XML file name retrieved from the URL parameter to replace the hard code SceneFile_Q1.xml.
        //questionXMLPath = hidden_AITypeQuestionTitle.Value + ".xml"; The hidden field value will be "" at the first page load because we set the value of the hidden field,hidden_AITypeQuestionTitle, in document.ready in front end, which is run after the page_load function on backend.
            questionXMLPath = Request.QueryString["strQID"] + ".xml";


            //check which body part is being used in this question
            QuestionBodyPart = Request.QueryString["QuestionBodyPart"];

            //decide which body part organ xml file should be loaded
            if (QuestionBodyPart == "Knee")
            {

               
                if (File.Exists(CsDynamicConstants.absoluteKneeXMLFolder + questionXMLPath))
                {
                    //if the xml of this question has already existed,
                    //use it as the template
                    completeBodyPartOrgansXMLPath = CsDynamicConstants.relativeKneeXMLFolder + questionXMLPath;
                }

                else
                {   //use the original knee xml template
                    completeBodyPartOrgansXMLPath = CsDynamicConstants.completeKneeOrgansXMLPath;
                }
                

               


            }
           


    }

    //Store the content that the teacher has edited currently to the xml file when the teacher clicks the button to activate 3DBuilder.
    public void saveCurrentEditionOnWebPage()
    {
       
        //save the currently edited content on the AITypeQuestion editor page to xml file 
        FinishBtn_Click(this, null);
        
        //wait for the 3DBuilder to respond
        System.Threading.Thread.Sleep(100);
    }

    public void btn_cutBodyPartIn3DBuilder_Onclick(object sender, EventArgs e)
    {
        //temporarily we only activate CSNamedPipe.exe, and manually activate 3DBuilder
        //run CSNamedPipe.exe
        runCSNamedPipe();

        //Store the content that the teacher has edited currently to the xml file when the teacher clicks the button to activate 3DBuilder.
        saveCurrentEditionOnWebPage();
       
    }

    public void btn_connectTo3DBuilder_Onclick(object sender, EventArgs e)
    {

        //originating from ALHomePage.aspx
        StreamWriter wr = (StreamWriter)Session["Writer"];
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>alert('" + os.StartInfo.RedirectStandardInput + os.Id + "');</script>"); 
        //StreamWriter wr = os.StandardInput;

        wr.WriteLine("1 2");

        //wait for the 3DBuilder to respond
        System.Threading.Thread.Sleep(10);

        //originating from Item.aspx
        string absoluteKneeXMLFolder = CsDynamicConstants.absoluteKneeXMLFolder;
        //2018011030 use the XML file name retrieved from the URL parameter to replace the hard code SceneFile_Q1.xml.
        questionXMLPath = hidden_AITypeQuestionTitle.Value + ".xml";

      
        wr.WriteLine("3 " + absoluteKneeXMLFolder + questionXMLPath);//send protocol,Data to 3DBuilder.
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


    private static string GetXml(string url)
    {
        using (XmlReader xr = XmlReader.Create(url, new XmlReaderSettings() { IgnoreWhitespace = true }))
        {
            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter xw = XmlWriter.Create(sw))
                {
                    xw.WriteNode(xr, false);
                }
                return sw.ToString();
            }
        }
    }

    
   
    public void FinishBtn_Click(object sender, EventArgs e)
    {
        //add the <Question> tag to XML to indicate that the organ is picked to be a question or not.
        //string readIn_xml_str = GetXml("./IPC_Question_Origin/SceneFile_ex.xml");

        //decide which body part organ xml file should be loaded according to which body part is being used in this question
        decide_QuestionBodyPartOrganXML();

        //read in the XML files that contains all organs of a certain body part. e.g., Knee 
        XMLHandler xmlHandler = new XMLHandler(Server.MapPath(completeBodyPartOrgansXMLPath));

      
        
        //if the selected AITypeQuestionMode is the "SurgeryMode", we append AITypeQuestionMode tag to "SurgeryMode".
        if (Request.Form["radioBtn_AITypeQuestionMode"] != null)
        {
            string selectedAITypeQuestionMode = Request.Form["radioBtn_AITypeQuestionMode"].ToString();

            //set the Mode according to which AITypeQuestion mode is selected by the teacher
            if (selectedAITypeQuestionMode == "Surgery Mode")
            {
                
                xmlHandler.setValueOfSpecificNonNestedTag("AITypeQuestionMode", "Surgery Mode");

                //append AITypeQuestionMode tag to "SurgeryMode".
                //xmlHandler.appendTag2EachOrgan("AITypeQuestionMode", "Surgery Mode", "OneElememt","Scene");
            }

            if (selectedAITypeQuestionMode == "Anatomy Mode")
            {

                xmlHandler.setValueOfSpecificNonNestedTag("AITypeQuestionMode", "Anatomy Mode");

                //append AITypeQuestionMode tag to "SurgeryMode".
                //xmlHandler.appendTag2EachOrgan("AITypeQuestionMode", "Surgery Mode", "OneElememt","Scene");
            }
          
          

        }
        
        //Only if this is new question do we append <Question> to each organ in the xml file
        if (Request.QueryString["viewContent"] != "Yes")
        {
            //append Question tag to each Organ with init value = No.
            xmlHandler.appendTag2EachOrgan("Question", "No", "Organ");
        }
        else
        {
            //reset the <Question> of each organ
            xmlHandler.setValueOfSpecificTagsWithSpecificValue("Question", "Yes", "No");
        }
        
        
        //set the Visibility of all the organs to visible by setting its Visible tag to "1" except for skin 
        //first para is the tag name of the target tag,the second is the Specific Value,and the third is the new value that user wants to set as the tags new value.
        xmlHandler.setValueOfSpecificTagsWithSpecificValue("Visible", "0", "1");

       
        //record which organ is stored as a question, and which organ is set to be visible. 
        recordQuestionOrgan_InvisibleOrgan(xmlHandler);
        
        // we won't store the AITypeQuestion if there is no organ picked as a question.
        if (xmlHandler.correctAnswer.Length == 0)
            return;


        
        
       
        

       //handle the mode of the AITypeQuestion e.g.,Suergery Mode 
        handleAITypeQuestionMode(xmlHandler);
        
        


       

       

        //retrieve the cQID from the hidden field   
        cQID = hidden_AITypeQuestionTitle.Value;

        //11/9 store question XML file name ('questionXMLPath')  to DB IPCExamHWCorrectAnswer table/ correctAnswer and correctAnswerOrdering
        //11/9 store correct answer list to DB IPCExamHWCorrectAnswer table/ correctAnswer
        //11/9 store order of correct answer list to DB IPCExamHWCorrectAnswer table/ correctAnswerOrdering        
        string xmlpath = XMLFolder + questionXMLPath;
        string CA = xmlpath + xmlHandler.correctAnswer + ":";//compose the required format of the correct answer
        string CAO = xmlpath + xmlHandler.correctAnswerOrder + ":";//compose the required format of the correct answer order
        string QBP = QuestionBodyPart;//get the  body part that is used for the  question

        
        
        //store the content of the AITypeQuestion as XML
        xmlHandler.saveXML(Server.MapPath(xmlpath));

        //Ben temp comment  for demo and experiment
        
        //store the AITypeQuestion to DB
        store2DB(CA, QBP, CAO); //可能有問題
        
        //Ben temp comment for demo and experiment

        // only when the teacher actually click the "Save the Question" button can the system redirect back to the previous page
        if (e != null)
        {
            //redirect back to the Paper_MainPage.aspx (the exam paper editing page) in Hints.
            redirectBack2HintsPaper_MainPage();
        }
          
        
        


    }

    private void recordQuestionOrgan_InvisibleOrgan(XMLHandler xmlHandler)
    {
        //check if a check box is checked
        foreach (GridViewRow row in gvScore.Rows)
        {
            //get the organ name of the current row
            var OrganName = row.FindControl("LBTextBox_OrganName") as Label;

            if (((row.FindControl("checkbox_pickedOrgan")) as CheckBox).Checked)
            {


                //set the checked organs as Questions by set its Question tag to "Yes".
                xmlHandler.setATargetTag2ANewValue("Question", OrganName.Text, "Yes");

                /*
                //use JS alert() in C#
                ScriptManager.RegisterStartupScript(
                 this,
                 typeof(Page),
                 "Alert",
                 "<script>alert('" + target.Element("Question").Value + "');</script>",
                 false);
               */





            }

            if ((row.FindControl("hidden_markHideShowOrgan") as HiddenField).Value != "-1")
            {
                //2019/1/12 set the "Visible" tag of the organ to be hidden if the teacher set it to be invisible on AITypeQuestion editing page
                xmlHandler.setATargetTag2ANewValue("Visible", OrganName.Text, "0");
            }

        }


    }

    private void handleAITypeQuestionMode(XMLHandler xmlHandler)
    {
        if (Request.Form["radioBtn_AITypeQuestionMode"].ToString() == "Surgery Mode")
        {

            //set Skin to be cuttable     
            xmlHandler.setATargetTag2ANewValue("Cut", "Skin", "1");

            //set Skin to be visible
            xmlHandler.setATargetTag2ANewValue("Visible", "Skin", "1");

        }
        else
        {
            //if Skin is not picked as a question, we can hide the Skin
            if (!xmlHandler.checkOrganAttrValue("Skin", "Question", "Yes"))
            {
                //set Skin to be invisible
                xmlHandler.setATargetTag2ANewValue("Visible", "Skin", "0");
            }


        }

    }



    private void store2DB(string CA, string QBP, string CAO)
    {
        //create Or update required AITypeQuestion attributes in HintsDB 
        createOrUpdateAITypeQuestionInHintsDB();


        //store correct answer of the AI type question to DB
        CsDBOp.InsertIPCExamHWCorrectAns(cQID, CA, QBP, CAO);


        
    }

    //create Or update required AITypeQuestion attributes in HintsDB 
    private void createOrUpdateAITypeQuestionInHintsDB()
    {
        //get all the required parameter for creating a new AITypeQuestion or modifying an existing AITypeQuestion in the NewVersionHintsDB  .
        string strQID = Request.QueryString["strQID"];
        string strQuestion = AITypeQuestionDescription.InnerText;
        string strPaperID = Request.QueryString["strPaperID"];

        //When creating a new AITypeQuestion, we must store the AITypeQuestion into NewVersionHintsDB QuestionIndex, QuestionMode,Paper_Content datatable  
        //if Request.QueryString["viewContent"] == null means it is a new question rather than an existing one, which is opened again for modification.
        if (Request.QueryString["viewContent"] == null || Request.QueryString["viewContent"] != "Yes")
        {

            storeAITypeQuestion2HintsDB(strQID, strQuestion, strPaperID);


        }

        //When modifying an existing AITypeQuestion, we only need to update the content of the question description textarea.
        //Because the instructor can only modify its question description and the organs that are set as the questions.
        else
        {
            CsDBOp.updateAITypeQuestionDescription_QuestionIndex(strQID, strQuestion, null, 1);
        }

    }

    //redirect back to the Paper_MainPage.aspx (the exam paper editing page) in Hints.
    private void redirectBack2HintsPaper_MainPage()
    {
        //if the opener page is still opened, refresh it and close the current page.
        ClientScript.RegisterClientScriptBlock(this.GetType(), "Refresh", "opener.document.getElementById('btnRefresh').click();window.close();", true);
        
        //if the opener page is already closed, direct back to the opener page.
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "history.go(-2);", true);
       
    }


    //storing AI type question into NewVersionHintsDB QuestionIndex, QuestionMode datatable.
    private void storeAITypeQuestion2HintsDB(string strQID,string strQuestion,string strPaperID)
    {
        
        
        //get all the required parameter for storing AI type question into NewVersionHintsDB QuestionIndex, QuestionMode datatable.
        string strQuestionDivisionID = Request.QueryString["strQuestionDivisionID"];
        string strQuestionGroupID = Request.QueryString["strQuestionGroupID"];
        string strQuestionMode = Request.QueryString["strQuestionMode"];

        //All the questions need to store a record in QuestionIndex and QuestionMode table.
        //儲存一筆資料至QuestionIndex //
        CsDBOp.saveIntoQuestionIndex(strQID, strQuestion, null, 1);//set sLevel to 1 for the time being because although sLevel ranges from 0 to 12 ,only sLevel == 1 is used in the entire Hints system 

        //儲存一筆資料至QuestionMode  //###with parameter "strQuestionType" set to 9 in AITypeQuestion,which is defined in Paper_QuestionTypeNew.aspx to represent the  QuestionType of  AITypeQuestion
        CsDBOp.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "9", null, null);

        //儲存一筆資料至Paper_Content //add the AI Type Question to the exam paper.
        CsDBOp.saveToPaper_Content(strPaperID, strQID, "9", strQuestionMode);



    }
   
    //submit current edition to 3DBuilder
    protected void gvScore_RowCommand(Object sender, GridViewCommandEventArgs e)
    {

        // If multiple ButtonField column fields are used, use the
        // CommandName property to determine which button was clicked.
        if (e.CommandName == "Submit")
        {

            // Convert the row index stored in the CommandArgument
            // property to an Integer.
            int index = Convert.ToInt32(e.CommandArgument);

            // Get the last name of the selected author from the appropriate
            // cell in the GridView control.
            GridViewRow selectedRow = gvScore.Rows[index];
            var tbx = selectedRow.FindControl("TextBox_Text") as TextBox;
            var num = selectedRow.FindControl("TextBox_Number") as Label;
            var answer = selectedRow.FindControl("TextBox_Answer") as HiddenField;
            string input = tbx.Text.Replace(" ","");
            string contact = "5 " + input + " " + num.Text + " " + answer.Value.ToString();
            
               
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
            var answer = selectedRow.FindControl("TextBox_Answer") as HiddenField; //The name of selected 3D object 
            
            //switch visibility icon of the selected row .
            
           
           /*
            //use JS alert() in C#
            ScriptManager.RegisterStartupScript(this,
             typeof(Page),
             "Alert",
             "<script>alert('" + (selectedRow.Controls[3]).ImageUrl + "');</script>",
             false);
             
            //send hide 3D organ msg to 3DBuilder
            */
            
           
           
            
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
