<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlertMessageDisplayPage.aspx.cs"  Inherits="aAlertMessageDisplayPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script>

        var url = new URL(window.location.href);

        if (url.searchParams.get("messageContent") != null  )
        {

            alert(url.searchParams.get("messageContent"));
            //redirectURL = url.searchParams.get("redirectURL");
            //window.location.href = redirectURL;

            
            location.href = ("<%= Previous_Page_URL_Session %>");
           
            
        }


       

    </script>
    <style>
body {
  background-color: coral;
}

    </style>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <input type="hidden" id="hidden_redirectURL" runat="server"/>
    </div>
    </form>
</body>
</html>
