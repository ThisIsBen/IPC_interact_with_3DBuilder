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
    protected int[] RandomQuestionNoSession//project is 徵件或報告
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
}