using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CsDynamicConstants 的摘要描述
/// This class is to store all the Constants that are likely to change depending on the situation.
/// </summary>
public class CsDynamicConstants 
{
#region The folder that keeps the AIQ for "Knee" 
    //the file path of the AIQ XML folder that will be accessed by browser and .Net Server.
    public static string relativeKneeXMLFolder = "IPC_Questions/1161-1450/";

    //the file path of the AIQ XML folder that will be accessed by 3DBuilder.exe .
    //It will access "~\\AIQ\\IPC_Questions\\1161-1450\\";
    public static string absoluteKneeXMLFolder = HttpContext.Current.Request.MapPath("~/").Replace(@"\", @"\\") + "IPC_Questions\\1161-1450\\";
   
    //this is organ xml file contains all the organs in a certain body part.
    //e.g., ./IPC_Question_Origin/SceneFile_ex.xml stores all the organs in Knee.
    public static string completeKneeOrgansXMLPath = "./IPC_Question_Origin/SceneFile_Knee_ex.xml";
#endregion


    /*
     * ####以後支援讓老師出更多身體部位的AIQ時，需加入的程式:
     * @@@@注意!! 之後須到AITypeQuestion_EditingPage.aspx.cs中的function "decide_QuestionBodyPartOrganXML" 中
     * 加入讓系統可讓老師出更多身體部位的AIQ時的程式
     if there are more body parts available in the future, 
     you have to create a new region and add another 3 file paths like what is done to the "Knee" as shown above.
     e.g., 
     if the "Neck" is available for the AIQ
     #region The folder that keeps the AIQ for "Neck" 
        //the file path of the AIQ XML folder that will be accessed by browser and .Net Server.
        public static string relativeNeckXMLFolder = "IPC_Questions/[Neck project folder]/";

        //the file path of the AIQ XML folder that will be accessed by 3DBuilder.exe .
        //It will access "~\\AIQ\\IPC_Questions\\[Neck project folder]\\";
        public static string absoluteNeckXMLFolder = HttpContext.Current.Request.MapPath("~/").Replace(@"\", @"\\") + "IPC_Questions\\[Neck project folder]\\";
   
        //this is organ xml file contains all the organs in a certain body part.
        //e.g., ./IPC_Question_Origin/[Neck_project.xml] stores all the organs in Neck.
        public static string completeNeckOrgansXMLPath = "./IPC_Question_Origin/[Neck_project.xml]";
    #endregion
     
     */


    //the download link of the 3DBuilder RemoteDeskTopRDPFile.
    public static string RemoteDeskTopRDPFile_For3DBuilder_DownloadLink="https://drive.google.com/open?id=1h7QUiN2iXEKbTzMXn4UWj-c56tfoAO6i";

}