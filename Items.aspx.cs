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
    protected void btnKnee_Click(object sender, EventArgs e)
    {
        StreamWriter wr = (StreamWriter)Session["Writer"];
        wr.WriteLine("3 " + XMLFolder + questionXMLPath);//send protocol,Data to 3DBuilder.
        //wr.WriteLine("3  " + questionXMLPath);//send protocol,Data to 3DBuilder.
 //!!!!//如何將上方兩個參數傳送到3DBuilder那裏的IPCInterface呢? 上行的WriteLine會寫到哪裡呢?
        Response.Redirect("IPC.aspx");
    }
}