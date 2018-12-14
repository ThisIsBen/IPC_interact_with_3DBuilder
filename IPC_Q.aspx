<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="./Site.Master" CodeFile="IPC_Q.aspx.cs" Inherits="IPC"  MaintainScrollPositionOnPostback="true"%>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--Include Bootstrap -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css">


    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">
   
    <link href="Content/IPC_Layout.css" rel="stylesheet" />
   
     <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.0/jquery.min.js"></script>
    <script src="./bootstrap-checkbox/dist/js/bootstrap-checkbox.min.js" defer></script>

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

        .questionNoFontStyle {

           font-size: 35px;

        }

        .organNameoFontStyle {
             font-size: 30px;
        }
        
    </style>

    <script type="text/javascript">
       
        //Close CSNamePipe.exe before close the webpage.
        //$(window).unload(function () {
         //   document.getElementById('<%= FinishBtn.ClientID %>').click();
        //});
        /////////////////////////////////



        //detect user close webpage event,but it is also fired when you leave a site over a link or your browsers back button. 
        //Bind webpage default close cross button with "Finish" button to shot down CsNamedPipe.exe when user close IPC webpage.
        //window.addEventListener("beforeunload", function (e) {
        //    document.getElementById('<%= FinishBtn.ClientID %>').click();
        //});
        //window.onbeforeunload = function () {
        //    document.getElementById('<%= FinishBtn.ClientID %>').click();
        //};
        
        ///////////////////////////////////////////
        
        //resume scroll position after asp.net postback

        


        window.onload = function () {
            //resume scroll position after asp.net postback
            //divAnswerSheet.scrollTop = hdnScroll.value;


           
            //comment it to test why "Can't read"
            
            //recover visbility icon of hide_showAll
           
           
           
           
            

            ///////////////////////////////////////////////////
            
            /*
            else {
                iconHide_ShowAll.src = "Image/invisible.png";
            }
            */



            //suspects of can't read
            ///////////////////////////////////////////////////
            //resume mark icon in gridView
            
            var gvDrv = document.getElementById("<%= gvScore.ClientID %>");

            /*
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
            */
//////////////////////////////////////////////////////////////
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


        function checkPickedOrgans(questionArray) {

            var gvDrv = document.getElementById("<%= gvScore.ClientID %>");

            for (i = 0; i < questionArray.length; i++) {


                //check the organ that has been chosen as part of question.
                $("#" + gvDrv.rows[questionArray[i]].cells[2].getElementsByTagName("input")[0].id ).prop('checked', true);

            }
        }
        function readInExistingQuestion() {
            $.ajax({
                type: "GET",
                url: XMLFolder + questionXMLPath,
                dataType: "xml",
                success: function (xml) {

                    var questionArray = []

                    $(xml).find('Organ').each(function () {

                        $title = $(this).find("Question");

                        var val = $title.text();

                        if (val == "Yes") {
                            $no = $(this).find("Number");

                            //push to questionArray
                            questionArray.push($no.text());
                            //alert($no.text());

                        }




                    });
                    console.log(questionArray);
                    checkPickedOrgans(questionArray);
                }

            });
        }

    </script>


   
    <div align="center">
       
        <div class="jumbotron">
            <%--        <asp:Button ID="StartIPC" OnClick="StartIPC_Click" Text="開始" runat="server" />
        <asp:Button ID="Button1" OnClick="Button1_Click" Text="傳遞參數" runat="server" />--%>
            <div class="container">
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Button ID="FinishBtn" CssClass='btn-info btn-lg' OnClick="FinishBtn_Click" Text="Finish" runat="server" />
                    </div>
                    <div class="col-sm-6">

                      

                        <label class="radioBtnContainer">Surgery Mode
                          <input type="radio"  name="radioBtn_AITypeQuestionMode" value="Surgery Mode" checked="checked">
                          <span class="radioBtnCheckmark"></span>
                        </label>
                        <label class="radioBtnContainer">Anatomy Mode
                          <input type="radio" name="radioBtn_AITypeQuestionMode" value="Anatomy Mode">
                          <span class="radioBtnCheckmark"></span>
                        </label>
                      

                           
                      
                    </div>
                </div>
                <%--<input type="text" id="TBX_Input" runat="server" />--%>
                <%--<input type="button" onclick="" ID="StartRemoteApp"  Text="開始RemoteAPP" runat="server" />--%>
            </div>
        </div>
        


        <div class="row">
            
            <asp:Panel ID="scorePanel" runat="server" Width="90%" HorizontalAlign="Center">
                <div>
                    
            
            </div>
            <div>

               
                
              
            </div>
                <asp:GridView CssClass="table  table-condensed table-bordered table-hover table-responsive " ID="gvScore" runat="server" ShowHeaderWhenEmpty="true" OnRowCommand="gvScore_RowCommand">
                  
                    <Columns>

                        <asp:TemplateField HeaderText="Organ Number">
                            <ItemTemplate>
                                <asp:Label ID="TextBox_Number" CssClass="questionNoFontStyle" CliendIDMode="static" name="TextBox3" Visible="true" runat="server" Text='<%# Eval("Number") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ControlStyle-Width="80%" ControlStyle-Height="40px" HeaderText="Organ Name">
                            <ItemTemplate>
                              
                                <asp:Label ID="LBTextBox_OrganName"  CssClass="organNameoFontStyle" runat="server" Text='<%# Eval("Name") %>' />
                                

                            </ItemTemplate>

                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Set it as a question?">
                            <ItemTemplate>
                                 <asp:CheckBox runat="server" ID="checkbox_pickedOrgan" />

                             
                            </ItemTemplate>
                        </asp:TemplateField>



                        
                        

                        

                    </Columns>
                </asp:GridView>
                  <input type="hidden" id="hidden_AITypeQuestionTitle"  runat="server" >
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
        /*Temporary hard-code variable*/
        //In the near future ,we will get the questionXMLPath from URL para or other para transmission method.
        XMLFolder = "IPC_Questions/1161-1450/";
        questionXMLPath = "SceneFile_Q1.xml";
        /*Temporary hard-code variable*/


        //"../Mirac3DBuilder/HintsAccounts/Student/Mirac/1161-1450/SceneFile_Q1.xml";

        //to extract para in URL
        var url = new URL(window.location.href);
       

        $(document).ready(function () {
            $(':checkbox').checkboxpicker();

          
            //load XML to check the organs that are picked to be part of question.
            if (url.searchParams.get("viewContent") != null && url.searchParams.get("viewContent")  == "Yes")
            {
               
                //load XML to check the organs that are picked to be part of question.
                
                readInExistingQuestion();
                
            }
            

            if (url.searchParams.get("strQID")  != null)
            {
                //load XML to check the organs that are picked to be part of question.
                var hidden_AITypeQuestionTitle = document.getElementById("<%= hidden_AITypeQuestionTitle.ClientID %>");
                hidden_AITypeQuestionTitle.value = url.searchParams.get("strQID");

            }

          

        });
       
     </script>

</asp:Content>
