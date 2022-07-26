using System;
using System.Collections.Generic;

namespace InterviewQSol
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReverseString("My Name is John");
            

        }

        static void ReverseString(string word)
        {
            var convertedToArrayOfChar = word.ToCharArray();
            var result = new List<string>();
            int wordLength = convertedToArrayOfChar.Length;
            for (int i = wordLength-1; i >=0; i--)
            {
                result.Add(convertedToArrayOfChar[i].ToString());
            }
            foreach(var s in result)
            {
                Console.Write(s);
            }
        }
    }
}
