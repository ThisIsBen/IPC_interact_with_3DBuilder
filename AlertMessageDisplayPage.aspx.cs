using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aAlertMessageDisplayPage : CsSessionManager
{
    protected void Page_Load(object sender, EventArgs e)
    {


        /*removes all the objects stored in a Session. 
           If you do not call the Abandon method explicitly, the server removes these objects and destroys the session when the session times out.
           It also raises events like Session_End.*/
        Session.Abandon();
    
    }
}