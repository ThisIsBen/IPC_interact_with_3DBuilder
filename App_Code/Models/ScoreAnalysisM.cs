using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// ScoreAnalysisM 的摘要描述
/// </summary>
public class ScoreAnalysisM
{
    //StuCouHWDe_ID is still used for displaying the result of the Hints program type question.
    //So, although we don't use it for AITypeQuestion, we still need to keep it in the class 'ScoreAnalysisM'.
    public string StuCouHWDe_ID;

    public string cUserID;
    public List<string[]> Grade = new List<string[]>();
    public int QuestionNum;
    public int[] MemberQuestionNum;
    public int QueIndex_start = 1;
    public bool XMLerror = false;

    //student's total score
    public int studentTotalScore = 0;


    public string  questionTotalScore;

    //student's answer string of an AITypeQuestion(it corresponds to a specific cQID)
    public string[] studentAnswerString;


    //student’s organ number string of an AITypeQuestion(it corresponds to a specific cQID)
    public string[] questionOrderingString;

    //store student's answer of each question organ.
    public List<string[]> studentAnswer = new List<string[]>();

    

    public ScoreAnalysisM(string id, string GradeStr)
    {
        //StuCouHWDe_ID is still used for displaying the result of the Hints program type question.
        //So, although we don't use it for AITypeQuestion, we still need to keep it in the class 'ScoreAnalysisM'.
        StuCouHWDe_ID = id;

        cUserID = id;
        string[] QuestionGroup = GradeStr.Remove(GradeStr.Length - 1).Split(':');
        QuestionNum = QuestionGroup.Length;

        foreach (string temp_str in QuestionGroup)
        {
            string[] str_add;
            string[] temp_str_arr = temp_str.Split(',');

            str_add = new string[Convert.ToInt16(temp_str_arr[2]) + 1];
            str_add[0] = temp_str_arr[1];
            for (int i = 3; i < temp_str_arr.Length; i++)
                str_add[i - 2] = temp_str_arr[i];
            Grade.Add(str_add);
        }
        MemberQuestionNum = new int[QuestionNum];
        for (int i = 0; i < QuestionNum; i++)
            MemberQuestionNum[i] = Grade[i].Length - 1;
    }

    //for marking "Number Answering Mode" AITypeQuestion
    public ScoreAnalysisM(string id, string AnswerStr, string QuesOdrStr, List<string> xmlFile,  string cQID, int questionTotalScore, string cActivityID)
    {
        //StuCouHWDe_ID is still used for displaying the result of the Hints program type question.
        //So, although we don't use it for AITypeQuestion, we still need to keep it.
        StuCouHWDe_ID = id;

        cUserID = id;
        string[] AnswerStr_Question = AnswerStr.Remove(AnswerStr.Length - 1).Split(':');
        string[] QuesOdrStr_Question = QuesOdrStr.Remove(QuesOdrStr.Length - 1).Split(':');
        MemberQuestionNum = new int[AnswerStr_Question.Length];
        QuestionNum = AnswerStr_Question.Length;
        for (int i = 0; i < AnswerStr_Question.Length; i++)
        {

            string[] AnswerStr_MemQuestion = AnswerStr_Question[i].Split(',');
            string[] QuesOdrStr_MemQuestion = QuesOdrStr_Question[i].Split(',');
            if (xmlFile[i] != AnswerStr_MemQuestion[0])
                XMLerror = true;
            MemberQuestionNum[i] = QuesOdrStr_MemQuestion.Length - QueIndex_start;
            Grade.Add(new string[QuesOdrStr_MemQuestion.Length]);
            int grade_perQuestion = questionTotalScore / (QuesOdrStr_MemQuestion.Length - 1);
            int correct_num = 0;
            for (int index = 1; index < QuesOdrStr_MemQuestion.Length; index++)
            {
               
                string studentAnswer = AnswerStr_MemQuestion[index];
                string correctAnswer = QuesOdrStr_MemQuestion[index].ToString();

                if (studentAnswer != correctAnswer)
                {
                    Grade[i][index] = "0";

                }
                else
                {
                    Grade[i][index] = grade_perQuestion.ToString();
                }
                correct_num++;
            }
            Grade[i][0] = (correct_num * grade_perQuestion).ToString();
        }


        ////In  博宇's implementation
        /*
        CsDBOp.UpdateScore("StuCouHWDe_IPC", StuCouHWDe_ID, SQLGradeStr_Upgrade(Grade));
         * */
        CsDBOp.UpdateScore("AITypeQuestionStudentAnswer", cUserID, SQLGradeStr_Upgrade(Grade), cQID, cActivityID);

    }

    //for marking "Name Answering Mode" AITypeQuestion
    public ScoreAnalysisM(string id, string AnswerStr, string QuesOdrStr, List<string> xmlFile, Hashtable[] correctAnswerHT, string cQID, int questionTotalScore, string cActivityID)
    {


        //StuCouHWDe_ID is still used for displaying the result of the Hints program type question.
        //So, although we don't use it for AITypeQuestion, we still need to keep it.
        StuCouHWDe_ID = id;

        cUserID = id;
        string[] AnswerStr_Question = AnswerStr.Remove(AnswerStr.Length - 1).Split(':');
        string[] QuesOdrStr_Question = QuesOdrStr.Remove(QuesOdrStr.Length - 1).Split(':');
        MemberQuestionNum = new int[AnswerStr_Question.Length];
        QuestionNum = AnswerStr_Question.Length;
        for (int i = 0; i < AnswerStr_Question.Length; i++)
        {

            string[] AnswerStr_MemQuestion = AnswerStr_Question[i].Split(',');
            string[] QuesOdrStr_MemQuestion = QuesOdrStr_Question[i].Split(',');
            if (xmlFile[i] != AnswerStr_MemQuestion[0])
                XMLerror = true;
            MemberQuestionNum[i] = QuesOdrStr_MemQuestion.Length - QueIndex_start;
            Grade.Add(new string[QuesOdrStr_MemQuestion.Length]);
            int grade_perQuestion = questionTotalScore / (QuesOdrStr_MemQuestion.Length - 1);
            int correct_num = 0;
            for (int index = 1; index < QuesOdrStr_MemQuestion.Length; index++)
            {
                int studentScore;
                string studentAnswer = AnswerStr_MemQuestion[index];
                string correctAnswer = correctAnswerHT[i][QuesOdrStr_MemQuestion[index]].ToString();
                studentScore = grade_perQuestion - CalculateDeductionPoint(correctAnswer.Length, CalculateLevenshteinDistance(correctAnswer, studentAnswer));
                if (studentScore <= 0)
                {
                    Grade[i][index] = "0";
                 
                }
                else
                {
                    Grade[i][index] = studentScore.ToString();
                }
                correct_num++;
            }
            Grade[i][0] = (correct_num * grade_perQuestion).ToString();
        }


        ////In  博宇's implementation
        /*
        CsDBOp.UpdateScore("StuCouHWDe_IPC", StuCouHWDe_ID, SQLGradeStr_Upgrade(Grade));
         * */
        CsDBOp.UpdateScore("AITypeQuestionStudentAnswer", cUserID, SQLGradeStr_Upgrade(Grade), cQID,cActivityID);

    }

    // This function calculates the deduction point by determining number of length of two given strings.
    private int CalculateDeductionPoint(int correctAnswerCharacterLength, int incorrectAnswerCharacterLength)
    {
        if (correctAnswerCharacterLength <= 15)
        {
            return incorrectAnswerCharacterLength * 3;
        }

        if (correctAnswerCharacterLength <= 20)
        {
            return incorrectAnswerCharacterLength * 2;
        }

        return incorrectAnswerCharacterLength * 1;
    }

    // This function makes use of Levenshtein distance algorithm.
    // It returns the different number of length between two given strings.
    // e.g. CalculateLevenshteinDistance(strong, stong) => returns 1.
    private int CalculateLevenshteinDistance(string a, string b)
    {
        if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
        {
            return 0;
        }

        if (string.IsNullOrEmpty(a))
        {
            return b.Length;
        }

        if (string.IsNullOrEmpty(b))
        {
            return a.Length;
        }

        var lengthA = a.Length;
        var lengthB = b.Length;
        var distances = new int[lengthA + 1, lengthB + 1];

        for (var i = 0; i <= lengthA; distances[i, 0] = i++) ;
        for (var j = 0; j <= lengthB; distances[0, j] = j++) ;

        for (var i = 1; i <= lengthA; i++)
            for (var j = 1; j <= lengthB; j++)
            {
                var cost = b[j - 1] == a[i - 1] ? 0 : 1;
                distances[i, j] = Math.Min
                (
                  Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                  distances[i - 1, j - 1] + cost
                );
            }

        return distances[lengthA, lengthB];
    }


    string SQLGradeStr_Upgrade(List<string[]> grade)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < grade.Count; i++)
        {
            sb.Append((i + 1).ToString());
            sb.Append("," + grade[i][0]);
            sb.Append("," + (grade[i].Length - 1).ToString());
            for (int index = 0; index < grade[i].Length - 1; index++)
                sb.Append("," + grade[i][index + 1]);
            sb.Append(':');
        }
        return sb.ToString();
    }
}