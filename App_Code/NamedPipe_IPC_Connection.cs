using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

/// <summary>
/// NamedPipe_IPC_Connection 的摘要描述
/// </summary>
public class NamedPipe_IPC_Connection : CsSessionManager
{

    public  Process CSNamedPipeProcess = new Process();
    public  string CSNamedPipeWorkingDir;
    public  string CSNamedPipeFileName;
    public StreamWriter CSNamedPipeWriter;
    public StreamReader CSNamedPipeReader;
    //public string CSNamedPipeReader;
    public string CSNamedPipePID;
    public string HintsUserID;



    public NamedPipe_IPC_Connection(string namedPipeWorkingDir, string namedpipeFileName, string strUserID)
	{
        CSNamedPipeWorkingDir = namedPipeWorkingDir;
        CSNamedPipeFileName = namedpipeFileName;
        HintsUserID = strUserID;

        //activate the CSNamedPipe.exe
        activateCSNamedPipe();

        //set the reader, writer ,PID of the CSNamedPipe.exe 
        setNamedPipeReadWriter_PID();
	}

    //activate the CSNamedPipe.exe
    private void activateCSNamedPipe()
    {
        //originating from Default.aspx
        //Process os = new Process();
        

        CSNamedPipeProcess.StartInfo.WorkingDirectory = CSNamedPipeWorkingDir;
        CSNamedPipeProcess.StartInfo.FileName = CSNamedPipeFileName;
        CSNamedPipeProcess.StartInfo.UseShellExecute = false;

        //redirect the standard input of the CSNamedPipe.exe so that we can write messages that we want to send to the 3DBuilder
        //on AIQ web pages.
        CSNamedPipeProcess.StartInfo.RedirectStandardInput = true;

        //redirect the standard output of the CSNamedPipe.exe so that we can read messages that we sent from the 3DBuilder
        //on AIQ web pages.
        CSNamedPipeProcess.StartInfo.RedirectStandardOutput = true;
        
        /*
        os.StartInfo.Arguments = hintID;
        */


        //pass Hints's userID to CSNamedPipe.exe as the name of the namedPipe.
        CSNamedPipeProcess.StartInfo.Arguments = HintsUserID;
        CSNamedPipeProcess.Start();


    }


    //set the reader, writer ,PID of the CSNamedPipe.exe 
    private void setNamedPipeReadWriter_PID()
    {

        //store the StreamWriter of the CSNamedPipe.exe to a session variable
        //for writing message to CSNamedPipe.exe, and CSNamedPipe.exe will send it to the 3DBuilder.
        CSNamedPipeWriter = CSNamedPipeProcess.StandardInput;

        //store the StreamReader of the CSNamedPipe.exe to a session variable
        //for reading message from CSNamedPipe.exe 
        //because 3DBuilder can send message to the CSNamedPipe.exe with named pipe.
        CSNamedPipeReader = CSNamedPipeProcess.StandardOutput;

        

        //get process ID of the CSNamedPipe, and store it in a session var so that we can kill the CSNamedPipe process after the user finishes using the connection with 3DBuilder
        CSNamedPipePID = CSNamedPipeProcess.Id.ToString();


        //store the StreamWriter of the CSNamedPipe.exe to a session variable
        //for writing message to CSNamedPipe.exe, and CSNamedPipe.exe will send it to the 3DBuilder.       
        CSNamedPipeStreamWriter_Session = CSNamedPipeWriter;

        //store the StreamReader of the CSNamedPipe.exe to a session variable
        //for reading message from CSNamedPipe.exe 
        //because 3DBuilder can send message to the CSNamedPipe.exe with named pipe.      
        CSNamedPipeStreamReader_Session = CSNamedPipeReader;

        //store the process of CSNamedPipe.exe to a session variable  
        //so that we can access it in other AIQ pages.
        CSNamedPipeProcess_Session = CSNamedPipeProcess;

        //get process ID of the CSNamedPipe, and store it in a session var so that we can kill the CSNamedPipe process after the user finishes using the connection with 3DBuilder
        CSNamedPipePID_Session = CSNamedPipePID;
    }


    //send message through CSNamedPipe.exe to the corresponding 3DBuilder.
    public static void sendMsg23DBuilder(string contact)
    {


        //send cmd1
        try
        {

            StreamWriter wr = (StreamWriter)CSNamedPipeStreamWriter_Session;
            //StreamWriter wr = new StreamWriter((StreamWriter)Session["Writer"]);
            //StreamWriter wr = new StreamWriter((Stream )Session["Writer"], Encoding.UTF8, 4096, true);
            //send cmd2
            wr.WriteLine(contact);//!!!!!send update msg to 3DBuilder

            // the streamwriter WILL be closed and flushed here, even if an exception is thrown.

            //wr.Flush();
        }
        catch (Exception e)
        {

        }

    }

    //read message sent from the 3DBuilder from the CSNamedPipe.exe
    public static string readMsgFrom3DBuilder()
    {

        try
        {

            StreamReader rd = (StreamReader)CSNamedPipeStreamReader_Session;

            return rd.ReadLine();


        }
        catch (Exception e)
        {
            return "Read message from named pipe failed.";
        }
    }

    //wait for the 3DBuilder to finish initialization
    public static void sleepUntil3DBuilderFinishInit()
    {
        string messageFrom3DBuilder;
        int sleepTimeCounter = 0;

        while (true)
        {
            //increase sleep time counter
            sleepTimeCounter++;

            //read message sent from the 3DBuilder from the CSNamedPipe.exe
            messageFrom3DBuilder = NamedPipe_IPC_Connection.readMsgFrom3DBuilder();


            //use JS alert() in C# to alert "Read message from named pipe failed." when we failed to read message from the 3DBuilder.
            if (messageFrom3DBuilder == "Read message from named pipe failed.")
            {
                /*
                ScriptManager.RegisterStartupScript(
                 this,
                 typeof(Page),
                 "Alert",
                 "<script>alert('" + messageFrom3DBuilder + "');</script>",
                 false);
                */
                break;
            }
            else if (messageFrom3DBuilder == "3DBuilder init is finished.")
            {
                /*
                ScriptManager.RegisterStartupScript(
                 this,
                 typeof(Page),
                 "Alert",
                 "<script>alert('" + messageFrom3DBuilder + "');</script>",
                 false);
                */
                break;
            }

            //keep sleeping until we successfully /fail to get the message from the 3DBuilder or  
            Thread.Sleep(1);
        }
        /*
        //alert how long it slept
        ScriptManager.RegisterStartupScript(
                 this,
                 typeof(Page),
                 "Alert",
                 "<script>alert('The AIQ slept for " + sleepTimeCounter + " millisecond to wait for the 3DBuilder Init.');</script>",
                 false);
        */
    }


    //kill the corresponding running CsNamedPipe.exe process which is created when the teacher clicks "connect to 3DBuilder" to edit the AITypeQuestion in 3DBuilder.
    public static  void killCorrespondingCSNamedPipe()
    {
        //kill the corresponding running CsNamedPipe.exe process which is created when the teacher clicks "connect to 3DBuilder" to edit the AITypeQuestion in 3DBuilder.
        //kill process with processID
        Process[] procList = Process.GetProcesses();

        for (int i = 0; i < procList.Length; i++)
        {
            string pid = procList[i].Id.ToString();
            if (string.Equals(pid, CSNamedPipePID_Session))
            {
                procList[i].Kill();
            }
        }
    }


}