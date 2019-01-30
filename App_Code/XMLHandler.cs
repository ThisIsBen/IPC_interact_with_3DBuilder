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

    //11/9 add correct answer list member  variable
    public StringBuilder correctAnswer = new StringBuilder();
    //11/9 add order of correct answer list member  variable
    public StringBuilder correctAnswerOrder = new StringBuilder();

    public XMLHandler(string XMLSrcPath)
    {
        xmlDoc = new XmlDocument();
        
        //load all the organ of the corresponding part of body.
        xmlDoc.Load(XMLSrcPath);

        //create XElement xDoc to keep the content of xmlDoc for locate specific tag and modify it later.
        xDoc = XElement.Load(new XmlNodeReader(xmlDoc));
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

    /*
    Arg description:
    targetTagName: the target tag name
    organName: the target organ name
    newValue: We will use this new Value to update the value of the target organ 
    */
    //set the checked organs as Questions by setting its Question tag to "Yes"
    public void setATargetTag2ANewValue(string targetTagName, string organName, string newValue)
    {


        var target = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element("Name").Value == organName).Single();
        var test = target.Element(targetTagName).Value;


        target.Element(targetTagName).Value = newValue;

        //if the target name is "Question", we append the  organ Name and its Number 
        //to the StringBuilder as the correct answer for this AI type question.
        if (targetTagName == "Question")
        {
            correctAnswer.Append("," + organName);
            correctAnswerOrder.Append("," + target.Element("Number").Value);
        }
        

        //11/9 Add a function in XMLHandler.cs  to accumulate all the selected organName.Text in a list.
        //add 'OrganName.Text' to   correct answer list 
        
        //add 'target.Element("Number").Value' to order of correct answer list
       
        

    }

    /*Arg description:
     * targetOrganName: the name of the target organ name.
     * targetAttrTag: the target attribute tag of the target organ.
     * targetAttrTagValue: the attribute value that we want to check of the target organ
     * 
     * return true/false whether the specific attribute of a given target organ name is the same as targetAttrTagValue argument.
     * */

    //check a specific attribute of a given target organ name
    public bool checkOrganAttrValue(string targetOrganName, string targetAttrTag, string targetAttrTagValue)
    {
        var targetOrgan = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element("Name").Value.ToString().Equals(targetOrganName)).Single();

        if (targetOrgan.Element(targetAttrTag).Value == targetAttrTagValue)
            return true;
        return false;




    }

    /*
    Arg description:
    tagName: the target tag name.
    withSpecificValue: it is used to locate the target tag whose value is this specific value.
    newValue: We use the new Value to update the value of the target tag.
    */
    
    //set the Visibility of all the organs to visible by setting its Visible tag to "1" except for skin 
    //first para is the tag name of the target tag,the second is the Specific Value,and the third is the new value that user wants to set as the tags new value.

    public void setValueOfSpecificTagsWithSpecificValue(string SpecificTagName, string withSpecificValue, string newValue)
    {

        //var target = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element("Question").Value == "Yes").Single();
        var targets = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element(SpecificTagName).Value == withSpecificValue);

        foreach (var node in targets)
        {
            node.Element(SpecificTagName).Value = newValue;
        }
       
       

      
    }


   //set a given value to a specific tag
    public void setValueOfSpecificTag(string SpecificTagName, string value)
    {

        //var target = xDoc.Element("Organs").Elements("Organ").Where(element => element.Element("Question").Value == "Yes").Single();
        var targets = xDoc.Elements(SpecificTagName);

        foreach (var node in targets)
        {
            node.Value = value;
        }




    }

    //append Question tag to each Organ
    /*
     Arg description:
     tagName: the name of the new tag you want to append 
     initValue: the init value of the new tag you want to append
     appendOneEle_InNestedStructure: append the new tag as an One element to the XML file or append it to a nested structure.
     parentTag: to indicate under which tag you want to append the new tag to
    */
    public void appendTag2EachOrgan(string tagName,string initValue,string appendOneEle_InNestedStructure="OneElememt", string parentTag="")
    {
        

        if (appendOneEle_InNestedStructure == "NestedStructure")
        {
                XmlNodeList elemList = xmlDoc.GetElementsByTagName(parentTag);
                for (int i = 0; i < elemList.Count; i++)
                {
                    //Create a new node.
                    XmlElement elem = xmlDoc.CreateElement(tagName);
                    elem.InnerText = initValue;


                    elemList[i].AppendChild(elem);
                }

            
        }


        else 
        {

            //Create a new node.
            XmlElement elem = xmlDoc.CreateElement(tagName);
            elem.InnerText = initValue;
            xmlDoc.DocumentElement.AppendChild(elem);


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
