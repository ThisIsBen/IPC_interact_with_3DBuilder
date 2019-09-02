using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
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
    /*
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
    */
    //store the StreamWriter of the CSNamedPipe.exe
    protected static StreamWriter CSNamedPipeStreamWriter_Session//Store correct answer of Organ name
    {
        get
        {
            if (HttpContext.Current.Session["CSNamedPipeStreamWriter_Session"] == null)
            {
                //出始值為""
                HttpContext.Current.Session["CSNamedPipeStreamWriter_Session"] = "";
            }
            return (StreamWriter)HttpContext.Current.Session["CSNamedPipeStreamWriter_Session"];

        }
        set
        {
            HttpContext.Current.Session["CSNamedPipeStreamWriter_Session"] = value;
        }
    }

    //store the StreamReader of the CSNamedPipe.exe
    protected static StreamReader CSNamedPipeStreamReader_Session//Store correct answer of Organ name
    {
        get
        {
            if (HttpContext.Current.Session["CSNamedPipeStreamReader_Session"] == null)
            {
                //出始值為""
                HttpContext.Current.Session["CSNamedPipeStreamReader_Session"] = "";
            }
            return (StreamReader)HttpContext.Current.Session["CSNamedPipeStreamReader_Session"];

        }
        set
        {
            HttpContext.Current.Session["CSNamedPipeStreamReader_Session"] = value;
        }
    }
    //store the Process of the CSNamedPipe.exe
    protected Process CSNamedPipeProcess_Session//Store correct answer of Organ name
    {
        get
        {
            if (Session["CSNamedPipeProcess_Session"] == null)
            {
                //出始值為""
                Session["CSNamedPipeProcess_Session"] = "";
            }
            return (Process)Session["CSNamedPipeProcess_Session"];

        }
        set
        {
            Session["CSNamedPipeProcess_Session"] = value;
        }
    }
    //store the Process ID of the CSNamedPipe.exe
    protected static string CSNamedPipePID_Session//Store correct answer of Organ name
    {
        get
        {
            if (HttpContext.Current.Session["CSNamedPipePID_Session"] == null)
            {
                //出始值為""
                HttpContext.Current.Session["CSNamedPipePID_Session"] = "";
            }
            return (string)HttpContext.Current.Session["CSNamedPipePID_Session"];

        }
        set
        {
            HttpContext.Current.Session["CSNamedPipePID_Session"] = value;
        }
    }

    //store the URL of the previous page
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

    //store the URL of the previous page
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


    //store the mapping of organ number and the randomized organ name with a dictionary.
    protected Dictionary<int,string> NumberAnsweringMode_RandOrganNoNameMapping_Session//Store correct answer of Organ name
    {
        get
        {
            if (Session["NumberAnsweringMode_RandOrganNoNameMapping_Session"] == null)
            {
                //出始值為null
                Session["NumberAnsweringMode_RandOrganNoNameMapping_Session"] = new Dictionary<int, string>(); 
            }
            return Session["NumberAnsweringMode_RandOrganNoNameMapping_Session"] as Dictionary<int, string>;

        }
        set
        {
            Session["NumberAnsweringMode_RandOrganNoNameMapping_Session"] = value;
        }
    }



    //When the AITypeQuestion is of 'Number Answering Mode',
    //we need to store the whole randomized organ number for creating a  mapping of organ number and the randomized organ name
    protected int[] NumberAnsweringMode_WholeRandOrganNo_Session
    {
        get
        {
            if (Session["NumberAnsweringMode_WholeRandOrganNo_Session"] == null)
            {
                int[] empty = { -1 };
                Session["NumberAnsweringMode_WholeRandOrganNo_Session"] = empty;
            }
            return (int[])Session["NumberAnsweringMode_WholeRandOrganNo_Session"]; ;

        }
        set
        {
            Session["NumberAnsweringMode_WholeRandOrganNo_Session"] = value;
        }
    }



}