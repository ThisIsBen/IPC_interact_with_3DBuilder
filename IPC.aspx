<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeFile="IPC.aspx.cs" Inherits="IPC" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--Include Bootstrap -->
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.0/jquery.min.js"></script>
    <style type="text/css">
        th {
            
            font-size:20px;
            text-align: center;         
            background-color: #ffd800;
            
           
        }

        .table-hover tbody tr:hover td, .table-hover tbody tr:hover th {
            background-color: #b6ff00;
        }

        .menu_img {
            width: 100%;
        }

        
    </style>

    <script type="text/javascript">
        
        //Bind webpage default close cross button with "Finish" button to shot down CsNamedPipe.exe when user close IPC webpage.
        window.onbeforeunload = function () {
            document.getElementById('<%= FinishBtn.ClientID %>').click();
        };


        //resume scroll position after asp.net postback

        var hdnScroll = document.getElementById("<%= hdnScrollPos.ClientID %>");


        window.onload = function () {
            //resume scroll position after asp.net postback
            //divAnswerSheet.scrollTop = hdnScroll.value;


           
            //comment it to test why "Can't read"
            
            //recover visbility icon of hide_showAll
           
            var hfHide_ShowAll = document.getElementById("<%= AllInOrVisible.ClientID %>");
            var iconHide_ShowAll = document.getElementById("<%= ShowOrHideAll.ClientID %>");
           
           
            if (hfHide_ShowAll.value == "true") {
                iconHide_ShowAll.src= "Image/visible.png";
            }
            /*
            else {
                iconHide_ShowAll.src = "Image/invisible.png";
            }
            */

            //resume mark icon in gridView
            var gvDrv = document.getElementById("<%= gvScore.ClientID %>");
            for (i = 1; i < gvDrv.rows.length; i++) {

                
                
                //recover visbility icon of each row
                if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[2].value == "true") {
                    gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = "Image/visible.png";
                }
                else {
                    gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = "Image/invisible.png";
                }
                
                //recover mark icon of each row
                switch (parseInt(gvDrv.rows[i].cells[1].getElementsByTagName("input")[3].value)) {

                    case 0:
                        gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = "image/notSure.png";
                        gvDrv.rows[i].cells[4].getElementsByTagName("input")[1].src = "image/giveUp.png";
                        break;
                    case 1:
                        gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = "image/notSureClick.png";
                        break;
                    case 2:
                        gvDrv.rows[i].cells[4].getElementsByTagName("input")[1].src = "image/giveUpClick.png";
                        break;




                }


            }

        }



        //change mark icon to "not sure " icon and remove the mark when the mark has existed with single click.
        function toNotSureIcon(lnk) {
            //get the clicked row of TemplageField
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;


            //change " " to "not sure"icon or change"not sure"icon and "give up"Icon to " "
            var img = row.cells[4].getElementsByTagName("input")[0].src;
            var imgBesideIt = row.cells[4].getElementsByTagName("input")[1].src;
            if (img.indexOf('notSureClick.png') != -1 ) {
                row.cells[4].getElementsByTagName("input")[0].src = "image/notSure.png";
                //record the current mark icon in the hidden field,0 means empty
                row.cells[1].getElementsByTagName("input")[3].value = 0;
            }

            
            else {
                if (imgBesideIt.indexOf('giveUpClick.png') != -1)
                {
                    row.cells[4].getElementsByTagName("input")[1].src = "image/giveUp.png";
                }
                row.cells[4].getElementsByTagName("input")[0].src = "image/notSureClick.png";
                
                //record the current mark icon in the hidden field,1 means notSure.png
                row.cells[1].getElementsByTagName("input")[3].value = 1;
            }

            //to prevent refreshing of page when button inside form clicked
            return false;
        }

        //change mark icon to "give up " icon with double click.
        function toGiveUpIcon(lnk) {
            //get the clicked row of TemplageField
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;

           
            //change to "give up" icon
            var img = row.cells[4].getElementsByTagName("input")[1].src;
            var imgBesideIt = row.cells[4].getElementsByTagName("input")[0].src;
            if (img.indexOf('giveUpClick.png') != -1) {

                row.cells[4].getElementsByTagName("input")[1].src = "image/giveUp.png";
                //record the current mark icon in the hidden field,0 means empty
                row.cells[1].getElementsByTagName("input")[3].value = 0;
            }
          
            else {
                if (imgBesideIt.indexOf('notSureClick.png') != -1)
                {
                    row.cells[4].getElementsByTagName("input")[0].src = "image/notSure.png";
                }
                row.cells[4].getElementsByTagName("input")[1].src = "image/giveUpClick.png";

                //record the current mark icon in the hidden field,1 means notSure.png
                row.cells[1].getElementsByTagName("input")[3].value = 2;
            }

            //to prevent refreshing of page when button inside form clicked
            return false;
        }

    </script>



    <div align="center">
        <asp:HiddenField ID="hdnScrollPos" runat="server" />
        <div class="jumbotron">
            <%--        <asp:Button ID="StartIPC" OnClick="StartIPC_Click" Text="開始" runat="server" />
        <asp:Button ID="Button1" OnClick="Button1_Click" Text="傳遞參數" runat="server" />--%>
            <div class="container">
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Button ID="FinishBtn" OnClick="FinishBtn_Click" Text="Finish" runat="server" />
                    </div>
                    <div class="col-sm-6">
                       
                    </div>
                </div>
                <%--<input type="text" id="TBX_Input" runat="server" />--%>
                <%--<input type="button" onclick="" ID="StartRemoteApp"  Text="開始RemoteAPP" runat="server" />--%>
            </div>
        </div>
         <div>
                            <label>顯示/隱藏全部</label>
                        </div>
                        <div>
                            <input type="image" id="ShowOrHideAll" src="~/Image/visible.png" runat="server" onclick="javascript: __doPostBack('ShowOrHideAll', ''); return false;"  />
                            <asp:HiddenField ID="AllInOrVisible" runat="server" Value="true" />
                        </div>


        <div class="row">

            <asp:Panel ID="scorePanel" runat="server" Width="60%" HorizontalAlign="Center">
                <asp:GridView CssClass="table  table-condensed table-bordered table-hover table-responsive " ID="gvScore" runat="server" ShowHeaderWhenEmpty="true" OnRowCommand="gvScore_RowCommand">

                    <Columns>

                        <asp:TemplateField HeaderText="號碼">
                            <ItemTemplate>
                                <asp:Label ID="TextBox_Number" CliendIDMode="static" name="TextBox3" Visible="true" runat="server" Text='<%# Eval("Number") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ControlStyle-Width="80%" ControlStyle-Height="40px" HeaderText="名稱">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox_Text" ClientIDMode="static" Visible="true" runat="server" Text="" />
                                <asp:HiddenField ID="TextBox_Answer" runat="server" Value='<%# Eval("Name") %>' />
                                <asp:HiddenField ID="InOrVisible" runat="server" Value="true" />
                                <asp:HiddenField ID="markRecord" runat="server" Value="0" />
                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:ButtonField ButtonType="Image" CommandName="Submit" ImageUrl="~/Image/checkmark2.png" ControlStyle-Height="40px" HeaderText="確認">
                            <ControlStyle CssClass=" menu_img" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Image" CommandName="InvisibleAndVisible" ImageUrl="~/Image/checkmark2.png" ControlStyle-Height="40px" HeaderText="顯示/隱藏">
                            <ControlStyle CssClass=" menu_img" />
                        </asp:ButtonField>

                        <asp:TemplateField HeaderText="記號<br/>不確定/不會">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnMark" runat="server" CssClass="img-thumbnail" ImageUrl="image/notSure.png" OnClientClick="return toNotSureIcon(this) " ControlStyle-Height="40px" />
                                <asp:ImageButton ID="btnMarkGiveUp" runat="server" CssClass="img-thumbnail" ImageUrl="image/giveUp.png" OnClientClick="return toGiveUpIcon(this)" ControlStyle-Height="40px" />

                            </ItemTemplate>


                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </div>


</asp:Content>
