﻿
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

    public class CsDBOp
    {
       
        #region Common
        private static DataTable GetDataTable(string sql, string targetDB)
        {
            return CsDBConnection.GetDataSet(sql, targetDB).Tables[0];
        }

        /// <summary>
        /// Get a first DataTable for the query string.
        /// (ex: "SELECT * FROM table1 WHERE ...")
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static int InsertData(string sql,string targetDB)
        {
            SqlCommand cmd = new SqlCommand(sql);
            return CsDBConnection.ExecuteNonQuery(cmd, targetDB);
            
            //return CsDBConnection.ExecuteNonQuery(sql);
        }

        private static int InsertUserEnteredData(string sql, object[] sqlParameterList, string targetDB)
        {

            SqlCommand cmd = new SqlCommand(sql);
            fillSqlParameters(cmd, sqlParameterList);
            return CsDBConnection.ExecuteNonQuery(cmd, targetDB);
        }


        private static int UpdateData(string sql, string targetDB)
        {

            SqlCommand cmd = new SqlCommand(sql);
            return CsDBConnection.ExecuteNonQuery(cmd, targetDB);
            
            //return CsDBConnection.ExecuteNonQuery(sql);
        }

        private static int UpdateUserEnteredData(string sql, object[] sqlParameterList, string targetDB)
        {

            SqlCommand cmd = new SqlCommand(sql);
            fillSqlParameters(cmd, sqlParameterList);
            return CsDBConnection.ExecuteNonQuery(cmd, targetDB);
        }

        private static int DeleteData(string sql, string targetDB)
        {
            SqlCommand cmd = new SqlCommand(sql);
            return CsDBConnection.ExecuteNonQuery(cmd, targetDB);


            //return CsDBConnection.ExecuteNonQuery(sql);
        }



        //使用SQLParameter to make the value of the variable to be a parameter to prevent SQL Injection Attack.
        //Parameter 可以 (1)檢查參數的型別 (2)檢查資料長度 (3)確保參數為非可執行的SQL命令 
        private static void fillSqlParameters(SqlCommand cmd, object[] pList)
        {
            Regex r = new Regex("(@\\w+)", RegexOptions.IgnoreCase);
            MatchCollection matches = r.Matches(cmd.CommandText);
            for (int i = 0; i < matches.Count; i++)
            {
                Match m = matches[i];
                if (pList[i] == null)
                {
                    pList[i] = " ";
                }

                cmd.Parameters.AddWithValue(m.Value, pList[i]);
            }
        }



    #endregion

    #region Insert Student's answer of the AITypeQuestion related DB Data
    /// <summary>
    /// Get user's information by token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>



    //Store student's answer and QuestionNumberOrder to DB
    //get 
    public static DataTable GetStuIPCAns()
    {
       //string sql = string.Format("Select * From StuCouHWDe_IPC ");


        string sql = string.Format("Select * From AITypeQuestionStudentAnswer ");
        return GetDataTable(sql,"NewVersionHintsDB");
    }
    //set
    public static int InsertStuIPCAns(string cUserID, string cQID, string _QuesOrdering, string _StudentAnswer,string cActivityID)
    {



        DataRowCollection DRC = GetDataTable(string.Format("Select * from AITypeQuestionStudentAnswer where cUserID = '{0}' and cActivityID = {1} and cQID='{2}'", cUserID, cActivityID,cQID), "NewVersionHintsDB").Rows;
        

        if (DRC.Count == 0)
        {
            //string sql = string.Format("Insert into StuCouHWDe_IPC(StuCouHWDe_ID ,cActivityID,StudentAnswer,QuesOrdering ) VALUES( '{0}', '{1}', '{2}','{3}' )", _StuCouHWDe_ID, cActivityID, _StudentAnswer, _QuesOrdering);

            string sql = string.Format("Insert into AITypeQuestionStudentAnswer(cUserID ,cQID,StudentAnswer,QuesOrdering,cActivityID ) VALUES( '{0}', '{1}', @StudentAnswer,'{2}','{3}' )", cUserID, cQID, _QuesOrdering,cActivityID);
            
            object[] sqlParametersList = { _StudentAnswer };

           

            return InsertUserEnteredData(sql,sqlParametersList,"NewVersionHintsDB");

        }


        //update the student's answer to AITypeQuestionStudentAnswer data table
        else {
            
            /*
            //append the student's new answer to the existing answer
            string sql = string.Format("UPDATE AITypeQuestionStudentAnswer  set StudentAnswer =  cast(StudentAnswer as nvarchar(max)) + cast( @StudentAnswer as nvarchar(max)), QuesOrdering = cast(QuesOrdering as nvarchar(max)) + cast( '{0}' as nvarchar(max)) where cUserID =  '{1}' and cQID =  '{2}' and  cActivityID='{3}'", _QuesOrdering, cUserID, cQID, cActivityID);
            */


            //replace the student's existing answer with the new answer.
            string sql = string.Format("UPDATE AITypeQuestionStudentAnswer  set   StudentAnswer = @StudentAnswer, QuesOrdering='{1}'  where cUserID = '{0}' and  cQID =  '{2}' and cActivityID = {3}  ", cUserID, _QuesOrdering, cQID, cActivityID);
            object[] sqlParametersList = { _StudentAnswer };
            return UpdateUserEnteredData(sql, sqlParametersList, "NewVersionHintsDB");
        }
    }





        //11/9 store filename ,order, and correct answer to DB '[SCOREDB].[dbo].[IPCExamHWCorrectAnswer]'
        //use cActivityID to find if the record has already existed.
        /*
         if (GetDataTable(string.Format("Select * from AdditionScore where RID = {0:d}", rId))?.Rows.Count > 0)
            sql = string.Format("Update AdditionScore Set Score = '{1}', Opinion = '{2}' Where RID = {0:d} ", rId, model.AdditionScore, model.AdditionOpinion);
        else
            sql = string.Format("Insert Into AdditionScore Values({0:d}, '{1}', '{2}')", rId, model.AdditionScore, model.AdditionOpinion);

        */




//The following is the example of how to Access and store data into DB
/*
    //Access and store Score into DB
    public static DataTable GetAllTBData()
        {
            string sql = string.Format("Select * From StuCouHWDe_IPC ");
            return GetDataTable(sql);
        }
        public static int InsertScore(string ID,string grade)
        {
            string  sql = string.Format("Insert into StuCouHWDe_IPC VALUES( '{0}', '{1}' )", ID,grade);
            return InsertData(sql);
        }

        public static int UpdateScore(string ID, string NewGrade)
        {
            string sql = string.Format("Update StuCouHWDe_IPC set Grade = ' {1:d} ' where StuCouHWDe_ID = '{0}'  ", ID, NewGrade);
            return UpdateData(sql);
        }

        public static int DeleteScore(string ID)
        {
            string sql = string.Format("Delete from StuCouHWDe_IPC where StuCouHWDe_ID = '{0}' ", ID);
            return UpdateData(sql);
        }
*/


    #endregion






    #region Access AITypeQuestionCorrectAnswer DB Data
    public static DataTable GetIPCExamHWCorrectAns()
    {
        //string sql = string.Format("Select * From IPCExamHWCorrectAnswer ");

        string sql = string.Format("Select * From AITypeQuestionCorrectAnswer ");
        return GetDataTable(sql,"NewVersionHintsDB");
    }


   

    //Insert the correct Answer of the AITypeQuestion to AITypeQuestionCorrectAnswer datatable in NewVersionHintsDB
    public static int InsertIPCExamHWCorrectAns(string cQID, string _correctAnswer, string _QuestionBodyPart, string _QuesOrdering)
    {
        //first add
        //if (Num_Of_Question_Submision_Session == 1)


        /*
        DataRowCollection DRC = GetDataTable(string.Format("Select * from IPCExamHWCorrectAnswer where cActivityID = {0:d}", _cActivityID)).Rows;
        */

        DataRowCollection DRC = GetDataTable(string.Format("Select * from AITypeQuestionCorrectAnswer where cQID = '{0}'", cQID), "NewVersionHintsDB").Rows;
        

        if (DRC.Count == 0)
        {
            /*
            string sql = string.Format("Insert into IPCExamHWCorrectAnswer(cActivityID,correctAnswer ,QuestionBodyPart,correctAnswerOrdering ) VALUES( '{0}', '{1}', '{2}','{3}' )", _cActivityID, _correctAnswer, _QuestionBodyPart, _QuesOrdering);
            
             */
            string sql = string.Format("Insert into AITypeQuestionCorrectAnswer (cQID,correctAnswer ,QuestionBodyPart,correctAnswerOrdering ) VALUES( '{0}', '{1}', '{2}','{3}' )", cQID, _correctAnswer, _QuestionBodyPart, _QuesOrdering);
            
           return InsertData(sql,"NewVersionHintsDB");

        }
        //update
        else
        {
            string sql;
            if (DRC[0][2].ToString().Contains(_QuestionBodyPart))
            {
                int index=0;
                foreach(string tempStr in DRC[0][2].ToString().Split(',')){
                    if (tempStr == _QuestionBodyPart)
                        break;
                    index++;
                }
                //subtrim end ":" from string 
                _correctAnswer =_correctAnswer.Remove(_correctAnswer.Length - 1);
                _QuesOrdering = _QuesOrdering.Remove(_QuesOrdering.Length - 1);
                /*
                sql = string.Format("UPDATE[SCOREDB].[dbo].[IPCExamHWCorrectAnswer] SET correctAnswer = REPLACE(correctAnswer,'{0}','{1}')  where cActivityID =  '{2}' ",
                    DRC[0][1].ToString().Split(':')[index], _correctAnswer, _cActivityID);
                */
                sql = string.Format("UPDATE AITypeQuestionCorrectAnswer SET correctAnswer = REPLACE(correctAnswer,'{0}','{1}')  where cQID =  '{2}' ",
                    DRC[0][1].ToString().Split(':')[index], _correctAnswer, cQID);
                UpdateData(sql,"NewVersionHintsDB");

                /*
                sql = string.Format("UPDATE[SCOREDB].[dbo].[IPCExamHWCorrectAnswer] SET correctAnswerOrdering = REPLACE(correctAnswerOrdering,'{0}','{1}') where cActivityID =  '{2}' ",
                    DRC[0][3].ToString().Split(':')[index], _QuesOrdering, _cActivityID);
                 * 
                 */
                sql = string.Format("UPDATE AITypeQuestionCorrectAnswer SET correctAnswerOrdering = REPLACE(correctAnswerOrdering,'{0}','{1}') where cQID =  '{2}' ",
                    DRC[0][3].ToString().Split(':')[index], _QuesOrdering, cQID);
                return UpdateData(sql, "NewVersionHintsDB"); 

            }
                
            else{
                /*
                sql = string.Format("UPDATE[SCOREDB].[dbo].[IPCExamHWCorrectAnswer]  set correctAnswer =  cast(correctAnswer as nvarchar(max)) + cast( '{0}' as nvarchar(max)), QuestionBodyPart = cast(QuestionBodyPart as nvarchar(max)) + cast( ',{1}' as nvarchar(max)) ,correctAnswerOrdering = cast(correctAnswerOrdering as nvarchar(max)) + cast( '{2}' as nvarchar(max)) where cActivityID =  '{3}'"
                , _correctAnswer, _QuestionBodyPart,_QuesOrdering, _cActivityID);
                 * */
                sql = string.Format("UPDATE AITypeQuestionCorrectAnswer  set correctAnswer =  cast(correctAnswer as nvarchar(max)) + cast( '{0}' as nvarchar(max)), QuestionBodyPart = cast(QuestionBodyPart as nvarchar(max)) + cast( ',{1}' as nvarchar(max)) ,correctAnswerOrdering = cast(correctAnswerOrdering as nvarchar(max)) + cast( '{2}' as nvarchar(max)) where cQID =  '{3}'"
                , _correctAnswer, _QuestionBodyPart, _QuesOrdering, cQID);

                return UpdateData(sql, "NewVersionHintsDB");
            }
            
        }
    }

    #endregion



    #region get data needed on student answer sheet for AITypeQuestion e.g., Count Down timer
    //get the question description of an existing  AITypeQuestion when the instructor wants to edit it.
    public static int getExamRemainingTime(string cActivityID)
    {
        string strSQL = "SELECT cDeadline FROM MLAS_ActivityExaminationSetting WHERE cActivityID = '" + cActivityID + "' ";
        DataRowCollection DRC = GetDataTable(string.Format(strSQL), "MLASDB").Rows;

        if (DRC.Count == 0)
        {
            return -1;
        }

        else
        {
            string s = DRC[0]["cDeadline"].ToString();

            IFormatProvider culture = new System.Globalization.CultureInfo("zh-TW", true);
            DateTime deadline = DateTime.ParseExact(s, "yyyyMMddHHmm", culture);

            DateTime currenttime = DateTime.Now;

            TimeSpan span = (TimeSpan)(deadline - currenttime);

            int remainginTime = (int)span.TotalSeconds;

            //防止剛好時間相減後也等於沒有找到期限資料的回傳值-1
            if (remainginTime == -1)
            {
                remainginTime--;
            }

            return remainginTime;
        }




        //You can modify the following to get the remaining exam time
        /*
        string strSQL = "SELECT * FROM QuestionIndex WHERE cQID = '" + strQID + "' ";
        DataRowCollection DRC = GetDataTable(string.Format(strSQL), "MLASDB").Rows;

        //return empty string if there is no AITypeQuestion_QuestionDescription
        if (DRC.Count == 0)
            return "";

        //return the value of 'cQuestion' field of the retrieved record
        return int.Parse(DRC[0]["cQuestion"]);
        */

    }


    #endregion

    #region set up AITypeQuestion in Hints DB

    


    //get the question description of an existing  AITypeQuestion when the instructor wants to edit it.
    public static string getAITypeQuestion_QuestionDescription(string strQID)
    {
       
        string strSQL = "SELECT * FROM QuestionIndex WHERE cQID = '" + strQID + "' ";
        DataRowCollection DRC = GetDataTable(string.Format(strSQL),"NewVersionHintsDB").Rows;

        //return empty string if there is no AITypeQuestion_QuestionDescription
        if (DRC.Count == 0)
            return "";

        return DRC[0]["cQuestion"].ToString();

    }

    /// <summary>
    /// 儲存一筆資料至QuestionIndex
    /// </summary>
    /// <param name="strQID"></param>
    /// <param name="strQuestion"></param>
    /// <param name="intLevel"></param>
    public static int updateAITypeQuestionDescription_QuestionIndex(string strQID, string strQuestion, string strAnswer, int intLevel)
    {
       
            //Update the existing questions in the QuestionIndex table, if there is a question with the same cQID in the QuestionIndex table.
            string strSQL = "UPDATE QuestionIndex SET cQuestion = @cQuestion , cAnswer = @cAnswer , sLevel = '" + intLevel.ToString() + "' " +
                     "WHERE cQID = '" + strQID + "' ";

            //make the content entered by users be SQL Parameters when we execute the SQL Command to prevent SQL Injection Attack.
            object[] sqlParametersList = { strQuestion, strAnswer };

            return UpdateUserEnteredData(strSQL, sqlParametersList, "NewVersionHintsDB");
        



    }  

    /// <summary>
    /// 儲存一筆資料至QuestionIndex
    /// </summary>
    /// <param name="strQID"></param>
    /// <param name="strQuestion"></param>
    /// <param name="intLevel"></param>
    public static int saveIntoQuestionIndex(string strQID, string strQuestion, string strAnswer, int intLevel)
    {
        string strSQL = "";
        strSQL = "SELECT * FROM QuestionIndex WHERE cQID = '" + strQID + "' ";
        DataRowCollection DRC = GetDataTable(string.Format(strSQL), "NewVersionHintsDB").Rows;
        
        if (DRC.Count == 0)
        {
            //Insert the new questions in the QuestionIndex table, if there is not a question with the same cQID in the QuestionIndex table.
            strSQL = "INSERT INTO QuestionIndex (cQID , cQuestion, cAnswer , sLevel) " +
                     "VALUES ('" + strQID + "' , @cQuestion , @cANswer, '" + intLevel.ToString() + "') ";

            //make the content entered by users be SQL Parameters when we execute the SQL Command to prevent SQL Injection Attack.
            object[] sqlParametersList = { strQuestion, strAnswer };

            return InsertUserEnteredData(strSQL, sqlParametersList, "NewVersionHintsDB");
        }

        else 
        {
            //Update the existing questions in the QuestionIndex table, if there is a question with the same cQID in the QuestionIndex table.
            strSQL = "UPDATE QuestionIndex SET cQuestion = @cQuestion , cAnswer = @cAnswer , sLevel = '" + intLevel.ToString() + "' " +
                     "WHERE cQID = '" + strQID + "' ";

            //make the content entered by users be SQL Parameters when we execute the SQL Command to prevent SQL Injection Attack.
            object[] sqlParametersList = { strQuestion, strAnswer };

            return UpdateUserEnteredData(strSQL, sqlParametersList, "NewVersionHintsDB"); 
        }
        
        
       
    }

    /// <summary>
    /// 儲存一筆資料至QuestionMode
    /// </summary>
    /// <param name="strQID"></param>
    /// <param name="strPaperID"></param>
    /// <param name="strQuestionDivisionID"></param>
    /// <param name="strQuestionGroupID"></param>
    /// <param name="strQuestionMode"></param>
    /// <param name="strQuestionType"></param>
    /// <param name="templateQuestionQID"></param>
    /// <param name="strUserID"></param>
    public static void saveIntoQuestionMode(string strQID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode, string strQuestionType, string templateQuestionQID, string strUserID)
    //ben
    //public void saveIntoQuestionMode(string strQID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode, string strQuestionType, string templateQuestionQID = null, string strUserID=null)
    {
        string strSQL = "";
        //取得QuestionGroupName
        string strQuestionGroupName = getQuestionGroupNameByQuestionGroupID(strQuestionGroupID);


        //if teacher save the edited question as a new question
        //add similarID to the new question to show all the related similar question when the teacher picks one of the parent or offspring of this new question.
        if (templateQuestionQID != null && strUserID != null)
        {
            //to contain similarID to assign to the new question
            string similarID = "";
            string TemplateQuesQuestionGroupID = "";
            string TemplateQuesQuestionGroupName = "";
            //Ben check whether the the value of “similarID”of the question that is used as a template is null or not.

            strSQL = " SELECT similarID ,cQuestionGroupID,cQuestionGroupName FROM QuestionMode" +
                            " WHERE cQID = '" + templateQuestionQID + "' and cQuestionType= '" + strQuestionType + "' and similarID IS NOT NULL";
            DataRowCollection DRC = GetDataTable(string.Format(strSQL), "NewVersionHintsDB").Rows;           
    
            //If the question that is used as a template already has similarID in QuestionMode table.
            if (DRC.Count > 0)
            {

               
                //set similarID the same as the that of the question that is used as a template
                /*
                similarID = similarIDCheck.Tables[0].Rows[0]["similarID"].ToString();
                 * */
                similarID = DRC[0]["similarID"].ToString();


                //get the cQuestionGroupName and cQuestionGroupID of the template question
                /*
                TemplateQuesQuestionGroupID = similarIDCheck.Tables[0].Rows[0]["cQuestionGroupID"].ToString();
                TemplateQuesQuestionGroupName = similarIDCheck.Tables[0].Rows[0]["cQuestionGroupName"].ToString();
                */

                TemplateQuesQuestionGroupID = DRC[0]["cQuestionGroupID"].ToString();
                TemplateQuesQuestionGroupName = DRC[0]["cQuestionGroupName"].ToString();

            }

            //If the question that is used as a template doesn't have similarID in QuestionMode table.
            else
            {

                //set new similarID to the template question and the new question.
               
                similarID = strUserID + "_simiID_" + getNowTime();

                //assign the new similarID to the question that is used as a template
                //Update similarID of the template question
                strSQL = " UPDATE QuestionMode SET similarID = '" + similarID + "'  WHERE cQID = '" + templateQuestionQID + "' and cQuestionType='" + strQuestionType + "'";


                UpdateData(strSQL, "NewVersionHintsDB"); 




                //strQuestionGroupID and strQuestionGroupName are all empty, just get these fields from the template question.
                if (strQuestionGroupID == "" && strQuestionGroupName == "")
                {
                    //get the cQuestionGroupName and cQuestionGroupID of the template question
                    strSQL = " SELECT cQuestionGroupID,cQuestionGroupName FROM QuestionMode" +
                              " WHERE cQID = '" + templateQuestionQID + "' and cQuestionType= '" + strQuestionType + "' and similarID IS NOT NULL";


                    DataRowCollection DRCsimilarID = GetDataTable(string.Format(strSQL), "NewVersionHintsDB").Rows;    

                    //If the question that is used as a template already has similarID in QuestionMode table.
                    if (DRCsimilarID.Count > 0)
                    {
                        //set similarID the same as the that of the question that is used as a template
                        similarID = DRCsimilarID[0]["similarID"].ToString();

                        //get the cQuestionGroupName and cQuestionGroupID of the template question
                        TemplateQuesQuestionGroupID = DRCsimilarID[0]["cQuestionGroupID"].ToString();
                        TemplateQuesQuestionGroupName = DRCsimilarID[0]["cQuestionGroupName"].ToString();


                    }
                   
                }

                else//use the GroupID passed by parameter in to this function and the corresponding GroupName for the new question
                {
                    TemplateQuesQuestionGroupID = strQuestionGroupID;
                    TemplateQuesQuestionGroupName = strQuestionGroupName;
                }
            }
           





            //store similarID along with all other related data to table QuestionMode
            strSQL = " SELECT * FROM QuestionMode" +
                            " WHERE cQID = '" + strQID + "' ";

            DataRowCollection DRCcQID = GetDataTable(string.Format(strSQL), "NewVersionHintsDB").Rows;


           
            //Do nothing if there is an existing question with the same cQID in QuestionMode table.
            if (DRCcQID.Count > 0)
            {
                // 先註解，因為將QuestionMode資料表的QID欄位的Primary Key拿掉，所以若執行此Update會導致有重複筆資料。(功能上是為了可以將一題對話題放入不同的問題主題中。)  老詹 2015/09/06
                /*{
                    //Update
                    strSQL = " UPDATE QuestionMode SET cPaperID = '" + strPaperID + "' , cDivisionID = '" + strQuestionDivisionID + "' , cQuestionGroupID = '" + strQuestionGroupID + "' , cQuestionGroupName = '" + strQuestionGroupName + "' , cQuestionMode = '" + strQuestionMode + "' , cQuestionType = '" + strQuestionType + "' " +
                            " WHERE cQID = '" + strQID + "' ";
                }*/
            }

            //Insert the new question to QuestionMode table, if there is no existing questions with the same cQID.
            else
            {
                //Insert
                strSQL = " INSERT INTO QuestionMode (cQID , cPaperID , cDivisionID , cQuestionGroupID , cQuestionGroupName , cQuestionMode , cQuestionType,similarID) " +
                        " VALUES ('" + strQID + "' , '" + strPaperID + "' , '" + strQuestionDivisionID + "' , '" + TemplateQuesQuestionGroupID + "' , '" + strQuestionGroupName + "' , '" + strQuestionMode + "' , '" + strQuestionType + "' , '" + similarID + "') ";
                /*write SQL to file to 
                //to inspect the SQL cmd when something went wrong with SQL cmd 
                // Create a file to write to.              
                File.WriteAllText("D:/Hints_on_60/Hints/App_Code/AuthoringTool/CaseEditor/Paper/updateSimilarIDSQL.txt", strSQL);
                */

            }

            InsertData(strSQL, "NewVersionHintsDB");


        }


        //store a record to table QuestionMode(If it's not 另存新題)
        else
        {

            strSQL = " SELECT * FROM QuestionMode" +
                            " WHERE cQID = '" + strQID + "' ";

            DataRowCollection DRCcQID = GetDataTable(string.Format(strSQL), "NewVersionHintsDB").Rows;



            
            

            //Do nothing if there is an existing question with the same cQID in QuestionMode table.
            if (DRCcQID.Count > 0)
            {
                // 先註解，因為將QuestionMode資料表的QID欄位的Primary Key拿掉，所以若執行此Update會導致有重複筆資料。(功能上是為了可以將一題對話題放入不同的問題主題中。)  老詹 2015/09/06
                /*{
                    //Update
                    strSQL = " UPDATE QuestionMode SET cPaperID = '" + strPaperID + "' , cDivisionID = '" + strQuestionDivisionID + "' , cQuestionGroupID = '" + strQuestionGroupID + "' , cQuestionGroupName = '" + strQuestionGroupName + "' , cQuestionMode = '" + strQuestionMode + "' , cQuestionType = '" + strQuestionType + "' " +
                            " WHERE cQID = '" + strQID + "' ";
                }*/
            }

            //Insert the new question to QuestionMode table, if there is no existing questions with the same cQID.
            else
            {
                //Insert
                strSQL = " INSERT INTO QuestionMode (cQID , cPaperID , cDivisionID , cQuestionGroupID , cQuestionGroupName , cQuestionMode , cQuestionType) " +
                        " VALUES ('" + strQID + "' , '" + strPaperID + "' , '" + strQuestionDivisionID + "' , '" + strQuestionGroupID + "' , '" + strQuestionGroupName + "' , '" + strQuestionMode + "' , '" + strQuestionType + "') ";
            }
           
            //insert record to table QuestionMode 
            InsertData(strSQL, "NewVersionHintsDB");
        }

    }


    /// <summary>
    /// 取得某問卷在Paper_Content中最高的問題順序。
    /// </summary>
    /// <param name="strPaperID"></param>
    /// <returns></returns>
    private static int getPaperContentMaxSeq(string strPaperID)
    {
        int intReturn = 0;

        string strSQL = "SELECT MAX(sSeq) AS 'sSeq' FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' ";
        DataRowCollection DRCcQID = GetDataTable(string.Format(strSQL), "NewVersionHintsDB").Rows;
        if (DRCcQID.Count > 0)
        {
            try
            {
                intReturn = Convert.ToInt32(DRCcQID[0]["sSeq"]);
            }
            catch
            {
            }
        }
        
        return intReturn;
    }

    /// 將值存入Paper_Content
    public static  void  saveToPaper_Content(string strPaperID, string strQID, string strQuestionType, string strQuestionMode, string strStandardScore = "0")
    {
        //get the seq for the new AITypeQuestion when adding it to the exam paper.
        string strSeq = Convert.ToString(CsDBOp.getPaperContentMaxSeq(strPaperID) + 1);

        string strSQL = "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";
        DataRowCollection DRCcQID = GetDataTable(string.Format(strSQL), "NewVersionHintsDB").Rows;
        if (DRCcQID.Count > 0)
        {
            //Update
            strSQL = " UPDATE Paper_Content SET  sStandardScore = '" + strStandardScore + "' , cQuestionType = '" + strQuestionType + "' , cQuestionMode = '" + strQuestionMode + 
                "' WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";

            UpdateData(strSQL, "NewVersionHintsDB");

        }
        else
        {
            //Insert
            strSQL = " INSERT INTO Paper_Content (cPaperID , cQID , sStandardScore , cQuestionType , cQuestionMode , sSeq) " +
                " VALUES ('" + strPaperID + "' , '" + strQID + "'  ,'" + strStandardScore + "'  ,'" + strQuestionType + "'  ,'" + strQuestionMode + "'  ,'" + strSeq + "')";

            InsertData(strSQL, "NewVersionHintsDB");
        }
       
        
    }


    /// <summary>
    /// 傳回某個QuestionGroupName by QuestionGroupID
    /// </summary>
    /// <returns></returns>
    private static string getQuestionGroupNameByQuestionGroupID(string strQuestionGroupID)
    {
        string strReturn = "";

        string strSQL = "SELECT cNodeName FROM QuestionGroupTree WHERE cNodeID = '" + strQuestionGroupID + "' ";
        DataRowCollection DRC = GetDataTable(string.Format(strSQL), "NewVersionHintsDB").Rows;



        //Do nothing if there is an existing question with the same cQID in QuestionMode table.
        if (DRC.Count > 0)
        {
            strReturn = DRC[0]["cNodeName"].ToString();
        }
       

        return strReturn;
    }



    private static string getNowTime()
    {
        //此function會傳回符合資料庫定義的時間格式的現在時間
        string strYear, strMonth, strDay, strHour, strMin, strSec;

        //get year
        strYear = DateTime.Now.Year.ToString();
        //get month
        if (DateTime.Now.Month > 9)
        {
            strMonth = DateTime.Now.Month.ToString();
        }
        else
        {
            strMonth = "0" + DateTime.Now.Month.ToString();
        }
        //get day
        if (DateTime.Now.Day > 9)
        {
            strDay = DateTime.Now.Day.ToString();
        }
        else
        {
            strDay = "0" + DateTime.Now.Day.ToString();
        }
        //get Hour
        if (DateTime.Now.Hour > 9)
        {
            strHour = DateTime.Now.Hour.ToString();
        }
        else
        {
            strHour = "0" + DateTime.Now.Hour.ToString();
        }
        //get min
        if (DateTime.Now.Minute > 9)
        {
            strMin = DateTime.Now.Minute.ToString();
        }
        else
        {
            strMin = "0" + DateTime.Now.Minute.ToString();
        }
        //get sec
        if (DateTime.Now.Second > 9)
        {
            strSec = DateTime.Now.Second.ToString();
        }
        else
        {
            strSec = "0" + DateTime.Now.Second.ToString();
        }

        string strReturn = strYear + strMonth + strDay + strHour + strMin + strSec;
        return strReturn;
    }

        #endregion




    //the followings are the name of the databases. 
    //Other functions should denote that which database will the sql cmd execute on.
    private static string IPCTypeQuestionDB = "SCOREDB";
    private static string ProgramTypeQuestionDB = "CorrectStuHWDB";
    private static string AITypeQuestionDB = "NewVersionHintsDB";
    private static string DB_Slector = AITypeQuestionDB; //ProgramTypeQuestionDB;//IPCTypeQuestionDB;


    /*
    #region Common
    private static DataTable GetDataTable(string sql, string DBName)
    {
        return CsDBConnection.GetDataSet(sql, DBName).Tables[0];
    }

    /// <summary>
    /// Get a first DataTable for the query string.
    /// (ex: "SELECT * FROM table1 WHERE ...")
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    private static int InsertData(string sql, string DBName)
    {
        return CsDBConnection.ExecuteNonQuery(sql, DBName);
    }

    private static int UpdateData(string sql, string DBName)
    {
        return CsDBConnection.ExecuteNonQuery(sql, DBName);
    }

    private static int DeleteData(string sql, string DBName)
    {
        return CsDBConnection.ExecuteNonQuery(sql, DBName);
    }







    #endregion
     * 
     */ 


    #region Get correct answer or student's answer of the AITypeQuestion
    /// <summary>
    /// Get user's information by token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static DataTable GetAllTBData(string DB_Child)
    {
        string sql = string.Format("Select * " + DB_Child);
        return GetDataTable(sql, DB_Slector);//execute the sql cmd in DB_Slector database
    }

    //a input to select all data if CPaperID is cPaperID
    //get a student's recored according to his cUserID and cQID in datatable 'AITypeQuestionStudentAnswer'
    public static DataTable GetAllTBData(string DB_Child, string cQID, string cUserID = "", string cActivityID = "")
    {
        /*
        //In  博宇's implementation
        string sql = string.Format("Select * From " + DB_Child + " Where cActivityID In('" + cActivityID + "')");
         * */
        string sql = "";

        //we need to use cActivityID to get student's answer of the AITypeQuestion
        if (DB_Child == "AITypeQuestionStudentAnswer")
        {

            //it means cUserID is not provided with we call the function.
            if (cUserID == "")
            {
                sql = string.Format("Select * From " + DB_Child + " Where cQID In('" + cQID + "') and cActivityID='" + cActivityID + "'");
            }
            else//if cUserID is  provided with we call the function.
            {
                sql = string.Format("Select * From " + DB_Child + " Where cQID In('" + cQID + "') and cUserID='" + cUserID + "'" + "and cActivityID='" + cActivityID + "'");

            }

        }

        else if (DB_Child == "AITypeQuestionCorrectAnswer")
        {

            //it means cUserID is not provided with we call the function.
            if (cUserID == "")
            {
                sql = string.Format("Select * From " + DB_Child + " Where cQID In('" + cQID + "')");
            }
            else//if cUserID is  provided with we call the function.
            {
                sql = string.Format("Select * From " + DB_Child + " Where cQID In('" + cQID + "') and cUserID='" + cUserID + "'");

            }


        }

       
        return GetDataTable(sql, DB_Slector);//execute the sql cmd in DB_Slector database
    }


    public static int InsertScore(string DB_Child, string cUserID, string grade,string cActivityID)
    {
        string sql = string.Format("Insert into " + DB_Child + " VALUES( '{0}', '{1}', '{2}' )", cUserID, grade, cActivityID);
        return InsertData(sql, DB_Slector);//execute the sql cmd in DB_Slector database
    }

    public static int UpdateScore(string DB_Child, string cUserID, string NewGrade, string cQID, string cActivityID)
    {
        //In  博宇's implementation
        /*
        string sql = string.Format("Update " + DB_Child + " set Grade = '{1}' where StuCouHWDe_ID = '{0}'  ", ID, NewGrade);
         * */
        string sql = string.Format("Update " + DB_Child + " set Grade = '{1}' where cUserID = '{0}' and cQID = '{2}' and cActivityID= '{3}' ", cUserID, NewGrade, cQID, cActivityID);
        return UpdateData(sql, DB_Slector);//execute the sql cmd in DB_Slector database
    }

    public static int DeleteScore(string DB_Child, string cUserID, string cQID, string cActivityID)
    {
        //In  博宇's implementation
        /*
        string sql = string.Format("Delete from " + DB_Child + " where StuCouHWDe_ID = '{0}' ", ID);
         * */
        string sql = string.Format("Delete from " + DB_Child + " where cUserID = '{0}' and cQID = '{1}'  and cActivityID = '{2}' ", cUserID, cQID, cActivityID);
        return UpdateData(sql, DB_Slector);//execute the sql cmd in DB_Slector database
    }

    public static DataTable GetAITypeQuestionScore(string DB, string cQID, string cPaperID, string cQuestionType)
    {
        /*
        //In  博宇's implementation
        string sql = string.Format("Select * From " + DB_Child + " Where cActivityID In('" + cActivityID + "')");
         * */
        string sql = string.Format("Select cQuestionScore From " + DB + " Where cQID ='{0}' and cQuestionType='{1}' and cPaperID='{2}'", cQID, cQuestionType, cPaperID);
        return GetDataTable(sql, DB_Slector);//execute the sql cmd in DB_Slector database
    }

    #endregion

        /*
        public static DataTable GetMOEManagerInfo(int uId)
        {
            string sql = string.Format("Select * From MOEManager Where UID = '{0}'", uId);
            return GetDataTable(sql);
        }

        private static bool ExistRow(string sql) {
            bool ret = false;
            DataTable tbl = GetDataTable(sql);
            ret = (tbl.Rows.Count > 0);
            tbl.Dispose();

            return ret;
        }
*/
        /// <summary>
        /// Check if the user is administrator
        /// </summary>
        /// <param name="uId">User ID</param>
        /// <returns></returns>
        /// 
        /*
        private static bool IsAdministrator(int uId)
        {
            string sql = string.Format("Select * From Administrator Where UID = {0:d}", uId);

            return ExistRow(sql);
        }

        private static bool IsMOEManager(int uId)
        {
            string sql = string.Format("Select * From MOEManager Where UID = {0:d}", uId);

            return ExistRow(sql);
        }

        private static int GetIdentify(string fId, int uId)
        {
            int identify = 0;
            string sql = string.Format("Select * From UserIdentification WHERE FID = '{0}' And UID = {1:d}", fId, uId);
            DataTable tbl = GetDataTable(sql);
            if (tbl.Rows.Count > 0) {
                identify = tbl.Rows[0].Field<int>("Identification");
            }
            tbl.Dispose();

            return identify;
        }

        public static int GetUserIdentify(string fId, int uId) {
            int ret = 0;

            if (IsAdministrator(uId))
            {
                ret = CsConstants.AUTH_ADMIN;
            }
            else {
                ret = GetIdentify(fId, uId);
            }

            return ret;
        }

        public static int GetUserIdentifyWithManager(string fId, int uId)
        {
            int ret = 0;

            if (IsAdministrator(uId))
            {
                ret = CsConstants.AUTH_ADMIN;
            }
            else if(IsMOEManager(uId))
            {
                ret = CsConstants.AUTH_MAIN_MANAGER;
            }
            else
            {
                ret = GetIdentify(fId, uId);
            }

            return ret;
        }

        public static int GetUIDWithIdentify( string fId, int identify)
        {
            string sql = string.Format("Select UID from UserIdentification where FID = '{0}' AND Identification = {1:d} ", fId, identify);
            if (GetDataTable(sql).Rows.Count > 0)
                return GetDataTable(sql).Rows[0].Field<int>("UID");
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>0: Normal, -1: Invalid Token, -2: Token is expired</returns>
        public static int CheckAccessToken(string token, out int userid)
        {
            int ret = 0;
            userid = 0;

            DataTable user = CsDBOp.GetUser(token);
            if (user.Rows.Count == 0)
            {
                ret = -1;
                user.Dispose();
            }
            else
            {
                if (user.Rows[0].Field<string>("DateOfAccess").CompareTo(CsCommonFunction.TodayString()) < 0)
                {
                    ret = -2;
                    user.Dispose();
                }
                else
                {
                    // Check identify
                    userid = user.Rows[0].Field<int>("UID");
                }
            }

            return ret;
        }

        public static int GetReviewerLevel(string fId, int uId,int state) {
            int reviewerLevel = 1;
            //string sql = string.Format("Select * From Reviewer WHERE FID = '{0}' And UID = {1:d}", fId, uId); 
            string sql = string.Format(" Select c._Index From " +
            "(Select * From[Group] Where FID = '{0}' AND State = {2:d}) c Inner Join " +
            "(Select * From ReviewerGroup Where UID = {1:d}) d On c.GID = d.GID order by c._Index DESC ", fId, uId,state);
            DataTable tbl = GetDataTable(sql);
            if (tbl.Rows.Count > 0)
            {
                reviewerLevel = tbl.Rows[0].Field<Int32>("_Index");
            }
            tbl.Dispose();

            return reviewerLevel;
        }


        public static DataTable GetAllUserwithIdentity(string fId, int Identity )
        {
            string sql = "";
            if(Identity == CsConstants.AUTH_REVIEWER)
            {
                 sql = string.Format("SELECT a.*, c.*, d.State FROM elfess_User a left join UserIdentification b on a.UID = b.UID " +
                                     "left join ReviewerCandidate c on a.UID = c.UID left join ReviewerState d on c.UID = d.UID and b.FID = d.FID " +
                                     "where b.FID = '{0}' AND ( Identification & {1:d} ) = {1:d} order by a.UID", fId, Identity);
            }
            else
            {
                sql = string.Format("SELECT a.* FROM elfess_User a left join UserIdentification b on a.UID = b.UID " +
                                     "where b.FID = '{0}' AND ( Identification & {1:d} ) = {1:d} order by a.UID", fId, Identity);
            }
            
            return GetDataTable(sql);
        }

        public static int SetUserWithIdentity(string fId, int uId, int Identity)
        {
            string sql = string.Format("Select * from UserIdentification where FID = '{0}' AND UID =  {1:d} ", fId, uId);
            if( GetDataTable(sql).Rows.Count > 0)
            {
                int NewIdentity = GetDataTable(sql).Rows[0].Field<int>("Identification");
                NewIdentity = (Identity | NewIdentity);
                sql = string.Format("Update UserIdentification set Identification = {2:d} where FID = '{0}' AND UID =  {1:d} ", fId, uId, NewIdentity);
            }
            else
            {
                sql = string.Format("Insert into UserIdentification VALUES( '{0}', {1:d}, {2:d} )", fId, uId, Identity);
            }
            
            return InsertData(sql);
        }


        public static int GetUserIDWithEmail(string email)
        {
            string sql = string.Format("SELECT UID FROM elfess_User where account = '{0}' ", email);
            if(  GetDataTable(sql).Rows.Count > 0 )
            {
                return GetDataTable(sql).Rows[0].Field<int>("UID");
            }

            return 0;
        }

        public static DataTable GetDesignatedReviewer(string fId)
        {
            string sql = string.Format("select A.UID, C.Name, C.Email from UserIdentification A left join ReviewerCandidate B on A.UID= B.UID left join [User] C on A.UID = C.UID where (A.Identification & {1:d})= {1:d} AND A.FID='{0}' ", fId, CsConstants.AUTH_REVIEWER);
            return GetDataTable(sql);
        }

        public static DataTable GetAllReviewerCandidate()
        {
            string sql = string.Format("select A.UID, Name, Email from ReviewerCandidate A left join [User] B on A.UID = B.UID ");
            return GetDataTable(sql);
        }

        public static DataTable GetAllReviewers(string fId, int identification)
        {
            string sql = string.Format("SELECT A.UID, B.Field, B.Expertise, C.* from UserIdentification " + 
                                    "A left join ReviewerCandidate B on A.UID = B.UID left join elfess_User C on A.UID = C.UID" +
                                    " where A.FID = '{0}' and ( a.Identification & {1:d} ) = {1:d} ", fId, identification);
            return GetDataTable(sql);
        }

        public static DataTable GetAllUsers()
        {
            string sql = string.Format("select distinct * from [User]");
            return GetDataTable(sql);
        }

        public static DataTable GetReviewerList(string fId, bool test = false )
        {
            string sql = string.Format("SELECT * from elfess_User A left join ReviewerCandidate B on A.UID = B.UID " +
                "where A.UID not in  (select UID from UserIdentification where FID = {0} AND ( " + ( test ? "" : "Identification & 1 = 1 OR" ) + " Identification & 16 = 16 ) )", fId);
            return GetDataTable(sql);
        }

        public static DataTable GetAllUserList()
        {
            string sql = string.Format("SELECT * from elfess_User " );
            return GetDataTable(sql);
        }
        public static DataTable GetUserList(int uId)
        {
            string sql = string.Format("SELECT * from elfess_User where UID = {0} ",uId);
            return GetDataTable(sql);
        }
        public static DataTable GetUserByFIDMoreThanIden(string fId, int identification )
        {
            string sql = string.Format("select * from UserIdentification where FID = '{0}' AND Identification >= {1:d}", fId, identification );
            return GetDataTable(sql);
        }

        public static DataTable GetUserByFIDAndIden(string fId, int identification)
        {
            string sql = string.Format("select I.*,U.Name from UserIdentification I left join [User] U on I.UID=U.UID where I.FID = '{0}' AND I.Identification & {1:d}={1:d}", fId, identification);
            return GetDataTable(sql);
        }

        public static DataTable GetUserByDivision(string Division)
        {
            string sql = string.Format("Select* FROM MOEManager Where Division='{0}'  order by Position ASC", Division);
            return GetDataTable(sql);
        }
        public static DataTable GetDivisionByDepartment(string Department)
        {
            string sql = string.Format("Select distinct Division FROM MOEManager Where Department='{0}' And Division is not null", Department);
            return GetDataTable(sql);
        }

        public static string GetDomainName( string name)
        {
            string sql = string.Format("Select * FROM [University] Where schoolName like '%{0}%' ", name);
            DataTable dt =  GetDataTable(sql);
            if (dt.Rows.Count > 0) return dt.Rows[0].Field<string>("DomainName");
            else return null;
        }
        #endregion

        #region ScoreItem
        /// <summary>
        /// Get the count of ScoreItem.
        /// </summary>
        /// <param name="fId">Form ID</param>
        /// <param name="reviewLevel">Review Level</param>
        /// <returns></returns>
        public static int GetScoreItemCount(string fId, int reviewLevel) {
            string sql = string.Format("Select Count(*) From ScoreItem Where FID = '{0}' AND _Index = {1}", fId, reviewLevel);
            int ret = 0;

            DataTable tbl = GetDataTable(sql);
            ret = tbl.Rows[0].Field<int>(0);
            tbl.Dispose();

            return ret;
        }

        /// <summary>
        /// Get ScoreItem.
        /// </summary>
        /// <param name="fId">Form ID</param>
        /// <param name="reviewLevel">Review Level</param>
        /// <returns></returns>
        public static DataTable GetScoreItem(string fId, int reviewLevel,int state)
        {
            //string sql = string.Format("Select A.SIID, A.Name, A.MaxScore, B.Seq From ScoreItem A left join ScoreItemfordisplay B on A.SIID = B.SIID " +
            //                        " left join DisplayListName C on B.DID = C.DID Where A.FID = '{0}' AND C.[Current] = 'True' " +
            //                        " AND _Index = {1:d} Order By A.Seq", fId, reviewLevel);
            string sql = string.Format("Select * From ScoreItem Where FID = '{0}' AND _Index = {1:d} AND State = {2:d} Order By Seq", fId, reviewLevel, state);
            return GetDataTable(sql);
        }

        public static DataTable GetDisplayScoreItem(int dId)
        {
            string sql = string.Format("SELECT A.SIID, A.Seq, B.Name From ScoreItemForDisplay A " +
                                       "left join ScoreItem B on A.SIID = B.SIID where A.DID = {0:d}", dId);
            return GetDataTable(sql);
        }



        public static int GetMaxReviewerLevel(string fId , int state)
        {
            string sql = string.Format("Select Value From Configuration Where FID = '{0}' and  CID = 'ExaminerNum' and State = {1:d}", fId , state);
            DataTable tbl = GetDataTable(sql);
            int ret = 0;
            if(tbl.Rows.Count>0)
            {
                ret = Int32.Parse(tbl.Rows[0].Field<string>(0));
                tbl.Dispose();
            }
            

            return ret;
        }

        public static bool DeleteScoreItems(string fId, int reviewlevel,int state)
        {

            bool ret = false;
            int tmp = 0;
            string sql = "";
            // Batch operation
            BegineTransaction(SCOREITEM_TRANSACTION);
            sql = string.Format("Delete from ScoreItem where FID = '{0}' AND _Index =  {1:d} AND State = {2:d}", fId, reviewlevel, state);
            BatchDeleteData(SCOREITEM_TRANSACTION, sql);
            sql = string.Format("Delete From ScoreDescription Where FID = '{0}' and  ReviewLevel = {1:0} AND State = {2:d}", fId, reviewlevel, state);
            BatchDeleteData(SCOREITEM_TRANSACTION, sql);
            sql = string.Format("Delete From ReviewConfig Where FID = '{0}' and  _Index = {1:0}  and CID = 'TeachingMemberScore' AND State = {2:d}", fId, reviewlevel, state);
            BatchDeleteData(SCOREITEM_TRANSACTION, sql);
            sql = string.Format("Delete From ReviewConfig Where FID = '{0}' and  _Index = {1:0}  and CID = 'LimitNumber' AND State = {2:d}", fId, reviewlevel, state);
            BatchDeleteData(SCOREITEM_TRANSACTION, sql);
            sql = string.Format("Delete From ReviewConfig Where FID = '{0}' and  _Index = {1:0}  and CID = 'AdditionLimitNUmber' AND State = {2:d}", fId, reviewlevel, state);
            BatchInsertData(SCOREITEM_TRANSACTION, sql);
            ret = CommitTransaction(SCOREITEM_TRANSACTION);
            EndTransaction(SCOREITEM_TRANSACTION);

            return ret;
        }

        public static bool SetScoreItems(string fId, ScoreItemDetailM scoreitemdetail ,int state)
        {

            bool ret = false;
            int tmp = 0;
            string sql = "";
            // Batch operation
            BegineTransaction(SCOREITEM_TRANSACTION);
            foreach ( ScoreItemM scoreitem in scoreitemdetail.ScoreItems)
            {
                sql = string.Format("Insert Into ScoreItem(FID,_Index,Name,MaxScore,Seq,State) Values( '{0}', {1:d}, '{2}', {3}, {4},{5:d})", fId, scoreitemdetail.ReviewLevel,
                                scoreitem.Name, scoreitem.MaxScore, scoreitem.Seq , state);
                BatchInsertData(SCOREITEM_TRANSACTION, sql);
            }InsertData
            sql = string.Format("Insert Into ScoreDescription(FID,ReviewLevel,Description,State) Values('{0}', {1:d}, '{2}',{3:d} )", fId, scoreitemdetail.ReviewLevel, scoreitemdetail.ScoreDescription,state);
            BatchInsertData(SCOREITEM_TRANSACTION, sql);
            sql = string.Format("Insert Into ReviewConfig(FID,_index,CID,Value,State) Values('{0}', {1:d}, 'TeachingMemberScore', '{2}',{3:d})", fId, scoreitemdetail.ReviewLevel, scoreitemdetail.AdditionScore,state);
            BatchInsertData(SCOREITEM_TRANSACTION, sql);
            sql = string.Format("Insert Into ReviewConfig(FID,_index,CID,Value,State) Values('{0}', {1:d}, 'LimitNumber', '{2}',{3:d})", fId, scoreitemdetail.ReviewLevel, scoreitemdetail.LimitNumber, state);
            BatchInsertData(SCOREITEM_TRANSACTION, sql);
            sql = string.Format("Insert Into ReviewConfig(FID,_index,CID,Value,State) Values('{0}', {1:d}, 'AdditionLimitNUmber', '{2}',{3:d})", fId, scoreitemdetail.ReviewLevel, scoreitemdetail.AddLimitNumber, state);
            BatchInsertData(SCOREITEM_TRANSACTION, sql);
            ret = CommitTransaction(SCOREITEM_TRANSACTION);
            EndTransaction(SCOREITEM_TRANSACTION);

            return ret;
        }
        

        public static int DeleteScoreItemForDisplay( int dId)
        {
            string sql =string.Format("Delete from ScoreItemForDisplay where DID = {0:d}", dId);
            return DeleteData(sql);
        }

        public static bool InsertScoreItemForDisplay(int dId, List<ItemM> Item)
        {
            BegineTransaction(SCOREITEMFORDISPLAY_TRANSACTION);
            foreach (ItemM scoreitem in Item)
            {
                string sql = string.Format("Insert Into ScoreItemForDisplay Values( {0:d}, {1:d}, {2:d})", dId, scoreitem.IID, scoreitem.Seq);
                BatchInsertData(SCOREITEMFORDISPLAY_TRANSACTION, sql);
            }
            bool ret = CommitTransaction(SCOREITEMFORDISPLAY_TRANSACTION);
            EndTransaction(SCOREITEMFORDISPLAY_TRANSACTION);
            return ret;
        }





        public static string GetScoreDescription(string fId, int reviewLevel,int state)
        {
            string sql = string.Format("Select Description From ScoreDescription Where FID = '{0}' and  ReviewLevel = {1:0} and State={2:d}", fId, reviewLevel, state);
            if (GetDataTable(sql).Rows.Count > 0)
            {
                return GetDataTable(sql).Rows[0].Field<string>("Description");
            }
            return null;
        }

        public static string GetAdditionScore(string fId, int reviewLevel,int state)
        {
            string sql = string.Format("Select Value From ReviewConfig Where FID = '{0}' and  _Index = {1:0} and CID = 'TeachingMemberScore' and State = {2:d}", fId, reviewLevel,state);
            if (GetDataTable(sql).Rows.Count > 0)
            {
                return GetDataTable(sql).Rows[0].Field<string>("Value");
            }
            return "0";
        }

        public static string GetLimitNumber(string fId, int reviewLevel,int state)
        {
            string sql = string.Format("Select Value From ReviewConfig Where FID = '{0}' and  _Index = {1:0} and CID = 'LimitNumber' and State = {2:d}", fId, reviewLevel,state);
            if (GetDataTable(sql).Rows.Count > 0)
            {
                return GetDataTable(sql).Rows[0].Field<string>("Value");
            }
            return "0";
        }
        public static string GetAddLimitNumber(string fId, int reviewLevel, int state)
        {
            string sql = string.Format("Select Value From ReviewConfig Where FID = '{0}' and  _Index = {1:0} and CID = 'AdditionLimitNumber' and State = {2:d} ", fId, reviewLevel, state);
            if (GetDataTable(sql).Rows.Count > 0)
            {
                return GetDataTable(sql).Rows[0].Field<string>("Value");
            }
            return "0";
        }





        // TODO
        public static DataTable GetAllScoreItem(string fId, int reviewLevel)
        {
            string sql = string.Format("Select * From ScoreItem WHERE FID = '{0}' AND _Index <= {1:d} ORDER BY Seq", fId, reviewLevel);
            return GetDataTable(sql);
        }

        public static DataTable GetExistReviewScore(string fId, int state, int _index)
        {
            string sql = string.Format("SELECT TOP 1 R.RID from ReviewProject R left join Project P on R.PID=P.PID " +
                "where FID = {0} AND R.State = {1:d} AND R._Index = {2:d}", fId, state, _index);
            return GetDataTable(sql);
        } 

        #endregion

        #region EntityItem
        /// <summary>
        /// Get fields for displaying.
        /// </summary>
        /// <param name="fId">Form ID</param>
        /// <returns></returns>
        public static DataTable GetFormDisplayItemName(string fId)
        {
            string sql = string.Format("SELECT e.IID, e.ItemName, Seq  FROM DisplayListName a left join " +
                            "EntityItemForDisplay d on a.DID = d.DID LEFT JOIN EntityItem e ON d.IID = e.IID " +
                            "WHERE a.FID = '{0}' and A.[Current] = 'True' and d.DID is not null ORDER BY Seq", fId);
            //string sql = string.Format(
            //    "SELECT e.IID, e.ItemName, Seq " +
            //    "FROM EntityItemForDisplay d LEFT JOIN EntityItem e ON d.IID = e.IID " +
            //    "WHERE d.FID = '{0}' " +
            //    "ORDER BY Seq", fId);
            return GetDataTable(sql);
        }

        /// <summary>
        /// Get displaying items.
        /// </summary>
        /// <param name="fId">Form ID</param>
        /// <param name="pId">Project ID</param>
        /// <returns></returns>
        public static DataTable GetDisplayItem(string fId, int pId, int vId)
        {
            //string sql = string.Format("Select a.*, c.*, d.PID, d._Content From DisplayListName a left join EntityItemForDisplay c on a.DID = c.DID " + 
            //                " Left Join UsersFormRecord d On c.IID = d.IID Where a.FID = '{0}' And a.[Current] = 'True' And d.PID = {1} And d.VID={2:d} Order By c.Seq", fId, pId,vId);
            //string sql = string.Format("Select c.*, d.PID, d._Content From EntityItemForDisplay c Left Join UsersFormRecord d On c.IID = d.IID " +
            //                        "Where c.FID = '{0}' And d.PID = {1} Order By c.Seq", fId, pId);

            string sql = string.Format("Select * From UsersFormRecord  Where PID = {0:d} And VID= {1:d} ", pId, vId);
            return GetDataTable(sql);
        }

        public static int SetDefaultDisplayItem(string fId)
        {
            string Getsql = string.Format("Select * from DisplayListName where FID = '{0}' and Name = '預設' ", fId);
            if (GetDataTable(Getsql).Rows.Count > 0) return 0;
            string sql = string.Format("Insert into DisplayListName VALUES( '{0}', '預設', 1 ) ", fId);
            return InsertData(sql);
        }

        public static DataTable GetRecordCategory(string fId)
        {
            string sql = string.Format("Select A.CID, A.Seq, B.CategoryName from RecordFormCategory A " +
                                    "JOIN EntityCategory B on A.CID = B.CID where FID = '{0}'", fId);
            return GetDataTable(sql);
        }
        public static DataTable GetMultiRecordCategory(int mfId)
        {
            string sql = string.Format("Select A.CID, A.Seq, B.CategoryName from RecordMultiFormCategory A " +
                                    "JOIN EntityCategory B on A.CID = B.CID where MFID = '{0}'", mfId);
            return GetDataTable(sql);
        }

        public static DataTable GetMultiRecordCategory(int mfId, int egId)
        {
            string sql = string.Format("Select A.CID, A.Seq, B.CategoryName from RecordMultiFormCategory A " +
                                    "JOIN EntityCategory B on A.CID = B.CID where MFID = '{0}' AND EGID = {1:d}", mfId, egId);
            return GetDataTable(sql);
        }
        public static DataTable GetRecordItem(string fId, int cId )
        {
            string sql = string.Format("Select A.IID, A.Seq, B.ItemName from RecordFormItem A " + 
                                    "LEFT JOIN EntityItem B on A.IID = B.IID where FID = '{0}' and B.CategoryID = {1:d}", fId, cId);
            return GetDataTable(sql);
        }

        public static DataTable GetMultiRecordItem(int mfId, int cId)
        {
            string sql = string.Format("Select A.IID, A.Seq, B.ItemName from RecordMultiFormItem A " +
                                    "LEFT JOIN EntityItem B on A.IID = B.IID where MFID = '{0}' and B.CategoryID = {1:d}", mfId, cId);
            return GetDataTable(sql);
        }

        #endregion

        #region Project
        /// <summary>
        /// Get score of Project.
        /// </summary>
        /// <param name="pId">Project ID</param>
        /// <param name="reviewerId">Reviewer ID</param>
        /// <param name="reviewLevel">Review Level</param>
        /// <returns></returns>
        public static DataTable GetProjectScore(int pId, int reviewerId, int reviewLevel)
        {
            string sql = string.Format("Select * From ProjectScore Where RID In (Select RID From ReviewProject Where PID = {0:d} And UID = {1:d} And _Index = {2:d})", pId, reviewerId, reviewLevel);
            return GetDataTable(sql);
        }

        /// <summary>
        /// Get score of Project.
        /// </summary>
        /// <param name="rId">Review ID</param>
        /// <returns></returns>
        public static DataTable GetProjectScore(int rId)
        {
            string sql = string.Format("Select * From ProjectScore Where RID = {0:d}", rId);
            return GetDataTable(sql);
        }
        public static int GetProjectTotalScore(int rId)
        {
            string sql = string.Format("Select ISNULL(SUM(Score), 0) as Score From ProjectScore Where RID = {0:d}", rId);
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0].Field<int>("Score");
            }
            return -1;
        }

        /// <summary>
        /// Save Project Score
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddProjectScore(ModifyScoreM model,int state)
        {
            bool ret = false;
            int tmp = 0;

            lock (SCORE_TRANSACTION)
            {
                string sql = string.Format("Insert Into ReviewProject(PID, UID, _INDEX, Opinion,State, Finished) Values({0:d}, {1:d}, {2:d}, '{3}',{4:d},'{5}')",
                    model.PID, model.ReviewerId, model.ReviewLevel, model.Opinion, state, model.Finished);
                tmp = InsertData(sql);
                if (tmp > 0)
                {
                    int rId = GetReviewId(model.PID, model.ReviewerId, model.ReviewLevel, state);
                    // Batch operation
                    BegineTransaction(SCORE_TRANSACTION);
                    foreach (ProjectScoreM score in model.ScoreList)
                    {
                        sql = string.Format("Insert Into ProjectScore Values({0:d}, {1:d}, {2:d})", rId, score.SIID, score.Score);
                        BatchInsertData(SCORE_TRANSACTION, sql);
                    }

                    //專案計劃教學
                    if ( !string.IsNullOrEmpty(model.AdditionScore ) && !string.IsNullOrEmpty(model.AdditionOpinion))
                    {
                        sql = string.Format("Insert Into AdditionScore Values({0:d}, '{1}', '{2}')", rId, model.AdditionScore, model.AdditionOpinion);
                        BatchInsertData(SCORE_TRANSACTION, sql);
                    }

                    ret = CommitTransaction(SCORE_TRANSACTION);
                    EndTransaction(SCORE_TRANSACTION);
                }
            }

            return ret;
        }

        /// <summary>
        /// Update ProjectScore
        /// </summary>
        /// <param name="rId">Review ID</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateProjectScore(int rId, ModifyScoreM model)
        {
            bool ret = false;
            int tmp = 0;

            string sql = string.Format("Update ReviewProject Set Opinion = '{1}', Finished = '{2}' Where RID = {0:d}", rId, model.Opinion, model.Finished);
            tmp = UpdateData(sql);
            if (tmp > 0)
            {
                // Batch operation
                BegineTransaction(SCORE_TRANSACTION);
                foreach (ProjectScoreM score in model.ScoreList)
                {
                    sql = string.Format("Update ProjectScore Set Score = {2:d} Where RID = {0:d} And SIID = {1:d}", rId, score.SIID, score.Score);
                    BatchInsertData(SCORE_TRANSACTION, sql);
                }

                //專案計劃教學
                if (!string.IsNullOrEmpty(model.AdditionScore) && !string.IsNullOrEmpty(model.AdditionOpinion))
                {
                    if (GetDataTable(string.Format("Select * from AdditionScore where RID = {0:d}", rId))?.Rows.Count > 0)
                        sql = string.Format("Update AdditionScore Set Score = '{1}', Opinion = '{2}' Where RID = {0:d} ", rId, model.AdditionScore, model.AdditionOpinion);
                    else
                        sql = string.Format("Insert Into AdditionScore Values({0:d}, '{1}', '{2}')", rId, model.AdditionScore, model.AdditionOpinion);
                    BatchInsertData(SCORE_TRANSACTION, sql);
                }

                ret = CommitTransaction(SCORE_TRANSACTION);
                EndTransaction(SCORE_TRANSACTION);
            }
            return ret;
        }

        public static int DeleteProjectScore(int rId)
        {
            string sql = string.Format("Delete From ProjectScore Where RID = '{0}'", rId);
            return DeleteData(sql);

        }
        public static int DeleteReviewProject(int rId)
        {
            string sql = string.Format("Delete From ReviewProject Where RID = '{0}'", rId);
            return DeleteData(sql);

        }

        public static int DeleteAdditionScore(int rId)
        {
            string sql = string.Format("Delete From AdditionScore Where RID = '{0}'", rId);
            return DeleteData(sql);

        }
        */

    /// <summary>
    /// 取得顯示的ID
    /// </summary>
    /// <param name="pId">Project ID</param>
    /// <returns>Displaying ID</returns>
    /// 
    /*
    public static int GetDisplayingProjectId(int pId)//string formID, string owner, string name
        {
            int id = 0;
            string sql = string.Format("Select * From [Project] Where PID = {0:d}", pId);
            DataTable tbl = GetDataTable(sql);
            if (tbl.Rows.Count > 0)
            {
                id = int.Parse(tbl.Rows[0]["DisplayID"].ToString());
            }
            tbl.Dispose();

            return id;
        }

        /// <summary>
        /// Get all reviewed Projects.
        /// </summary>
        /// <param name="fId">Form ID</param>
        /// <returns></returns>
        public static DataTable GetAllReviewProject(string fId,int state)
        {
            string sql = string.Format("Select d.Name, c.* From " +
                "(Select x.*, y.DisplayID From ReviewProject x Left Join [Project] y On x.PID = y.PID Where y.FID = '{0}' AND x.State={1:d} AND x.Finished = 'true' ) c " +
                "Left Join[User] d On c.UID = d.UID Order By _Index, c.DisplayID, c.UID", fId, state);
            return GetDataTable(sql);
        }

        public static DataTable GetAllProjectWithFID(string fId)
        {
            string sql = string.Format("Select PID, DisplayID from [Project] where FID ='{0}' ", fId);
            return GetDataTable(sql);
        }

        public static DataTable GetAllProjectWithFID(string fId, int pId)
        {
            string sql = string.Format("Select PID, DisplayID from [Project] where FID ='{0}' and PID = {1:d} ", fId, pId);
            return GetDataTable(sql);
        }

        /// <summary>
        /// Get reviewed Projects.
        /// </summary>
        /// <param name="fId">Form ID</param>
        /// <param name="reviewerId">Reviewer ID</param>
        /// <param name="reviewerLevel">Reviewer Level</param>
        /// <returns></returns>
        public static DataTable GetReviewProject(string fId, int reviewerId, int reviewerLevel,int state)
        {
            string sql = string.Format("Select d.Name, c.* From " +
                "(Select x.*, y.DisplayID From ReviewProject x Left Join [Project] y On x.PID = y.PID Where y.FID = '{0}' And x.State={3:d} And (x._Index < {2} Or (x.UID = {1:d} And x._Index = {2}))) c " +
                "Left Join[User] d On c.UID = d.UID Order By _Index, c.DisplayID", fId, reviewerId, reviewerLevel, state);
            return GetDataTable(sql);
        }
        public static DataTable GetGroupByPID(int pId, int uId, int state,int index )
        {
            string sql = string.Format("Select distinct G.GID From [Group]G left join ProjectGroup P on G.GID=P.GID left join ReviewerGroup R on P.GID=R.GID "+
                "where  P.PID= {0:d} AND R.UID={1:d} AND G.State={2:d} AND G._Index={3:d}", pId, uId, state, index);
            return GetDataTable(sql);
        }
        /// <summary>
        /// Get Review ID
        /// </summary>
        /// <param name="pId">Project ID</param>
        /// <param name="reviewerId">Reviewer ID</param>
        /// <param name="reviewLevel">Review Level</param>
        /// <returns>Review ID</returns>
        public static int GetReviewId(int pId, int reviewerId, int reviewLevel,int state)
        {
            int reviewId = 0;
            string sql = string.Format("Select * From ReviewProject Where PID = {0:d} And UID = {1:d} And _Index = {2:d} And State={3:d}", pId, reviewerId, reviewLevel,state);
            DataTable tbl = GetDataTable(sql);
            if (tbl.Rows.Count > 0) {
                reviewId = tbl.Rows[0].Field<int>("RID");
            }
            tbl.Dispose();

            return reviewId;
        }

        /// <summary>
        /// Get reviewer's projects.
        /// </summary>
        /// <param name="fId">Form ID</param>
        /// <param name="reviewerId">Reviewer ID</param>
        /// <param name="reviewLevel">Review Level</param>
        /// <returns>Project IDs</returns>
        public static DataTable GetReviewProjectId(string fId, int reviewerId, int reviewLevel,int state)
        {
            // 此標記的語法不包含ReviewLevel, 暫保留
            //string sql = string.Format("Select PID, DisplayID From [Project] Where PID In (Select PID From ProjectGroup Where GID In " +
            //    "(Select GID From ReviewerGroup Where UID = {1:d} And GID In(Select GID From [Group] Where FID = '{0}')))", fId, reviewerId);
            string sql = string.Format("Select PID, DisplayID From [Project] Where PID In (Select PID From ProjectGroup Where GID In " +
                "(Select GID From ReviewerGroup Where UID = {1:d}  And GID In(Select GID From [Group] Where FID = '{0}' And _Index = {2:d} And State ={3:d})))", fId, reviewerId, reviewLevel, state);
            return GetDataTable(sql);
        }

        public static DataTable GetReviewProjectIdWithGroup(string fId, int reviewerId, int reviewLevel)
        {
            string sql = string.Format("Select c.PID, DisplayID, d.GID, e.Name " +
                "From(Select * From[Project] Where FID = '{0}') c Left Join ProjectGroup d On c.PID = d.PID " +
                "Right Join(Select * From[Group] Where FID = '{0}' And _Index = {2:d} And GID In(Select GID From ReviewerGroup Where UID = {1:d})) e On d.GID = e.GID ", fId, reviewerId, reviewLevel);
            return GetDataTable(sql);
        }

        public static DataTable GetAllReviewProjectId(string fId)
        {
            string sql = string.Format("Select PID, DisplayID " +
                "From [Project] " +
                "Where FID = '{0}'", fId);
            return GetDataTable(sql);
        }

        public static DataTable GetAllReviewProjectIdWithGroup(string fId,int state)
        {
            //string sql = string.Format("Select c.PID, DisplayID, AssignedID, ISNULL(e.GID, 0) AS GID, ISNULL(e.Name, '') AS Name " +
            //"From [Project] c Left Join ProjectGroup d On c.PID = d.PID Left Join [Group] e On d.GID = e.GID And e.State={1:d} " +
            //"Where c.FID = '{0}'", fId,state);
            string sql = string.Format("Select c.PID, DisplayID, AssignedID, ISNULL(e.GID, 0) AS GID,ISNULL(e.Name, '') AS Name " +
            "From [Project] c Left Join ( Select A.GID, ISNULL(A.Name, '') AS Name , PID from [Group] A left join ProjectGroup B on A.GID = B.GID " +
            "where A.State = {1:d} ) e on c.PID = e.PID Where c.FID = '{0}'", fId, state);
            return GetDataTable(sql);

        }

        public static DataTable GetProjectReviewerS(int pId) {
            string sql = string.Format("Select f.UID, f.Name, e.RID From (Select * From ProjectGroup Where PID = {0:d}) c " +
                "Left Join ReviewerGroup d On c.GID = d.GID " + 
                "Left Join ReviewProject e On c.PID = e.PID And d.UID = e.UID " + 
                "Left Join[User] f On d.UID = f.UID ", pId);
            return GetDataTable(sql);
        }

        public static DataTable GetProjectReviewerS(int pId, int gId) {
            string sql = string.Format("Select f.UID, f.Name, e.RID From (Select UID From ReviewerGroup Where GID = {1:d}) d " +
                "Left Join(Select * From ReviewProject Where PID = {0:d} And State=(Select State From [Group] where GID={1:d} ) and _Index=(Select _Index From [Group] where GID={1:d}  ))  e On d.UID = e.UID " + 
                "Left Join[User] f On d.UID = f.UID", pId, gId);
            return GetDataTable(sql);
        }

        //Delete UsersRepeatEntityRecord PID =pId的資料
        public static int DeleteUsersRepeatEntityRecord(string pId)
        {
            string sql = string.Format("Delete From UsersRepeatEntityRecord Where PID = '{0}'", pId);
            return DeleteData(sql);
        }

        //Delete Project PID =pId的資料
        public static int DeleteProjectByPID(string pId)
        {
            string sql = string.Format("Delete From [Project] Where PID = '{0}'", pId);
            return DeleteData(sql);
        }

        public static bool CheckUserProjectNum(string fId, int uId)
        {
            string sql = string.Format("Select PID From [Project] Where FID = '{0}' AND UID = {1:d}", fId, uId);
            DataTable dbProject = GetDataTable(sql);
            if(dbProject.Rows.Count > 0)
            {
                return true; //還有申請案
            }

            string sql2 = string.Format("Select U.PID From [ProjectUser] U left join [Project] P on U.PID=P.PID  Where P.FID = '{0}' AND U.UID = {1:d}", fId, uId);
            DataTable dbProjectUser = GetDataTable(sql2);
            if (dbProjectUser.Rows.Count > 0)
            {
                return true; //還有申請案
            }
            return false;
        }

        public static int DeleteApplicant(string fId, int uId)
        {
            string sql = string.Format("Delete From UserIdentification Where FID = '{0}' AND UID = {1:d} AND (Identification & {2:d}) == {2:d}", fId, uId,CsConstants.AUTH_APPLICANT);
            return DeleteData(sql);
        }

        public static int DeleteIdentity(string fId, int uId, int TransedIdentity)
        {
            int userIdentification = 0;
            string sql = string.Format("Select * from UserIdentification where FID = '{0}' AND UID =  {1:d} ", fId, uId);
            if (GetDataTable(sql).Rows.Count > 0)
            {
                userIdentification = GetDataTable(sql).Rows[0].Field<int>("Identification");
                                
            }
           

            if ((userIdentification & TransedIdentity) != TransedIdentity) return 0;
            int NewIdentity = (userIdentification ^ TransedIdentity);
            if(NewIdentity == 0)
            {
                sql = string.Format("Delete From UserIdentification Where FID = '{0}' AND UID = {1:d} ", fId, uId);
                return DeleteData(sql);
            }
            else
            {
                sql = string.Format("Update  UserIdentification Set Identification = {2:d} Where FID = '{0}' AND UID = {1:d} ", fId, uId, NewIdentity);
                return InsertData(sql);
            }
            
        }
        public static int UpdateProjectTransferID(string fId, string PID ,int uId , int transferedUID)
        {
            string sql = string.Format("Update [Project] Set UID = {3:d} where  FID = '{0}' AND PID = {1:d} ", fId, PID, uId, transferedUID);
            return InsertData(sql);

        }

        public static int UpdateAssignedID(ReviewProjectItemM model)
        {
            string sql = string.Format("Update [Project] Set AssignedID = '{1}' where PID = {0:d}", model.PId, model.AssignedId);
            return InsertData(sql);
        }

        public static DataTable CheckProjectPrincipal(int pId,int uId)
        {
            string sql = string.Format("Select PID from  Project where PID = {0:d} AND UID = {1:d}", pId, uId);
            return GetDataTable(sql);

        }
        

        public static int SetProjectUser(int pId, int uId,int init,string CanEdit)
        {
            string Getsql = string.Format("Select MAX(Seq) as Seq from ProjectUser where PID = {0:d}", pId);
            DataTable dt = GetDataTable(Getsql);
            int Seq = string.IsNullOrEmpty(  dt.Rows[0]["Seq"].ToString() ) ? init : dt.Rows[0].Field<int>("Seq")  + 1 ;
            string sql = string.Format("Insert Into ProjectUser(PID,UID,Seq,CanEdit) VALUES({0:d},{1:d},{2:d},'{3}' )", pId, uId, Seq, CanEdit);
            return InsertData(sql);
        }
        public static int UpdateProjectUser(int pId, int uId, string CanEdit, string OnlyRead,string Unable)
        {       
            string sql = string.Format("Update ProjectUser Set CanEdit = '{2}',OnlyRead='{3}',Unable='{4}' Where PID = {0:d} AND UID={1:d}", pId, uId, CanEdit, OnlyRead, Unable);
            return InsertData(sql);
        }
        public static DataTable GetProjectUser(int pId, int start = 0, int end = 0 )
        {
            if ( start == end && end == 0 )
            {
                string sql = string.Format("Select * from  ProjectUser A left join elfess_User B on A.UID = B.UID left join ApplicantState C on B.UID = C.UID and A.PId=C.PID  where A.pId = {0:d} order by Seq desc ", pId );
                return GetDataTable(sql);
            }
            
            else
            {
                string sql = string.Format("Select * from  ProjectUser A left join elfess_User B on A.UID = B.UID " +
                                        " where A.pId = {0:d} And Seq >= {1:d} And Seq <= {2:d} order by Seq desc ", pId, start, end );
                return GetDataTable(sql);
            }
        }

        public static DataTable GetProjectUserFromProject(int pId)
        {
                string sql = string.Format("Select A.*, B.*, C.State from  Project A left join elfess_User B on A.UID = B.UID left join ApplicantState C on A.PID = C.PID where A.pId = {0:d} ", pId );
                return GetDataTable(sql);
            
        }

        public static int DeleteProjectUser(int pId, int uId)
        {
            string sql = string.Format("DELETE from ProjectUser where PID = '{0}' AND UID = {1:d} ", pId, uId);
            return DeleteData(sql);
        }

        public static DataTable GetAdditionScore(int rId)
        {
            string sql = string.Format("Select * From AdditionScore Where RID = {0:d}", rId);
            return GetDataTable(sql);
        }

        public static DataTable GetProjectModify(int PID )
        {
            string sql = string.Format("Select * from ProjectModify where PID = {0:d} ", PID);
            return GetDataTable(sql);
        }

        public static DataTable GetProjectModifyDate(int PID)
        {
            string sql = string.Format("Select * from ProjectModify where PID = {0:d} ", PID);
            return GetDataTable(sql);
        }
        public static int SetProjectModify(ProjectModifyM project)
        {
            string sql = string.Format("Select * from ProjectModify where PID = {0:d} AND Type = {1:d}", project.PID, project.Type );
            if(GetDataTable(sql).Rows.Count > 0)
            {
                sql = string.Format("Update ProjectModify Set Host = '{2}', Unit = '{3}', " +
                    "Description = '{4}', Date = '{5}', NFID = '{6}' where PID = {0:d} AND Type = {1:d} ", project.PID, project.Type, project.Host, project.Unit, project.Description, project.Date, project.NFID);
            }

            else
            {
                sql = string.Format("Insert into ProjectModify (PID, Type, Host, Unit, Description, Date, NFID ) VALUES " +
                                "( {0:d}, {1:d},'{2}', '{3}', '{4}', '{5}', '{6}' )", project.PID, project.Type, project.Host, project.Unit, project.Description, project.Date, project.NFID);
            }

            return InsertData(sql);
        }
        public static int DeleteProjectModify(ProjectModifyM project)
        {
            string sql = string.Format("Delete from ProjectModify where PID = {0:d} ", project.PID);
            return DeleteData(sql);
        }


        public static int TransferFIDByPID(string fId, int pId )
        {
            string sql = string.Format("Update [Project] set FID = '{0}' where PID = {1:d} ", fId, pId);
            return InsertData(sql);
        }

        public static int SaveHistoryProject(string fId)
        {
            string sql = string.Format("Insert into HistoryProject (PID, UID, FID ) Select PID, UID, FID from [Project] where FID = '{0}' ", fId);
            return InsertData(sql);
        }
        public static int UpadateHistoryProjectData(int pId, int iId)
        {
            string sql = string.Format("Update HistoryProject Set HostName = (Select Top 1 [_Content] from UsersFormRecord where PID = {0:d}  AND IID = {1:d} order by VID desc ) where PID = {0:d} ", pId, iId);
            return InsertData(sql);
        }

        public static int UpadateHistoryProjectFile(int pId, string name)
        {
            string sql = string.Format("Update HistoryProject Set FileName = '{1}' where PID = {0:d} ", pId, name);
            return InsertData(sql);
        }

        

        // TODO: 
        /*
        public static DataTable GetExamProject(string formID, string examinerID, int ExamLevel)
        {
            string sql;
            if (examinerID.Equals(""))
            {
                sql = string.Format("SELECT * FROM ExamineProject WHERE EditIndex = {0:d}", ExamLevel);
            }
            else
            {
                //sql = string.Format("SELECT a.* FROM ExamineProject a RIGHT JOIN " +
                //    "(SELECT Distinct FormID, userID FROM ExamineProject WHERE ExaminerID = '{0}' And FormID = '{2}') b ON a.FormID = b.FormID And a.userID = b.userID " +
                //    "WHERE EditIndex = {1:d}", examinerID, ExamLevel, formID);
                sql = string.Format("SELECT a.* FROM ExamineProject a RIGHT JOIN " +
                    "(Select UserID From GroupMember Where Type = 'Applicant' And " +
                    "GroupID In (Select GroupID From GroupMember Where UserID = '{0}')) b ON a.FormID = '{2}' And a.userID = b.userID " +
                    "WHERE EditIndex = {1:d}", examinerID, ExamLevel, formID);
            }
            sql = string.Format("SELECT B.ExaminerName, A.* FROM ({0}) AS A LEFT JOIN elfess_Examiner AS B ON A.ExaminerID = B.userID ", sql);
            return GetDataTable(sql);
        }
        */

   







}

