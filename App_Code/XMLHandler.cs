using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;



public class XMLHandler
{
    XmlDocument xmlDoc;
    XElement xDoc;

    public XMLHandler(string XMLSrcPath)
    {
        xmlDoc = new XmlDocument();
        
        //load all the organ of the corresponding part of body.
        xmlDoc.Load(XMLSrcPath);


    }

    //get  values from specific Tag name with specific value
    public List<string> getValueOfSpecificTagName(string targetTagName,string SpecificTagName, string SpecificValue)
    {
        //to contain the value retrieved.
        List<string> retrievedValueList = new List<string>();
       
        //var target = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element("Question").Value == "Yes").Single();
        var targets = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element(SpecificTagName).Value == SpecificValue);

        foreach (var node in targets)
        {
            retrievedValueList.Add(node.Element(targetTagName).Value);
        }
        return retrievedValueList;
    }


    //get the number of the organ whose Question tag is "Yes"
    public int[] getPickedQuestionNumber()
    {
        //get  values from tag "Number" whose tag "Question" is marked "Yes"
        List<string> strPickedQuesionNumber = getValueOfSpecificTagName("Number", "Question", "Yes");
        
        
        //Convert the string list to int array.
        int elementNumInList=strPickedQuesionNumber.Count;
        int[] pickedQuestions = new int[elementNumInList];
        for (int i = 0; i < elementNumInList; i++)
        {
            pickedQuestions[i] = Int32.Parse(strPickedQuesionNumber[i]);

        }
        return pickedQuestions;
 
    }
    
    


     //set the checked organs as Questions by setting its Question tag to "Yes"
    public void setAsQuestion( Label OrganName)
    {
       

        var target = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element("Name").Value == OrganName.Text).Single();



        target.Element("Question").Value = "Yes";


        

    }



     //set the Visibility of skin to false 
    private void invisibleSkin()
    {
        var skin = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element("Name").Value.ToString().Equals("Skin")).Single();

        skin.Element("Visible").Value = "0";



    }



      //set the Visibility of all the organs to visible by setting its Visible tag to "1" except for skin 
    //first para is the tag name of the target tag,the second is the Specific Value,and the third is the new value that user wants to set as the tags new value.

    public void setValueOfSpecificTagsWithSpecificValue(string tagName,string withSpecificValue, string initValue)
    {

        //var target = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element("Name").Value != OrganName.Text).Single();
       
        //set the Visibility of all the organs to visible by setting its Visible tag to "1"
        foreach (XmlNode node in xmlDoc.SelectNodes(".//"+tagName+"[text()="+withSpecificValue+"]"))
        {
            node.InnerText = initValue;
        }

      
    }

    //convert  XmlDocument object to XElement object to use "where" phrase to locate a specific tag;
    public void convertXmlDoc2XElement()
    {
        //create XElement xDoc to keep the content of xmlDoc for locate specific tag and modify it later.
        xDoc = XElement.Load(new XmlNodeReader(xmlDoc));
        

    }

    //append Question tag to each Organ
    public void appendTag2EachOrgan(string tagName,string initValue)
    {
        XmlNodeList elemList = xmlDoc.GetElementsByTagName("Organ");
        for (int i = 0; i < elemList.Count; i++)
        {
            //Create a new node.
            XmlElement elem = xmlDoc.CreateElement(tagName);
            elem.InnerText = initValue;


            elemList[i].AppendChild(elem);
        }
    }

    //return the content of xmlDoc
    public XmlDocument getXmlDoc()
    {

        return xmlDoc;
    }


    //save the content of XML data in XElement object.
    public void saveXML(string xmlStorePath)
    {  
        //turn skin to invisible
        invisibleSkin();

        xDoc.Save(xmlStorePath);

        /*
        //sync the organ XML file which is just saved to xmlStorePath with the corresponding organ XML file in D:\Mirac3DBuilder\HintsAccounts\Student\Mirac\1161-1450 for 3DBuilder to read.
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = "D:\\IPC\\syncOrganXML.bat";
        proc.StartInfo.WorkingDirectory = "D:\\IPC";
        proc.Start();
         */
    
    
    
    }





}
