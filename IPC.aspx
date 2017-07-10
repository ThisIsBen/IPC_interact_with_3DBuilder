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

        .submit_img {
            width: 100%;
            visibility: hidden;
        }
        .menu_img {
            width: 100%;
           
        }
        .hideIfNotQuestion {
           visibility: hidden;
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

        var hdnScroll = document.getElementById("<%= hdnScrollPos.ClientID %>");


        window.onload = function () {
            //resume scroll position after asp.net postback
            //divAnswerSheet.scrollTop = hdnScroll.value;


           
            //comment it to test why "Can't read"
            
            //recover visbility icon of hide_showAll
           
            var hfHide_ShowAll = document.getElementById("<%= AllInOrVisible.ClientID %>");


            //suspects of can't read
            ///////////////////////////////////////////////////////////
            var iconHide_ShowAll = document.getElementById("<%= ShowOrHideAll.ClientID %>");
           
           
            if (hfHide_ShowAll.value == "true") {
                iconHide_ShowAll.src= "Image/visible.png";
            }


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
            
            for (i = 1; i < gvDrv.rows.length; i++) {
           
                
                
                //recover visbility icon of each row
                if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[2].value == "true") {
                    gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = "Image/visible.png";
                }
                else {
                    gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = "Image/invisible.png";
                }
                
               


            }
            

            for (i = 1; i < gvDrv.rows.length; i++)
            {
                //recover mark icon of each row
                if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[3] != null) {
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



        function showTBOfQuestionOrgans(inExamMode) {
            //console.log(inExamMode);
            //console.log(questionArray);
            var gvDrv = document.getElementById("<%= gvScore.ClientID %>");

           

            for (i = 0; i < questionArray.length; i++) {

                showTBItems = questionArray[i];
                if (inExamMode)//if it's in exam mode,do showTBItems = i
                    showTBItems = i+1;
                
                    

                //show the textbox of the organs that have been chosen as part of question.
                gvDrv.rows[showTBItems].cells[1].getElementsByTagName("input")[0].style.visibility = 'visible';
                //show the submit button of the organs that have been chosen as part of question.
                gvDrv.rows[showTBItems].cells[2].getElementsByTagName("input")[0].style.visibility = 'visible';

                //show the markicona of the organs that have been chosen as part of question.
                gvDrv.rows[showTBItems].cells[4].getElementsByTagName("input")[0].style.visibility = 'visible';
                gvDrv.rows[showTBItems].cells[4].getElementsByTagName("input")[1].style.visibility = 'visible';
            }
        }

        function rearrangeQNo() {
            var gvDrv = document.getElementById("<%= gvScore.ClientID %>");

           

            for (i = 1; i < gvDrv.rows.length; i++) {

             
               
                //show the textbox of the organs that have been chosen as part of question.
               
               
                gvDrv.rows[i].cells[0].getElementsByTagName("span")[0].innerHTML = i;
            }
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
        


        <div class="row">
            
            <asp:Panel ID="scorePanel" runat="server" Width="60%" HorizontalAlign="Center">
                <div>
                <label>顯示/隱藏全部</label>
            </div>
            <div>

                <input type="image" id="ShowOrHideAll" src="~/Image/visible.png" runat="server" onserverclick="ShowOrHideAll_Click"/>
               
                
                <asp:HiddenField ID="AllInOrVisible" runat="server" Value="true" />
            </div>
                <asp:GridView CssClass="table  table-condensed table-bordered table-hover table-responsive " ID="gvScore" runat="server" ShowHeaderWhenEmpty="true" OnRowCommand="gvScore_RowCommand">
                  
                    <Columns>

                        <asp:TemplateField HeaderText="號碼">
                            <ItemTemplate>
                                <asp:Label ID="TextBox_Number" CliendIDMode="static" name="TextBox3" Visible="true" runat="server" Text='<%# Eval("Number") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ControlStyle-Width="80%" ControlStyle-Height="40px" HeaderText="名稱">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox_Text" ClientIDMode="static" CssClass=" hideIfNotQuestion" runat="server" Text="" />
                                <asp:HiddenField ID="TextBox_Answer" runat="server" Value='<%# Eval("Name") %>' />
                                <asp:HiddenField ID="InOrVisible" runat="server" Value="true" />
                                <asp:HiddenField ID="markRecord" runat="server" Value="0" />
                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:ButtonField ButtonType="Image" CommandName="Submit"  ImageUrl="~/Image/checkmark2.png" ControlStyle-Height="40px" HeaderText="確認">
                            <ControlStyle CssClass=" submit_img" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Image" CommandName="InvisibleAndVisible"  ImageUrl="" ControlStyle-Height="40px" HeaderText="顯示/隱藏">
                            <ControlStyle CssClass=" menu_img" />
                        </asp:ButtonField>

                        <asp:TemplateField HeaderText="記號<br/>不確定/不會">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnMark" runat="server" CssClass="img-thumbnail hideIfNotQuestion"  ImageUrl="image/notSure.png" OnClientClick="return toNotSureIcon(this) " ControlStyle-Height="40px" />
                                <asp:ImageButton ID="btnMarkGiveUp" runat="server" CssClass="img-thumbnail hideIfNotQuestion"  ImageUrl="image/giveUp.png" OnClientClick="return toGiveUpIcon(this)" ControlStyle-Height="40px" />

                            </ItemTemplate>


                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </div>

    <script>
        var questionArray = []

        //In the near future ,we will get the questionXMLPath from URL para or other para transmission method.
        XMLFolder = "IPC_Questions/";
        questionXMLPath = "SceneFile_Q1.xml";
        

        //to extract para in URL
        $.urlParam = function (name) {
            var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
            if (!results)
                return 0;
                
            else
                return results[1];
        }
        //"../Mirac3DBuilder/HintsAccounts/Student/Mirac/1161-1450/SceneFile_Q1.xml";





        //swap the elements of index x and y in an array.
        function swapElement(questionList, x, y) {
            var b = questionList[y];
            questionList[y] = questionList[x];
            questionList[x] = b;
        }


        $(document).ready(function () {
            $.ajax({
                type: "GET",
                url: XMLFolder + questionXMLPath,
                dataType: "xml",
                success: function (xml) {

                    




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

                    //activate exam mode

                    if ($.urlParam('examMode') == "Yes") {

                        //對調IPC table rows ,改tableID ,rowID與pickedRandQNo 與questionList :1~n即可符合使用

                        //give tr unique id
                        tableID = "MainContent_gvScore"
                        rowID = "tableRowID"
                        var tableTr = $("#" + tableID + " tr");
                        //var countTr = tableTr.size();
                        var countTr = document.getElementById(tableID).rows.length;
                        //alert(countTr);
                        for (i = 0; i < countTr; i++) {

                            tableTr.eq(i).attr('id', 'tableRowID' + i);

                        }




                        //generate the exam question number according to pickedRandQNo,and swap the corresponding row in the table at the same time.
                        
                        //replace pickedRandQNo with the random numbers generated by Peter's function.
                        //this sequence is the only thing that needs to be sent to 3DBuilder.
                        var pickedRandQNo = [5, 3, 1];

                        var questionList = [];
                        for (i = 0; i < countTr; i++) {
                            questionList[i] = i + 1;
                        }

                        var $elem1;
                        var $elem2;
                        var $placeholder = $("<tr><td></td></tr>");
                        var swapTarget;
                        for (i = 0; i < pickedRandQNo.length; i++) {


                            //swap swapTarget and i can get the ideal random question number array.

                            //swap keypoint1 for 3DBuilder
                            swapTarget = questionList.indexOf(pickedRandQNo[i]);


                            //test
                            console.log(questionList[swapTarget]);
                            console.log(questionList[i]);
                            console.log("\n");

                            
                            $elem1 = $("#" + rowID + questionList[swapTarget]);
                            $elem2 = $("#" + rowID + questionList[i]);
                            $elem2.after($placeholder);
                            $elem1.after($elem2);
                            $placeholder.replaceWith($elem1);



                            //swap keypoint2 for 3DBuilder
                            swapElement(questionList, swapTarget, i);

                        }
                        


                        //rearrange Question numbers in ascending order to keep Question numbers being arranged as ususal order.
                        rearrangeQNo();


                        //show the TextBox of the question Organs.
                        examMode = true;
                        showTBOfQuestionOrgans(examMode);



                    }




                    if ($.urlParam('examMode') == 0)
                    {

                        //show the TextBox of the question Organs.
                        examMode = false;
                        showTBOfQuestionOrgans(examMode);
                    }
                }

            });



            
           

        });
    </script>
</asp:Content>
