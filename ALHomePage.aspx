<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ALHomePage.aspx.cs" Inherits="ALHomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Anatomy Learning</title>

 <style type="text/css">
.css_btn_class {
	font-size:16px;
	font-family:Arial;
	font-weight:normal;
	-moz-border-radius:8px;
	-webkit-border-radius:8px;
	border-radius:8px;
	border:1px solid #dcdcdc;
	padding:9px 18px;
	text-decoration:none;
	background:-moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
	background:-ms-linear-gradient( top, #ededed 5%, #dfdfdf 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
	background:-webkit-gradient( linear, left top, left bottom, color-stop(5%, #ededed), color-stop(100%, #dfdfdf) );
	background-color:#ededed;
	color:#000000;
	display:inline-block;
 	-webkit-box-shadow:inset 1px 1px 0px 0px #ffffff;
 	-moz-box-shadow:inset 1px 1px 0px 0px #ffffff;
 	box-shadow:inset 1px 1px 0px 0px #ffffff;
}.css_btn_class:hover {
	background:-moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
	background:-ms-linear-gradient( top, #dfdfdf 5%, #ededed 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
	background:-webkit-gradient( linear, left top, left bottom, color-stop(5%, #dfdfdf), color-stop(100%, #ededed) );
	background-color:#dfdfdf;
}

</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br /><br /><br />
        <br /><br /><br />
        <br /><br /><br />
        
      
         <center><label style="font-size:45px;font-weight:bold;" >歡迎使用Anatomy Learning System</label></center>   
        <center><h2 style="color:red;">使用本系統需安裝Azure RemoteAPP，如未安裝，請照以下安裝步驟安裝;</h2></center>  
        <center><h2 style="color:red;">如安裝完成，並開啟Azure RemoteAPP中的Anatomy Learning後，請點選下一步按鈕。</h2></center>  

        <br />
        <center>
        <div style="width:610px;" >
            <label style="font-size:35px;font-weight:bold;" >安裝步驟</label> <br /><br />
            <div align="left">
            <label style="font-size:25px; left:0px;">1. 	請至 <a href="https://www.remoteapp.windowsazure.com/en/clients.aspx">Microsoft官方網站</a> 下載Azure RemoteApp安裝檔</label>
            <br />
            <label style="font-size:25px; left:0px;">2. 	點擊Azure RemoteApp安裝檔安裝</label>
            <br />
            <label style="font-size:25px; left:0px;">3. 	開啟Azure RemoteApp並以Microsoft帳號登入</label>
            </div>
        </div>
        </center>
        <br /><br />
        <asp:Button runat="server"  id="btnCheck" class="css_btn_class" Text="下一步" OnClick="btnCheck_Click"  style="position:absolute;right:250px;"  />
    </div>
    </form>
</body>
</html>
