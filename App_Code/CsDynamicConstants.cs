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
    //the file path of the AITypeQuetion XML folder that will be accessed by browser and .Net Server.
    public static string AITypeQuestionXMLFolder = "IPC_Questions/1161-1450/";

    //the file path of the AITypeQuetion XML folder that will be accessed by 3DBuilder.exe .
    public static string ThreeDBuilderXMLFolder = "D:\\IPC_interact_with_3DBuilder\\IPC_Questions\\1161-1450\\";

    //this is organ xml file contains all the organs in a certain body part.
    //e.g., ./IPC_Question_Origin/SceneFile_ex.xml stores all the organs in Knee.
    public static string completeBodyPartOrgansXMLPath = "./IPC_Question_Origin/SceneFile_ex.xml";

}