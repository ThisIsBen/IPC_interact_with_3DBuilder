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


public partial class IPC: System.Web.UI.Page
{
    //private static System.Diagnostics.Process os = new System.Diagnostics.Process();
   // public static System.Diagnostics.Process rd = new System.Diagnostics.Process();
   // private StreamWriter myStreamWriter = null; 
  
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
            ds.ReadXml(Server.MapPath("SceneFile13.xml")); //must synchronized with the XML file in Items.aspx.cs:  wr.WriteLine("3 D:\\Mirac3DBuilder\\HintsAccounts\\Student\\Mirac\\1161-1450\\SceneFile13.xml");//send protocol,Data to 3DBuilder.
            //in Items.aspx.cs

            //DataRow dtRow = dtScore.NewRow();
            //dtRow["Seq"] = "1";
            //dtScore.Rows.Add(dtRow);
            gvScore.DataSource = ds.Tables["Organ"];
            //gvScore.DataMember = 
            gvScore.DataBind();
            gvScore.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    public void FinishBtn_Click(object sender, EventArgs e)
    {
        
        Process os = (Process)Session["Process"];
        //string filename = os.ProcessName ;
        os.Kill();
        //File.Delete(Server.MapPath("~/") + @filename+".exe");
        
    }

    public String switchVisible_Invisible(GridViewRow selectedRow, string HFID, GridView gvScore)
    {
        bool BooLInOrVisible=false;
       
        if (selectedRow!=null)//hide or show the selected organ.
        {
            var HFInOrVisible =selectedRow.FindControl(HFID) as HiddenField;
            BooLInOrVisible = Convert.ToBoolean(HFInOrVisible.Value);
        }
       

        String HideOrShow = "";
        if (BooLInOrVisible )//if current state is visible
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
    public void ShowOrHideAllBtn_Click(object sender, EventArgs e)
    {
        //switch visibility icon All rows .
        String HideOrShow = switchVisible_Invisible(null, "InOrVisible", gvScore);

        /*
        //iterate through all the organs       
        String answerString = "";
        int GVtotalRow=gvScore.Rows.Count;
        
        for (int index = 0; index < GVtotalRow; index++)
        {



            answerString += (gvScore.Rows[index].FindControl("TextBox_Answer") as HiddenField).Value.ToString(); //The name of selected 3D object 
            if (index != GVtotalRow-1)
                answerString += "$";
            


        }
         */
        /*
        //use JS alert in C#

        ScriptManager.RegisterStartupScript(
        this,
        typeof(Page),
        "Alert",
        "<script>alert('" + answerString + "');</script>",
        false);
        */
       

        //send hide 3D organ msg to 3DBuilder
        //string contact = "7 " + HideOrShow;//send "7 Hide realOrganName" to 3DBuilder
        //string contact = "6 " + HideOrShow ;
        string contact = "7 " + HideOrShow ; //send "6 Hide realOrganName" to 3DBuilder        
        StreamWriter wr = (StreamWriter)Session["Writer"];

        /*
        //use JS alert() in C#
        ScriptManager.RegisterStartupScript(
         this,
         typeof(Page),
         "Alert",
         "<script>alert('" + contact + "');</script>",
         false);
        /////
       */
        wr.WriteLine(contact);//!!!!!send update msg to 3DBuilder
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
            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine(contact);//!!!!!send update msg to 3DBuilder
                // + num.Text + answer.Value.ToString()//;

            // Display the selected author.
           // Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>alert('" + contact + "');</script>");

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
            String HideOrShow = switchVisible_Invisible(selectedRow, "InOrVisible",null);
           
           /*
            //use JS alert() in C#
            ScriptManager.RegisterStartupScript(this,
             typeof(Page),
             "Alert",
             "<script>alert('" + (selectedRow.Controls[3]).ImageUrl + "');</script>",
             false);
             
            //send hide 3D organ msg to 3DBuilder
            */
            
            string contact = "6 " + HideOrShow + " " + answer.Value.ToString(); //send "6 Hide realOrganName" to 3DBuilder
            
            StreamWriter wr = (StreamWriter)Session["Writer"];
            wr.WriteLine(contact);//!!!!!send update msg to 3DBuilder
           
            
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
