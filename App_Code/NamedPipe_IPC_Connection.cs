using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// NamedPipe_IPC_Connection 的摘要描述
/// </summary>
public class NamedPipe_IPC_Connection
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
        string hintID = "5555";//this hintID is hard-coded by 昇宏學長

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
    
    }

}