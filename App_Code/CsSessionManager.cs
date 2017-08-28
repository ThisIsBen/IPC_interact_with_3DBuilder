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
}