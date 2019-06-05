using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;


    public static class CsDBConnection
    {
        /*
        //connect to SCOREDB
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SCOREDB"].ToString();
        */

        //connect to NewVersionHintsDB
        private static string connectionHintsDBString = System.Configuration.ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ToString();
        
        //connect to MLASDB
        private static string connectionMLASDBString = System.Configuration.ConfigurationManager.ConnectionStrings["MLASDBConnectionString"].ToString();

        private static SqlConnection batchConn = new SqlConnection(connectionHintsDBString);
        private static Dictionary<string, SqlTransaction> batchTransation = new Dictionary<string, SqlTransaction>();

        public static int ExecuteNonQuery(SqlCommand cmd, string targetDB)
        {

            try
            {
                /*
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                int connInt = cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                 * */
                //we set the default target DB as NewVersionHintsDB
                cmd.Connection = new SqlConnection(connectionHintsDBString);

                //if the target DB is MLASDB, we connect to MLASDB
                if (targetDB == "MLASDB")
                {
                    cmd.Connection = new SqlConnection(connectionMLASDBString);
                }

                cmd.Connection.Open();
                int connInt = cmd.ExecuteNonQuery();
                

                return connInt;
            }

            catch (Exception e)
            {
                //今日日期
                DateTime Date = DateTime.Now;
                string TodyMillisecond = Date.ToString("yyyy-MM-dd HH:mm:ss");
                string Tody = Date.ToString("yyyy-MM-dd");

                //如果此路徑沒有資料夾
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~") + "\\SQLLog"))
                {
                    //新增資料夾
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + "\\SQLLog");
                }

                //把例外狀況寫到目的檔案，若檔案存在則附加在原本內容之後(換行)

                File.AppendAllText(string.Format(HttpContext.Current.Server.MapPath("~") + "\\SQLLog\\{0}.txt", Tody), string.Format("\r\n{0} ： 查詢函式 「{1}」\r\n\t\t\t錯誤內容「{2}」\r\n\r\n", TodyMillisecond, cmd.CommandText, e));
                //File.AppendAllText("J:\\Thisway_Log\\" + Tody + ".txt", "\r\n" + TodyMillisecond + "：" +  e);
                throw e;
            }

            //to release the resources when exception occurs.
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();

            }
            
            
        }

        /// <summary>
        /// Get a DataSet for the select query string.
        /// (ex: "SELECT * FROM table1 WHERE ...")
        /// The returned DataSet will contain one table only which has no table name.
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string strSQL,string targetDB)
        {
            //we set the default target DB as NewVersionHintsDB
            System.Data.SqlClient.SqlDataAdapter sda = new SqlDataAdapter(strSQL, connectionHintsDBString);


            //if the target DB is MLASDB, we connect to MLASDB
            if(targetDB=="MLASDB")
            {
               sda = new SqlDataAdapter(strSQL, connectionMLASDBString);
            }
           
            
            DataSet dsResult = new DataSet();
            try
            {
                //cmd.Connection.Open();
                sda.Fill(dsResult);

            }
            catch (Exception e)
            {
                //今日日期
                DateTime Date = DateTime.Now;
                string TodyMillisecond = Date.ToString("yyyy-MM-dd HH:mm:ss");
                string Tody = Date.ToString("yyyy-MM-dd");

                //如果此路徑沒有資料夾
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~") + "\\SQLLog"))
                {
                    //新增資料夾
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + "\\SQLLog");
                }

                //把例外狀況寫到目的檔案，若檔案存在則附加在原本內容之後(換行)

                File.AppendAllText(string.Format(HttpContext.Current.Server.MapPath("~") + "\\SQLLog\\{0}.txt", Tody), string.Format("\r\n{0} ： 查詢函式 「{1}」\r\n\t\t\t錯誤內容「{2}」\r\n\r\n", TodyMillisecond, strSQL, e));
                //File.AppendAllText("J:\\Thisway_Log\\" + Tody + ".txt", "\r\n" + TodyMillisecond + "：" +  e);
                throw e;
            }
            finally
            {
                //cmd.Connection.Close();
                sda.Dispose();
            }
            return dsResult;
        }



    #region transaction
    /*
    public static void BegineTransaction(string transactionName) {
            if (batchTransation.Count == 0)
            {
                batchConn.Open();
            }
            if (!batchTransation.ContainsKey(transactionName)) {
                SqlTransaction transaction;
                if (!batchTransation.TryGetValue(transactionName, out transaction)) {
                    batchTransation.Add(transactionName, batchConn.BeginTransaction(transactionName));
                }
            }
        }

        public static void EndTransaction(string transactionName)
        {
            SqlTransaction transaction;
            if (batchTransation.TryGetValue(transactionName, out transaction))
            {
                transaction.Dispose();
                batchTransation.Remove(transactionName);
                if (batchTransation.Count == 0) {
                    batchConn.Close();
                }
            }
        }

        public static bool CommitTransaction(string transactionName) {
            SqlTransaction transaction;
            bool ret = false;
            if (batchTransation.TryGetValue(transactionName, out transaction))
            {
                try
                {
                    transaction.Commit();
                    ret = true;
                }
                catch (Exception e) {
                    ret = false;
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (SqlException ex)
                    {
                    }
                }
            }

            return ret;
        }

        public static void RollbackTransaction(string transactionName) {
            SqlTransaction transaction;
            if (batchTransation.TryGetValue(transactionName, out transaction))
            {
                transaction.Rollback();
            }
        }

        public static int BatchExecuteNonQuery(string transactionName, string sql)
        {
            int connInt = 0;
            SqlTransaction transaction;
            if (batchTransation.TryGetValue(transactionName, out transaction))
            {
                try
                {
                    SqlCommand cmd = batchConn.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = sql;
                    connInt = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    //今日日期
                    DateTime Date = DateTime.Now;
                    string TodyMillisecond = Date.ToString("yyyy-MM-dd HH:mm:ss");
                    string Tody = Date.ToString("yyyy-MM-dd");

                    //如果此路徑沒有資料夾
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~") + "\\SQLLog"))
                    {
                        //新增資料夾
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + "\\SQLLog");
                    }

                    //把例外狀況寫到目的檔案，若檔案存在則附加在原本內容之後(換行)

                    File.AppendAllText(string.Format(HttpContext.Current.Server.MapPath("~") + "\\SQLLog\\{0}.txt", Tody), string.Format("\r\n{0} ： 查詢函式 「{1}」\r\n\t\t\t錯誤內容「{2}」\r\n\r\n", TodyMillisecond, sql, e));
                    //File.AppendAllText("J:\\Thisway_Log\\" + Tody + ".txt", "\r\n" + TodyMillisecond + "：" +  e);
                    throw e;
                }
                
            }
            return connInt;
        }
        */
    #endregion

}

