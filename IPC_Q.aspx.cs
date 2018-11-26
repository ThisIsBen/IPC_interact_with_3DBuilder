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
    //private static System.Diagnostics.Process os = new System.Diagnostics.Process();
   // public static System.Diagnostics.Process rd = new System.Diagnostics.Process();
   // private StreamWriter myStreamWriter = null; 
    
    
    //In the near future ,we will get the IPC_Question_OriginXMLPath from URL para or other para transmission method.
    string IPC_Question_OriginXMLPath = "./IPC_Question_Origin/SceneFile_ex.xml";
    
    //In the near future ,we will get the Path from URL para or other para transmission method.
    string XMLFolder = "IPC_Questions/1161-1450/";
    string questionXMLPath = "SceneFile_Q1.xml";

    //11/9 set string cActivityID = "006" temporarily
    string cActivityID = "009";
    //11/9 string QuestionBodyPart="Knee";
    string QuestionBodyPart = "Knee";
    //string QuestionBodyPart = "Stomach";
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
            ds.ReadXml(Server.MapPath(IPC_Question_OriginXMLPath)); //must synchronized with the XML file in Items.aspx.cs:  wr.WriteLine("3 D:\\Mirac3DBuilder\\HintsAccounts\\Student\\Mirac\\1161-1450\\SceneFile13.xml");//send protocol,Data to 3DBuilder.
            //in Items.aspx.cs

            //DataRow dtRow = dtScore.NewRow();
            //dtRow["Seq"] = "1";
            //dtScore.Rows.Add(dtRow);
            gvScore.DataSource = ds.Tables["Organ"];
            //gvScore.DataMember = 
            gvScore.DataBind();
            gvScore.HeaderRow.TableSection = TableRowSection.TableHeader;






            //add an invisible row which contains a btn to activate ShowAll 
            
        }
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


        //read in the XML files that contains all organs of a certain body part. e.g., Knee 
        XMLHandler xmlHandler = new XMLHandler(Server.MapPath(IPC_Question_OriginXMLPath));

        
        //if the selected AITypeQuestionMode is the "SurgeryMode", we append AITypeQuestionMode tag to "SurgeryMode".
        if (Request.Form["radioBtn_AITypeQuestionMode"] != null)
        {
            string selectedAITypeQuestionMode = Request.Form["radioBtn_AITypeQuestionMode"].ToString();
            
            //append AITypeQuestionMode tag to "SurgeryMode".
            if (selectedAITypeQuestionMode == "Surgery Mode")
            {
                xmlHandler.appendTag2EachOrgan("AITypeQuestionMode", "Surgery Mode", "OneElememt","Scene");
            }
          

        }
        
        

        //append Question tag to each Organ with init value = No.
        xmlHandler.appendTag2EachOrgan("Question","No","NestedStructure","Organ");


        //set the Visibility of all the organs to visible by setting its Visible tag to "1" except for skin 
        //first para is the tag name of the target tag,the second is the Specific Value,and the third is the new value that user wants to set as the tags new value.
        xmlHandler.setValueOfSpecificTagsWithSpecificValue("Visible", "0", "1");

        //convert  XmlDocument object to XElement object to use "where" phrase to locate a specific tag;
        xmlHandler.convertXmlDoc2XElement();
        

        //check if a check box is checked
        foreach (GridViewRow row in gvScore.Rows)
        {
            if (((row.FindControl("checkbox_pickedOrgan"))as CheckBox).Checked)
            {
                var OrganName = row.FindControl("LBTextBox_OrganName") as Label;
               
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
        }

        // determine whether add data to DB, question reload、no bodypart
        if (xmlHandler.correctAnswer.Length == 0)
            return;

        //11/9 store question XML file name ('questionXMLPath')  to DB IPCExamHWCorrectAnswer table/ correctAnswer and correctAnswerOrdering
        //11/9 store correct answer list to DB IPCExamHWCorrectAnswer table/ correctAnswer
        //11/9 store order of correct answer list to DB IPCExamHWCorrectAnswer table/ correctAnswerOrdering
        string xmlpath = XMLFolder + questionXMLPath;
        string CA=xmlpath+xmlHandler.correctAnswer+":";
        string CAO = xmlpath + xmlHandler.correctAnswerOrder+":";
        string QBP = QuestionBodyPart;


        CsDBOp.InsertIPCExamHWCorrectAns(cActivityID, CA, QBP, CAO);

        if (Request.Form["radioBtn_AITypeQuestionMode"].ToString() == "Surgery Mode")
        {

            //set Skin to be cuttable     
            xmlHandler.setATargetTag2ANewValue("Cut", "Skin", "1");

        }
        else
        {
            //if Skin is not picked as a question, we can hide the Skin
            if (!xmlHandler.checkOrganAttrValue("Skin", "Question", "Yes"))
            {
                //hide the Skin
                xmlHandler.setATargetTag2ANewValue("Visible", "Skin", "0");
            }

           
        }

       //store as XML
        xmlHandler.saveXML(Server.MapPath(xmlpath));
        
        
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
