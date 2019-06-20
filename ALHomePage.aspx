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

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css"/>
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br /><br /><br />
  
       <center><label style="font-size:45px;font-weight:bold;" >歡迎使用  "創新解剖學學習系統"</label></center>      
       <ContentTemplate>
           <div class="row">
               <div class="col-md-2"></div>
               <div class="col-md-9">
            
       <h2 style="color:red;">使用本系統前需下載'3DBuilder MFC Application.rdp'<br />以連線使用顯示3D器官的軟體'3DBuilder'，<br /><span style="color:green;">如已有下載，則不須重新下載。</span>如尚未下載，請點此  <u><a target="_blank" href='<%= CsDynamicConstants.RemoteDeskTopRDPFile_For3DBuilder_DownloadLink %>'>下載</a></u> </h2>
    
                    <br /> 
               
        
                <div style="text-align:left">
                <h2 style="color:red;">請依循下列步驟操作: <br />
                    <ul class="list-group">
                         <li class="list-group-item list-group-item-success">第一步: 請開啟 '3DBuilder MFC Application.rdp'</li>
                              <li class="list-group-item list-group-item-success">第二步: 在'3DBuilder'視窗點選"Yes"按鈕</li>
                              <li class="list-group-item list-group-item-success">第三步: 輸入您的Hints Web System 使用者帳號後，'3DBuilder'視窗會被開啟</li>
                              <li class="list-group-item list-group-item-success">第四步: 接著點選本頁下方的 '進入系統' 按鈕<br /><br />
                                  <center><asp:Button runat="server"  id="btnCheck" class="  btn btn-success btn-lg " Text="進入系統"   OnClick="btnCheck_Click"   style="right:80%;font-size : 60px;"   />
                             </center>
                                   </li>
                   </ul>
                </div>
        
                   </div>
        <br />
               </div>

           <div class="col-md-1"></div>
            </ContentTemplate>
       <%-- <center>
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
        </center>--%>
        <br /><br />
       
    </div>
    </form>
</body>
</html>
