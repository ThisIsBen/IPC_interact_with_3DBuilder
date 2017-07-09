using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// RandomQuestionNo 的摘要描述
/// </summary>
public class RandomQuestionNo
{
    //public int[] Question;

    //public int id;

    //可用
    static public int[] rand(string ID_Num, int[] Question)
    {
        int id = int.Parse(ID_Num);
        int n = Question.Length;
        Random rnd = new Random();



        //  宣告用來儲存亂數的陣列
        int[] ValueString = new int[n];
        //  用來裝產生結果的array
        int[] randQuestionNo = new int[n];

        //  亂數產生
        for (int i = 0; i < n; i++)
        {
            ValueString[i] = rnd.Next(1, id + 1);
            ValueString[i] = ValueString[i] % n + 1;
            //  檢查是否存在重複
            while (Array.IndexOf(ValueString, ValueString[i], 0, i) > -1)
            {
                ValueString[i] = rnd.Next(1, id + 1);
                ValueString[i] = ValueString[i] % n + 1;
            }


            //排版
            Console.Write("\t" + ValueString[i]);
            if ((i + 1) % 5 == 0)
            {
                Console.Write("\n");
            }

        }

        for (int i = 0; i < randQuestionNo.Length; i++)
        {

            randQuestionNo[i] = Question[ValueString[i] - 1];



        }

        return randQuestionNo;
    }



    //不可用
    static public void rand_set_first_three(int[] threenum, out char[] arr)//arr is the return result
    {
        char[] array1 = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n' };
        arr = new char[array1.Length];
        int i = 0;
        Random rnd2 = new Random();
        int newLength = threenum.Length + (arr.Length - threenum.Length);
        int[] result = new int[newLength];

        for (int j = 0; j < threenum.Length; j++)
        {
            result[j] = threenum[j];
        }
        //result[newLength - 1] = newValue;
        for (int k = 0; k < newLength; k++)
        {

            while ((Array.IndexOf(result, result[k], 0, k) > -1) || result[k] == 0)
            {
                result[k] = rnd2.Next(1, newLength + 1);
            }
        }
        foreach (int element in result)
        {
            arr[i] = array1[element - 1];
            i++;
        }

    }
}