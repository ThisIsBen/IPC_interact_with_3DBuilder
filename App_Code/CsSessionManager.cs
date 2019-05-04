using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
/// <summary>
/// CsSessionManager 的摘要描述
/// </summary>
public class CsSessionManager : System.Web.UI.Page
{
    protected int[] RandomQuestionNoSession//Store the randomized Question Numbers ,which were picked by Instructor
    {
        get
        {
            if (Session["randomQuestionNo"] == null)
            {
                int[] empty={-1};
                Session["randomQuestionNo"] = empty;
            }
            return (int[])Session["randomQuestionNo"]; ;

        }
        set
        {
            Session["randomQuestionNo"] = value;
        }
    }


    protected List<string> CorrectOrganNameSession//Store correct answer of Organ name
    {
        get
        {
            if (Session["CorrectOrganNameSession"] == null)
            {
                List<string> empty = new List<string>();
                Session["CorrectOrganNameSession"] = empty;
            }
            return (List<string>)Session["CorrectOrganNameSession"]; 

        }
        set
        {
            Session["CorrectOrganNameSession"] = value;
        }
    }
    //存完成的大題次數
    protected  int Num_Of_Question_Submision_Session//Store correct answer of Organ name
    {
        get
        {
            if (Session["Num_Of_Question_Submision_Session"] == null)
            {
                //出始值為0
                Session["Num_Of_Question_Submision_Session"] = 0;
            }
            return (int)Session["Num_Of_Question_Submision_Session"];

        }
        set
        {
            Session["Num_Of_Question_Submision_Session"] = value;
        }
    }



    //store the URLof the previous page
    protected string Previous_Page_URL_Session//Store correct answer of Organ name
    {
        get
        {
            if (Session["Previous_Page_URL_Session"] == null)
            {
                //出始值為""
                Session["Previous_Page_URL_Session"] = "";
            }
            return (string)Session["Previous_Page_URL_Session"];

        }
        set
        {
            Session["Previous_Page_URL_Session"] = value;
        }
    }

    //store the URLof the previous page
    protected string NameOrNumberAnsweringMode_Session//Store correct answer of Organ name
    {
        get
        {
            if (Session["NameOrNumberAnsweringMode_Session"] == null)
            {
                //出始值為""
                Session["NameOrNumberAnsweringMode_Session"] = "";
            }
            return (string)Session["NameOrNumberAnsweringMode_Session"];

        }
        set
        {
            Session["NameOrNumberAnsweringMode_Session"] = value;
        }
    }

    
}