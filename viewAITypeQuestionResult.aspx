﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeFile="viewAITypeQuestionResult.aspx.cs" Inherits="IPC" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--Include Bootstrap -->
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

    <style type="text/css">
        th {
            font-size: 20px;
            text-align: center;
            background-color: rgb(215, 111, 241);
        }

        .table-hover tbody tr:hover td, .table-hover tbody tr:hover th {
            background-color: rgb(79, 187, 219);
        }


        .submit_img {
            width: 50%;
            visibility: hidden;
        }

        .showHideIcon_img {
            width: 50%;
        }

        .hideIfNotQuestion {
            visibility: hidden;
            font-size: 30px;
        }

        .nonQuestionTR {
            background-color: gray;
        }

        .questionNoFontStyle {
            font-size: 35px;
        }
    </style>
    <link href="Content/CountDownTimer.css" rel="stylesheet" />


    <script type="text/javascript">

        //if (a == 0) {; }

        //img source gallery 
        visibleImg = "Image/visible.png";
        invisibleImg = "Image/invisible.png";
        visible_disabledByTeacher = "image/visible_disabledByTeacher.png";
        invisible_disabledByTeacher = "image/invisible_disabledByTeacher.png";
        notSureImg = "image/notSure.png";
        giveUpImg = "image/giveUp.png";
        notSureClickImg = "image/notSureClick.png";
        giveUpClickImg = "image/giveUpClick.png";
        organSubmitImg = "Image/checkmark2.png";
        clickedOrganSubmitImg = "Image/checkmark2_marked.png";


        //incorrect answer <tr> background Color
        incorrectAnswerTRBgColor = "rgb(255, 127, 127)";

        //to decide the background color of the question TR.
        questionTRBgColor = "rgb(145, 201, 27)";

        //Close CSNamePipe.exe before close the webpage.
        //$(window).unload(function () {
        //   document.getElementById('<%= btnBack.ClientID %>').click();
        //});
        /////////////////////////////////



        //detect user close webpage event,but it is also fired when you leave a site over a link or your browsers back button. 
        //Bind webpage default close cross button with "Finish" button to shot down CsNamedPipe.exe when user close IPC webpage.
        //window.addEventListener("beforeunload", function (e) {
        //    document.getElementById('<%= btnBack.ClientID %>').click();
        //});
        //window.onbeforeunload = function () {
        //    document.getElementById('<%= btnBack.ClientID %>').click();
        //};

        ///////////////////////////////////////////

        //resume scroll position after asp.net postback




        //window.onload = onloadFun;

        function onloadFun() {


            //load();


            //resume scroll position after asp.net postback
            //divAnswerSheet.scrollTop = hdnScroll.value;




            //20180827 If there is no need to hide all the 3D organs,we can delete the following part and set the image src of the "#ShowOrHideAll" button 
            //20180827 the so-called following part start from here
            //recover visbility icon of hide_showAll


            var hfHide_ShowAll = document.getElementById("<%= hidden_AllInOrVisible.ClientID %>");




            ///////////////////////////////////////////////////////////
            var iconHide_ShowAll = document.getElementById("<%= ShowOrHideAll.ClientID %>");


            if (hfHide_ShowAll.value == "true") {
                iconHide_ShowAll.src = visibleImg;
            }


            ///////////////////////////////////////////////////

            /*
            else {
                iconHide_ShowAll.src =invisibleImg ;
            }
            */



            //suspects of can't read
            ///////////////////////////////////////////////////

            //keep submit btn visible at all times
            $('#ShowOrHideAll').attr('src', visibleImg);
            //20180827 end

            //assign the image of the hide non question <tr> btn
            $('#HideNonQuestionTR').innerHTML = '<img src="' + visibleImg + '" />';

            //resume mark icon in gridView
            var gvDrv = document.getElementById("<%= gvScore.ClientID %>");

            for (i = 1; i < gvDrv.rows.length; i++) {



                //recover visbility icon of each row

                //keep submit btn visible at all times
                gvDrv.rows[i].cells[2].getElementsByTagName("span")[0].innerHTML = " ";



                //keep Mark icons visible at all times

                //2019/4/11 Ben moves Show/Hide column to the last column
                /*
                gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = notSureImg;
                gvDrv.rows[i].cells[4].getElementsByTagName("input")[1].src = giveUpImg;
                */

                //gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = notSureImg;
                //gvDrv.rows[i].cells[3].getElementsByTagName("input")[1].src = giveUpImg;


                //if the currently traversed organ was visible,then keep it visible

                //2019/4/11 Ben moves Show/Hide column to the last column
                /*
                if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[1].value == "true") {
                    gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = visibleImg;
                }


                //set all the disableByTeacher icon to be invisibleImg and disabled.
                else if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[1].value == "disableByTeacher") {
                    gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = invisible_disabledByTeacher;
                    gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].disabled = true;


                }

                //if the currently traversed organ was inivisible,then keep it inivisible
                else {

                    gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = invisibleImg;
                }
                */
                //check if the Show/HideCol is currently displayed or hidden
                if (document.getElementById('<%= hidden_DisplayShow_HideCol.ClientID %>').value == "Yes") {


                    if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[1].value == "true") {
                        gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = visibleImg;
                    }


                        //set all the disableByTeacher icon to be invisibleImg and disabled.
                    else if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[1].value == "disableByTeacher") {
                        gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = invisible_disabledByTeacher;
                        gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].disabled = true;


                    }

                        //if the currently traversed organ was inivisible,then keep it inivisible
                    else {

                        gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = invisibleImg;
                    }

                }
            }


            //if the AITypeQuestion is of Surgery Mode, we set the hide/show icon of the skin to be visible and disabled.
            if (sessionStorage.getItem("AIQMode") == "Surgery Mode") {

                skinOrganNumber = sessionStorage.getItem("skinOrganNumber");

                //2019/4/11 Ben moves Show/Hide column to the last column
                /*
                gvDrv.rows[skinOrganNumber].cells[3].getElementsByTagName("input")[0].src = visible_disabledByTeacher;
                gvDrv.rows[skinOrganNumber].cells[3].getElementsByTagName("input")[0].disabled = true;
                */


                //check if the Show/HideCol is currently displayed or hidden
                if (document.getElementById('<%= hidden_DisplayShow_HideCol.ClientID %>').value == "Yes") {

                    gvDrv.rows[skinOrganNumber].cells[4].getElementsByTagName("input")[0].src = visible_disabledByTeacher;
                    gvDrv.rows[skinOrganNumber].cells[4].getElementsByTagName("input")[0].disabled = true;

                }
            }



            //Step2-2 Display the studen’ts score of each question organ 
            displayStuScoreEachQuestionOrgan(gvDrv);


            //recover the click status of the NotSure/GiveUp icon after coming back from UpdatePanel AJAX
            //recoverNotSure_GiveUpIconStatus(gvDrv);





            //////////////////////////////////////////////////////////////
        }


        //mark the OrganSubmitBtn in the GridView to let the student know he has submitted the answer of the organ. 
        function displayStuScoreEachQuestionOrgan(gvDrv) {



            /*
            //retrieve the lately clicked OrganSubmitBtn Number from the sessionStorage
            //this OrganSubmitBtn Number is stored before the UpdatePanel updates the content via AJAX
            clickedOrganSubmitBtnNo = parseInt(sessionStorage.getItem('clickedOrganSubmitBtnNo'));

            if (!isNaN(clickedOrganSubmitBtnNo)) {
                //mark the OrganSubmitBtn in the GridView to let the student know he has submitted the answer of the organ.
                gvDrv.rows[clickedOrganSubmitBtnNo].cells[1].getElementsByTagName("input")[3].value = 1;


                //change the icon of the OrganSubmitBtn to let the student know he has submitted the answer of the organ.
                for (i = 1; i < gvDrv.rows.length; i++) {


                    if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[3].value == "1") {

                        gvDrv.rows[i].cells[2].getElementsByTagName("span")[0].innerHTML = "submitted";

                    }



                }


            }
            */
            

            var studentEachQuestionOrganScoreString = ('<%= convertCsArray2JSONFormat( ScoreAnalysisList[0].Grade[0]) %>')

            //parse the JSON string received from backend to a Javascript object
            //here, it will be converted from "["ab","cd"]" to ab,cd 
            studentEachQuestionOrganScoreString = JSON.parse(studentEachQuestionOrganScoreString);

            //iteration starts from index=1 to avoid getting the filePath of the AITypeQuestion
            //e.g., YYY.xml,organ1,organ2->organ1,organ2


            //The 配分 of each question organ
            scoreOfEachQuestionOrgan = studentEachQuestionOrganScoreString[0] / (studentEachQuestionOrganScoreString.length - 1);

            for (i = 1; i < studentEachQuestionOrganScoreString.length; i++) {

               

                gvDrv.rows[i].cells[2].getElementsByTagName("span")[0].innerHTML = studentEachQuestionOrganScoreString[i] + "/" + scoreOfEachQuestionOrgan;



                //if the student's answer is not correct, we set the background of the row to red
                if (studentEachQuestionOrganScoreString[i] != scoreOfEachQuestionOrgan) {

                    gvDrv.rows[i].style.backgroundColor = incorrectAnswerTRBgColor;

                    //Step 2-3 Display the correct answer of the question organ if the student didn’t answer it correctly.
                    displayCorrectQuestionOrganAnswer(i, gvDrv);

                    gvDrv.rows[i].cells[2].getElementsByTagName("span")[0].innerHTML = studentEachQuestionOrganScoreString[i] + "/" + scoreOfEachQuestionOrgan;



                }
               

            }

        }


        //Step 2-3 Display the correct answer of the question organ if the student didn’t answer it correctly.
        function displayCorrectQuestionOrganAnswer(rowIndex, gvDrv) {

            var correctAnswerOfTheAITypeQuestionJSArray = [];
            <% 
        
                            
        //var correctAnswerOfTheAITypeQuestion = RandomQuestionNoSession;

        //if RandomQuestionNoSession is not null
        if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
        {
            
             %>
                //display the title of the correct answer for Number Answer Mode
                var gvDrv = document.getElementById("<%= gvScore.ClientID %>");
                gvDrv.rows[0].getElementsByTagName("th")[3].innerHTML = "Correct<br/>Organ Name";

             <% 

                for (int i = 0; i < correctAnswerHT[0].Count; i++)
                {
                                
                         
             %>

                         correctAnswerOfTheAITypeQuestionJSArray.push('<%= correctAnswerHT[0][ScoreAnalysisList[0].questionOrderingString[i+1]] %>');

                        

             <% 
                }
                
                
         }


        else if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
        {
            
             %>
                //display the title of the correct answer for Number Answer Mode
                var gvDrv = document.getElementById("<%= gvScore.ClientID %>");
                gvDrv.rows[0].getElementsByTagName("th")[3].innerHTML = "Correct<br/>Corresponding 3D Organ<br/>Label Number Here:";


                for (i = 0; i < questionArray.length; i++)
                {
                  
                    correctAnswerOfTheAITypeQuestionJSArray.push(questionArray[i]);
            
                }

                
                
            <%
                 
        }
            %>




            gvDrv.rows[rowIndex].cells[3].getElementsByTagName("span")[0].innerHTML = correctAnswerOfTheAITypeQuestionJSArray[rowIndex - 1];

        }





        function recoverNotSure_GiveUpIconStatus(gvDrv) {
            for (i = 1; i < gvDrv.rows.length; i++) {

                //recover mark icon of each row
                if (gvDrv.rows[i].cells[1].getElementsByTagName("input")[2] != null) {
                    switch (parseInt(gvDrv.rows[i].cells[1].getElementsByTagName("input")[2].value)) {


                        //2019/4/11 Ben moves Show/Hide column to the last column
                        /*
                        case 0:
                            gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = notSureImg;
                            gvDrv.rows[i].cells[4].getElementsByTagName("input")[1].src = giveUpImg;
                            break;
                        case 1:
                            gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = notSureClickImg;
                            break;
                        case 2:
                            gvDrv.rows[i].cells[4].getElementsByTagName("input")[1].src = giveUpClickImg;
                            break;
                        */

                        case 0:
                            gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = notSureImg;
                            gvDrv.rows[i].cells[3].getElementsByTagName("input")[1].src = giveUpImg;
                            break;
                        case 1:
                            gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = notSureClickImg;
                            break;
                        case 2:
                            gvDrv.rows[i].cells[3].getElementsByTagName("input")[1].src = giveUpClickImg;
                            break;





                    }
                }
            }
        }


        //change mark icon to "not sure " icon and remove the mark when the mark has existed with single click.
        function toNotSureIcon(lnk) {
            //get the clicked row of TemplageField
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;



            //change " " to "not sure"icon or change"not sure"icon and "give up"Icon to " "

            //2019/4/11 Ben moves Show/Hide column to the last column
            /*
            var img = row.cells[4].getElementsByTagName("input")[0].src;
            var imgBesideIt = row.cells[4].getElementsByTagName("input")[1].src;
            
           

            if (img.indexOf(notSureClickImg) != -1) {
                row.cells[4].getElementsByTagName("input")[0].src = notSureImg;
                //record the current mark icon in the hidden field,0 means empty
                row.cells[1].getElementsByTagName("input")[2].value = 0;

               
            }
          

           
            else {
                
                if (imgBesideIt.indexOf(giveUpClickImg) != -1)
                {
                    row.cells[4].getElementsByTagName("input")[1].src = giveUpImg;
                }
                
                row.cells[4].getElementsByTagName("input")[0].src = notSureClickImg;
                
                //record the current mark icon in the hidden field,1 means notSure.png
                row.cells[1].getElementsByTagName("input")[2].value = 1;//bug
                
               

              
            }
            
           */
            var img = row.cells[3].getElementsByTagName("input")[0].src;
            var imgBesideIt = row.cells[3].getElementsByTagName("input")[1].src;



            if (img.indexOf(notSureClickImg) != -1) {
                row.cells[3].getElementsByTagName("input")[0].src = notSureImg;
                //record the current mark icon in the hidden field,0 means empty
                row.cells[1].getElementsByTagName("input")[2].value = 0;


            }



            else {

                if (imgBesideIt.indexOf(giveUpClickImg) != -1) {
                    row.cells[3].getElementsByTagName("input")[1].src = giveUpImg;
                }

                row.cells[3].getElementsByTagName("input")[0].src = notSureClickImg;

                //record the current mark icon in the hidden field,1 means notSure.png
                row.cells[1].getElementsByTagName("input")[2].value = 1;//bug




            }

            //to prevent refreshing of page when button inside form clicked
            //return false;
        }

        //change mark icon to "give up " icon with double click.
        function toGiveUpIcon(lnk) {
            //get the clicked row of TemplageField
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex;


            //change to "give up" icon

            //2019/4/11 Ben moves Show/Hide column to the last column
            /*
            var img = row.cells[4].getElementsByTagName("input")[1].src;
            var imgBesideIt = row.cells[4].getElementsByTagName("input")[0].src;
            if (img.indexOf(giveUpClickImg) != -1) {

                row.cells[4].getElementsByTagName("input")[1].src = giveUpImg;
                //record the current mark icon in the hidden field,0 means empty
                row.cells[1].getElementsByTagName("input")[2].value = 0;
            }
      
            else {
                if (imgBesideIt.indexOf(notSureClickImg) != -1)
                {
                    row.cells[4].getElementsByTagName("input")[0].src = notSureImg;
                }
                row.cells[4].getElementsByTagName("input")[1].src = giveUpClickImg;

                //record the current mark icon in the hidden field,1 means notSure.png
                row.cells[1].getElementsByTagName("input")[2].value = 2;
            }
            */
            var img = row.cells[3].getElementsByTagName("input")[1].src;
            var imgBesideIt = row.cells[3].getElementsByTagName("input")[0].src;
            if (img.indexOf(giveUpClickImg) != -1) {

                row.cells[3].getElementsByTagName("input")[1].src = giveUpImg;
                //record the current mark icon in the hidden field,0 means empty
                row.cells[1].getElementsByTagName("input")[2].value = 0;
            }

            else {
                if (imgBesideIt.indexOf(notSureClickImg) != -1) {
                    row.cells[3].getElementsByTagName("input")[0].src = notSureImg;
                }
                row.cells[3].getElementsByTagName("input")[1].src = giveUpClickImg;

                //record the current mark icon in the hidden field,1 means notSure.png
                row.cells[1].getElementsByTagName("input")[2].value = 2;
            }

            //to prevent refreshing of page when button inside form clicked
            return false;
        }

        //store the Picked Organ Questions in a hidden field so that we can access it on backend
        function goBackToPreviousPage() {

            
            //clear sessionStorage
            sessionStorage.clear();

            //go back to the previous page
            window.history.back();
        }


        function showTBOfQuestionOrgans(inExamMode) {
            //console.log(inExamMode);
            //console.log(questionArray);



            var gvDrv = document.getElementById("<%= gvScore.ClientID %>");



                for (i = 0; i < questionArray.length; i++) {

                    showTBItems = questionArray[i];
                    if (inExamMode)//if it's in exam mode,do showTBItems = i
                        showTBItems = i + 1;

                    //change the background color of the organs that have been chosen as part of question.
                    gvDrv.rows[showTBItems].style.backgroundColor = questionTRBgColor;

                    //Step 2-1 apply the retrieved student’s question organ number to display the student’s answer in this order.


                    var studentAnswerString = ('<%= convertCsArray2JSONFormat( ScoreAnalysisList[0].studentAnswerString) %>')

                    //parse the JSON string received from backend to a Javascript object
                    //here, it will be converted from "["ab","cd"]" to ab,cd 
                    studentAnswerString = JSON.parse(studentAnswerString);

                    //iteration starts from index=1 to avoid getting the filePath of the AITypeQuestion
                    //e.g., YYY.xml,organ1,organ2->organ1,organ2


                    gvDrv.rows[showTBItems].cells[1].getElementsByTagName("input")[0].value = studentAnswerString[i + 1];



                    //show the textbox of the organs that have been chosen as part of question.
                    gvDrv.rows[showTBItems].cells[1].getElementsByTagName("input")[0].style.visibility = 'visible';
                    //show the submit button of the organs that have been chosen as part of question.
                    gvDrv.rows[showTBItems].cells[2].getElementsByTagName("span")[0].style.visibility = 'visible';


                    //show the markicona of the organs that have been chosen as part of question.
                    /*
                    gvDrv.rows[showTBItems].cells[4].getElementsByTagName("input")[0].style.visibility = 'visible';
                    gvDrv.rows[showTBItems].cells[4].getElementsByTagName("input")[1].style.visibility = 'visible';
                    */

                    //2019/4/11 Ben moves Show/Hide column to the last column
                    gvDrv.rows[showTBItems].cells[3].getElementsByTagName("span")[0].style.visibility = 'visible';
                    //gvDrv.rows[showTBItems].cells[3].getElementsByTagName("input")[1].style.visibility = 'visible';

                }

                //assign a class to the non question TRs
                assignClass2NonQuestionTR();


            }

            function assignClass2NonQuestionTR() {

                var gvDrv = document.getElementById("<%= gvScore.ClientID %>");

                //assign a common class to all the non question TRs for the further control.
                for (i = 1; i < gvDrv.rows.length; i++) {
                    if (gvDrv.rows[i].style.backgroundColor != questionTRBgColor) {
                        var nonQuestionRow = gvDrv.rows[i];
                        nonQuestionRow.classList.add('nonQuestionTR');

                        //display the reminder on the Non Question Row to inform the student that this organ is not a question. 
                        displayReminder_NonQuestionRow(nonQuestionRow);
                    }



                }


            }

            //display the reminder on the Non Question Row to inform the student that this organ is not a question. 
            function displayReminder_NonQuestionRow(nonQuestionRow) {
                var nonQuestionRowTextBox = nonQuestionRow.cells[1].getElementsByTagName("input")[0];
                nonQuestionRowTextBox.value = "Non Answer Row";
                nonQuestionRowTextBox.disabled = true;
                nonQuestionRowTextBox.style.backgroundColor = "gray";
                nonQuestionRowTextBox.style.color = "white";
                nonQuestionRowTextBox.style.borderStyle = "none"
                nonQuestionRowTextBox.style.visibility = 'visible';

            }



            function rearrangeQNo() {
                if (("<%= NameOrNumberAnsweringMode_Session %>") == "Name Answering Mode") {

                    var gvDrv = document.getElementById("<%= gvScore.ClientID %>");



                  for (i = 1; i < gvDrv.rows.length; i++) {



                      //show the textbox of the organs that have been chosen as part of question.


                      gvDrv.rows[i].cells[0].getElementsByTagName("span")[0].innerHTML = i;
                  }
              }


              else if (("<%= NameOrNumberAnsweringMode_Session %>") == "Number Answering Mode") {

                  //do rearrangement  for Number Answering Mode
                  //currently we don't need to do anything.
              }

            }

            //clear the ajax cache to read the latest content from the organ xml file
            function clearAjaxCache() {
                $.ajaxSetup({
                    cache: false
                });
            }
    </script>



    <div>
        <div class="row">

            <asp:Panel ID="scorePanel" runat="server" Width="100%" HorizontalAlign="Center">

                <div>






                    <input type="hidden" id="hidden_AllInOrVisible" runat="server" value="true">

                    <input type="hidden" id="hidden_HideNonQuestionTRS" value="false">

                    <input type="hidden" id="hidden_pickedQuestions" runat="server" value="false">


                    <input type="hidden" id="hidden_serverSideRemainingTimeSec" runat="server" value="0">
                </div>
                <div id="headerFunctionBar" class="container" style="position: fixed; background-color: lightblue;">
                    <div class="row">
                        <div class="col-md-8">
                        </div>
                        <div class="col-md-4">



                            <%--<div id="clockdiv">
  
                          <div>
                            <span class="hours"></span>
                            <div class="smalltext">Hours</div>
                          </div>
                          <div>
                            <span class="minutes"></span>
                            <div class="smalltext">Minutes</div>
                          </div>
                          <div>
                            <span class="seconds"></span>
                            <div class="smalltext">Seconds</div>
                          </div>
 

                        </div>--%>
                        </div>
                    </div>
                    <br />


                </div>

                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <%--to record Show/HideCol is currently displayed or hidden--%>
                        <input type="hidden" id="hidden_DisplayShow_HideCol" runat="server" value="Yes">

                        <%--        <asp:Button ID="StartIPC" OnClick="StartIPC_Click" Text="開始" runat="server" />
            <asp:Button ID="Button1" OnClick="Button1_Click" Text="傳遞參數" runat="server" />--%>
                        <div class="container" style="position: fixed;">
                            <div class="row text-left">
                                <div class="col-sm-4 ">
                                    <asp:Button ID="btnBack" CssClass='btn-danger btn-lg ' OnClick="btnBack_Click" OnClientClick="goBackToPreviousPage();" Text="<< Back" runat="server" />
                                    &nbsp&nbsp&nbsp&nbsp
                                    <input type="button" class='btn-info btn-lg ' id="HideNonQuestionTR" value="Hide/Show Non Answer Rows" onclick="hideNonQuestionTR()" />

                                    <%--<input id="Button1" type="button" class='btn-info btn-lg ' value="View organs in 3D" runat="server" onserverclick="displayShowHideIconCol_BtnClick">--%>
                                </div>
                                <div class="col-sm-4 ">
                                  
                                    <div class="row text-right">
                                        <div >
                                         
                                            <label style="display:none; font-size: 20px;" id="lb_showHiddenOrgans" runat="server">Show all hidden organs</label>
                                        </div>
                                        <div class="row text-right ">
                                            <div style="">
                                               
                                                 </br>
                                            <asp:Label ID="LB_StudentScore" CliendIDMode="static" CssClass='label label-success' Font-Size="20pt" Font-Names="TextBox3" Visible="true" runat="server" />
                                           
                                            </div>
                                      </div>
                                    </div>
                                    <br />

                                    <%-- <div>
                                        <label style="font-size: 20px;">Your total score:</label>

                                    </div>--%>
                                </div>

                               <div class="col-sm-1 text-left">
                                    <input type="image" id="ShowOrHideAll" class="img-thumbnail " src="" style="display: none; max-height: 60px; max-width: 60px;" onserverclick="ShowOrHideAll_Click" runat="server" />
                                      
                                   </div>

                                <div class="col-sm-3">
                                    <label style="font-size: 20px;">View 3D organs:</label>
                                   
                                    <input type="button" class="btn btn-info btn-md" id="btn_setUpCSNamedPipe" runat="server" value="Step1 Create IPC Pipe" onclick="askUserOpen3DBuilder();" onserverclick="btn_setUpCSNamedPipe_Onclick">

                                    <input type="button" class="btn btn-info btn-md" id="btn_connectTo3DBuilder" runat="server" value="Step2 Load 3D organs" onserverclick="btn_connectTo3DBuilder_Onclick">
                                </div>

                            </div>
                            <%--<input type="text" id="TBX_Input" runat="server" />--%>
                            <%--<input type="button" onclick="" ID="StartRemoteApp"  Text="開始RemoteAPP" runat="server" />--%>
                        </div>



                       
            </br>
            </br>
            </br>
            </br>
            </br>
            </br>
           
            

                        
                               
                          
                
                
                <asp:GridView CssClass="table  table-condensed table-bordered table-hover table-responsive " ID="gvScore" runat="server" ShowHeaderWhenEmpty="true" OnRowCommand="gvScore_RowCommand">

                    <Columns>
                        <%--cells[0]--%>
                        <asp:TemplateField ItemStyle-Width="40px" HeaderText="Question Indicator">
                            <ItemTemplate>
                                <asp:Label ID="TB_OrganIndicator" CliendIDMode="static" CssClass="questionNoFontStyle" Font-Names="TextBox3" Visible="true" runat="server" Text='<%# Eval("Number") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--cells[1]--%>
                        <asp:TemplateField ControlStyle-Width="100%" ControlStyle-Height="40px" HeaderText="Your Answer">
                            <ItemTemplate>
                                <asp:TextBox ID="TB_AnsweringField" ItemStyle-Width="130%" ClientIDMode="static" CssClass=" hideIfNotQuestion" runat="server" Text="" />

                                <%--show the corresponding correct organ name for debugging purpose--%>
                                <%-- <asp:HiddenField ID="TextBox_Answer" runat="server" Value='<%# Eval("Name") %>' />--%>
                                <%--show the corresponding correct organ name for debugging purpose--%>

                                <asp:HiddenField ID="InOrVisible" runat="server" Value="true" />
                                <%-- <asp:HiddenField ID="markRecord" runat="server" Value="0" />--%>
                                <%-- <input type="hidden" id="InOrVisible" runat="server" value="true">--%>
                                <input type="hidden" id="markRecord" runat="server" value="0">

                                <input type="hidden" id="clickedOrganSubmitBtn" runat="server" value="99">
                            </ItemTemplate>

                        </asp:TemplateField>


                        <%--- 
                        <asp:ButtonField ButtonType="Image" CommandName="Submit" ImageUrl="" ControlStyle-Height="40px" HeaderText="Point you got">
                            <ControlStyle CssClass=" submit_img" />
                        </asp:ButtonField>
                       ---%>
                        <%--cells[2]--%>
                        <asp:TemplateField ItemStyle-Width="40px" HeaderText="Point you got">
                            <ItemTemplate>
                                <asp:Label ID="TextBox_StudentScorePerQuestion" CliendIDMode="static" CssClass="questionNoFontStyle" Font-Names="TextBox3" Visible="true" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--cells[3]--%>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Label ID="TextBox_CorrectOrganAnswer" CliendIDMode="static" CssClass="questionNoFontStyle" Font-Names="TextBox3" Visible="true" runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>

                        <%--cells[4]--%>
                        <%--<asp:ButtonField ButtonType="Image" CommandName="InvisibleAndVisible" ImageUrl="" ControlStyle-Height="40px" ControlStyle-Width="40px" HeaderText="Show /<br/>Hide" Visible='<%# ShowHideIconCol_displayStatus() %>'>--%>
                        <asp:ButtonField ButtonType="Image" CommandName="InvisibleAndVisible" ImageUrl="" ControlStyle-Height="40px" ControlStyle-Width="40px" HeaderText="Show /<br/>Hide " Visible="true">
                            <ControlStyle CssClass=" showHideIcon_img" />
                        </asp:ButtonField>


                    </Columns>
                </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </div>

    <script>
        //to keep all the organ numbers that are assigned as questions 
        var questionArray = []



        //In the near future ,we will get the questionXMLPath from URL para or other para transmission method.
        XMLFolder = "<%=  CsDynamicConstants.relativeKneeXMLFolder %>";//"IPC_Questions/1161-1450/";



        //to extract para in URL
        var url = new URL(window.location.href);
        //"../Mirac3DBuilder/HintsAccounts/Student/Mirac/1161-1450/SceneFile_Q1.xml";

        $(document).ready(function () {

            //set the default value of each parameters that are retrieved from URL.
            questionXMLPath = "tea1_Q_20190205145709";//anatomy 
            //questionXMLPath = "tea1_Q_20181210231100";//surgery
            examMode = "Yes";



            //get the QID from URL parameter as the target file name if the instructor wants to check the contnet of the existing AI Type question.
            if (url.searchParams.get("strQID") != "" && url.searchParams.get("strQID") != null) {
                questionXMLPath = url.searchParams.get("strQID") + ".xml";

            }
            else {
                questionXMLPath = questionXMLPath + ".xml";
            }

            //get the examMode from URL parameter if it exists.
            if (url.searchParams.get("examMode") != "" && url.searchParams.get("examMode") != null) {
                examMode = url.searchParams.get("examMode");
            }


            //clear the ajax cache to read the latest content from the organ xml file
            //if we don't clear the cache, ajax will return an old version of the organ xml file cached in the memory.
            clearAjaxCache();


            //read the organ questiones picked by the instructor from the organ XML file. 
            $.ajax({
                type: "GET",
                url: XMLFolder + questionXMLPath,
                dataType: "xml",
                success: function (xml) {

                    //resume mark icon in gridView
                    var gvDrv = document.getElementById("<%= gvScore.ClientID %>");
                    //record the organ number of the Skin in this AITypeQuestion.
                    var skinOrganNumberVal = 0;


                    //access each <Organ>
                    $(xml).find('Organ').each(function () {
                        //access each <Question> in the <Organ>
                        $isQuestion = $(this).find("Question");

                        //access the value of each <Question> in the <Organ>
                        var isQuestionVal = $isQuestion.text();

                        if (isQuestionVal == "Yes") {
                            $questionNo = $(this).find("Number");

                            //push to questionArray
                            questionArray.push($questionNo.text());
                            //alert($no.text());

                        }


                        //gvDrv.rows[i].cells[1].getElementsByTagName("input")[1].value == "true"
                        //access each <Visible> in the <Organ>                       
                        $isVisible = $(this).find("Visible");

                        //access the value of each <Question> in the <Organ>
                        var isVisibleVal = $isVisible.text();

                        //we set the icon of the organs that are hidden by the teacher to inivisible
                        //and disable the icon of these organs to prevent the student from trying to show the organs hidden by the teacher.
                        if (isVisibleVal == "0") {



                            var i = $(this).find("Number").text();
                            gvDrv.rows[i].cells[1].getElementsByTagName("input")[1].value = "disableByTeacher";

                            //2019/4/11 Ben moves Show/Hide column to the last column
                            /*
                            gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].src = invisible_disabledByTeacher;
                            gvDrv.rows[i].cells[3].getElementsByTagName("input")[0].disabled = true;
                            */

                            //check if the Show/HideCol is currently displayed or hidden
                            if (document.getElementById('<%= hidden_DisplayShow_HideCol.ClientID %>').value == "Yes") {

                                gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].src = invisible_disabledByTeacher;
                                gvDrv.rows[i].cells[4].getElementsByTagName("input")[0].disabled = true;
                            }

                        }


                        //record the question number of the 'Skin' in this AITypeQuestion
                        $organName = $(this).find("Name");

                        //access the value of each <Question> in the <Organ>
                        var organNameVal = $organName.text();

                        if (organNameVal == "Skin") {
                            $skinOrganNumber = $(this).find("Number");

                            skinOrganNumberVal = $skinOrganNumber.text();
                        }


                    });

                    //access the value of  <AITypeQuestionMode> of this AITypeQuestion in the xml file.
                    $AITypeQuestionMode = $(xml).find('AIQMode')

                    //access the value of each <Question> in the <Organ>
                    var AITypeQuestionModeVal = $AITypeQuestionMode.text();
                    if (AITypeQuestionModeVal == "Surgery Mode") {

                        //disable the hide/show button of the Skin 
                        //when the AITypeQuestion is of Surgery Mode, 
                        //which the Skin should not be hidden by the student.                                            
                        gvDrv.rows[skinOrganNumberVal].cells[1].getElementsByTagName("input")[1].value = "disableByTeacher";

                        //2019/4/11 Ben moves Show/Hide column to the last column
                        /*
                        gvDrv.rows[skinOrganNumberVal].cells[3].getElementsByTagName("input")[0].src = giveUpImg;
                        gvDrv.rows[skinOrganNumberVal].cells[3].getElementsByTagName("input")[0].disabled = true;
                        */
                        //check if the Show/HideCol is currently displayed or hidden
                        if (document.getElementById('<%= hidden_DisplayShow_HideCol.ClientID %>').value == "Yes") {

                            gvDrv.rows[skinOrganNumberVal].cells[4].getElementsByTagName("input")[0].src = giveUpImg;
                            gvDrv.rows[skinOrganNumberVal].cells[4].getElementsByTagName("input")[0].disabled = true;
                        }


                        //store the mode of the AITypeQuestion in a session storage
                        sessionStorage.setItem("AIQMode", "Surgery Mode");

                        //record the organ number of the Skin in this AITypeQuestion.
                        sessionStorage.setItem("skinOrganNumber", skinOrganNumberVal);
                    }

                    else if (AITypeQuestionModeVal == "Anatomy Mode") {
                        //store the mode of the AITypeQuestion in a session storage
                        sessionStorage.setItem("AIQMode", "Anatomy Mode");
                    }



                    //activate exam mode

                    //2019/4/9 Ben commented for using default URL value if no paras provided.
                    //if (url.searchParams.get("examMode") == "Yes") {
                    if (examMode == "Yes") {

                        //generate the exam question number according to pickedRandQNo,and swap the corresponding row in the table at the same time.


                        //get the randomized  Question Numbers picked by instructor to IPC.aspx sent through Session
                        var pickedRandQNo = [];
                        <% 
     
        if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
        {




            var randomizedQuestionOrganNumber = RandomQuestionNoSession;
                                    //if RandomQuestionNoSession is not null
            if (randomizedQuestionOrganNumber[0] != -1)
                for (int i = 0; i < randomizedQuestionOrganNumber.Length; i++)
                    {
                                
                         
                        %>

                        pickedRandQNo.push('<%= randomizedQuestionOrganNumber[i] %>');

                        <% 
                    }
                                
                                
        }


        else if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
        {
          
            %>
                        //Currently we don't need to do any swap or rearrangement of the organs in Number Answering Mode.
                        //We can leave pickedRandQNo empty.

                        /*
                        //set the pickedRandQNo as the Question Number of organs picked by the instructor. 
                        //the content of 'pickedQuestionOrganArray' may look like '1,2,6,9,12'
                        pickedRandQNo = pickedQuestionOrganArray;
                        */
                        for (i = 0; i < questionArray.length; i++)
                        {
                            pickedRandQNo.push(questionArray[i])
                        }

                        
                       
             <% 
        }
            %>
        
                        



                        //對調IPC table rows ,改tableID ,rowID與pickedRandQNo 與questionList :1~n即可符合使用
                        //對調IPC table rows according to the pickedRandQNo array sent through Session
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
                            swapTarget = questionList.indexOf(parseInt(pickedRandQNo[i]));


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
                        inExamMode = true;
                        showTBOfQuestionOrgans(inExamMode);



                    }



                    //2019/4/9 Ben commented for using default URL value if no paras provided.
                    //if (url.searchParams.get("examMode") == 0) {
                    if (examMode != "Yes") {

                        //show the TextBox of the question Organs.
                        inExamMode = false;
                        showTBOfQuestionOrgans(inExamMode);
                    }



                    onloadFun();
                }



            });


            //Prevent users from submitting a TextBox content by hitting Enter
            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;//do not postBack
                }
            });





            //Let the function called 'EndRequestHandler' executed after coming back from UpdatePanel AJAX 
            //This is the fix terms for using ASP UpdatePanel AJAX
           
            /* Don't mess with any of the below code */
            Sys.Application.add_init(appl_init);

            
            /*
            //activate the count down timer
            activateCountDownTimer();
            */


            //window.onload = onloadFun;




        });


        function appl_init() {
            //set the function that  should be done on frontend before UpdatePanel postback
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beforeAsyncPostBack);
            //set the function that  should be done on frontend after UpdatePanel postback
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(afterAsyncPostBack);
          
        }



        function activateCountDownTimer() {
            /*get the remaining time (sec) and TimeIsUp value from server side*/
            //set these 2 vars in the hidden field on server side
            //read these 2 vars in the hidden field on client side


            serverSideRemainingTimeSec = parseInt(document.getElementById('<%= hidden_serverSideRemainingTimeSec.ClientID %>').value)


            var deadline = new Date(Date.parse(new Date()) + serverSideRemainingTimeSec * 1000);



            initializeClock('clockdiv', deadline);


        }


        function getTimeRemaining(endtime) {
            var t = Date.parse(endtime) - Date.parse(new Date());
            var seconds = Math.floor((t / 1000) % 60);
            var minutes = Math.floor((t / 1000 / 60) % 60);
            var hours = Math.floor((t / (1000 * 60 * 60)) % 24);

            return {
                'total': t,

                'hours': hours,
                'minutes': minutes,
                'seconds': seconds
            };
        }

        function initializeClock(id, endtime) {
            var clock = document.getElementById(id);

            var hoursSpan = clock.querySelector('.hours');
            var minutesSpan = clock.querySelector('.minutes');
            var secondsSpan = clock.querySelector('.seconds');






            function updateClock() {
                var t = getTimeRemaining(endtime);


                hoursSpan.innerHTML = ('0' + t.hours).slice(-2);
                minutesSpan.innerHTML = ('0' + t.minutes).slice(-2);
                secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);

                if (t.total <= 0) {
                    clearInterval(timeinterval);
                    //alert("time is up");

                    //force submit the AI type exam paper when time is up
                    document.getElementById('<%= btnBack.ClientID %>').click();


                }
            }


            //updateClock(endtime, hoursSpan, minutesSpan, secondsSpan);
            updateClock();
            var timeinterval = setInterval(updateClock, 1000);

        }



       


        //do  something on the frontend before the asp UpdatePanel post back
        function beforeAsyncPostBack() {

        }

        //do  something on the frontend after the asp UpdatePanel post back
        function afterAsyncPostBack() {


            //do the init ready function again after coming back from UpdatePanel AJAX 
            partOf_documentReadyFun();

            //do the window.onload function again because AJAX doesn't trigger window.onload event.
            onloadFun();






        }
        function recoverHideShowStatus_ofNonQuestionTRs() {

            if (document.getElementById("hidden_HideNonQuestionTRS").value == "true") {

                hideNonQuestionTR();


                //document.getElementById("hidden_HideNonQuestionTRS").value = "false";
            }


        }

        function resetParameters() {
            questionArray = [];
        }
        function partOf_documentReadyFun() {

            //activate exam mode

            //2019/4/9 Ben commented for using default URL value if no paras provided.
            //if (url.searchParams.get("examMode") == "Yes") {
            if (examMode == "Yes") {




                //get the randomized  Question Numbers picked by instructor to IPC.aspx sent through Session
                var pickedRandQNo = [];
                        <% 
     
        if (NameOrNumberAnsweringMode_Session == "Name Answering Mode")
        {
           
        
       
        
                 var randomizedQuestionOrganNumber = RandomQuestionNoSession;
                                    //if RandomQuestionNoSession is not null
                 if (randomizedQuestionOrganNumber[0] != -1)
                     for (int i = 0; i < randomizedQuestionOrganNumber.Length; i++)
                    {
                                
                         
                        %>

                pickedRandQNo.push('<%= randomizedQuestionOrganNumber[i] %>');

                <% 
                    }
                                
                                
        }


        else if (NameOrNumberAnsweringMode_Session == "Number Answering Mode")
        {
            // do nothing to leave pickedRandQNo empty.
        
            %>
                //Currently we don't need to do any swap or rearrangement of the organs in Number Answering Mode.
                //We can leave pickedRandQNo empty.

                /*
                //set the pickedRandQNo as the Question Number of organs picked by instructor.
                pickedRandQNo = pickedQuestionOrganArray;
                */

                
                            for (i = 0; i < questionArray.length; i++)
                            {
                                pickedRandQNo.push(questionArray[i])
                            }
                            



                <% 
        }
            %>

                //alert(pickedRandQNo);



                //對調IPC table rows ,改tableID ,rowID與pickedRandQNo 與questionList :1~n即可符合使用
                //對調IPC table rows according to the pickedRandQNo array sent through Session
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
                    swapTarget = questionList.indexOf(parseInt(pickedRandQNo[i]));


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
                inExamMode = true;
                showTBOfQuestionOrgans(inExamMode);


                //recover the hide or show status of the non question TRs
                recoverHideShowStatus_ofNonQuestionTRs();
            }




            if (examMode != "Yes") {

                //show the TextBox of the question Organs.
                inExamMode = false;
                showTBOfQuestionOrgans(inExamMode);
            }







        }


        //swap the elements of index x and y in an array.
        function swapElement(questionList, x, y) {
            var b = questionList[y];
            questionList[y] = questionList[x];
            questionList[x] = b;
        }


        function hideNonQuestionTR() {
            var x = document.getElementsByClassName("nonQuestionTR");


            //toggle the class to swich the hide and show 
            for (i = 0; i < x.length; i++) {
                x[i].classList.toggle("hidden");
            }

            //toggle the indicator of the hide and show Non Question Rows button
            if (x[0].classList.contains('hidden'))
                document.getElementById("hidden_HideNonQuestionTRS").value = "true";
            else
                document.getElementById("hidden_HideNonQuestionTRS").value = "false";

            //assign the image of the hide non question <tr> btn
            //$('#HideNonQuestionTR').innerHTML = '<img src="' + visibleImg + '" />';
            //$('#HideNonQuestionTR').innerHTML = 'abcde';
        }




        //store the clickedOrganSubmitBtnNo to a sessionStorage on Client side for setting the value of the corresponding hidden field in the GridView in the UpdatePanel
        //after the UpdatePanel updates the content via AJAX.
        $(document).on("click", ".submit_img", function () {

            //to know which row's OrganSubmitBtn is clicked
            clickedOrganSubmitBtnNo = parseInt($('.submit_img').index(this)) + 1;


            //store the clickedOrganSubmitBtnNo to a sessionStorage on Client side
            //We will use it to change the value of the corresponding hidden field in the GridView in the UpdatePanel
            //after the UpdatePanel updates the content via AJAX.
            //If we set the value of the corresponding hidden field in the GridView here, the modification will be overwritten by the AJAX used by the UpdatePanel.
            sessionStorage.setItem('clickedOrganSubmitBtnNo', clickedOrganSubmitBtnNo);

        });

        
        
        function askUserOpen3DBuilder() {

            alert("IPC Pipe has been created! \n\nNow,please activate the 3DBuilder by clicking the '3DBuilder MFC Application.rdp'.");
        }




    </script>



</asp:Content>
