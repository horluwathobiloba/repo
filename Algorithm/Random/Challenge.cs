using System;
using System.Collections.Generic;
using System.Linq;

namespace Random
{
    public static class Challenge
    {

        public static int LaptopRentals(int[][] times)
        {
            if (times.Length == 0)
            {
                return 0;
            }

            int[] startTimes = new int[times.Length];
            int[] endTimes = new int[times.Length];
            for (int i = 0; i < times.Length; i++)
            {
                startTimes[i] = times[i][0];
                endTimes[i] = times[i][1];
            }

            int usedLaptops = 0;
            Array.Sort(startTimes);
            Array.Sort(endTimes);
            int startIndex = 0;
            int endIndex = 0;



            while (startIndex < startTimes.Length)
            {
                if (endTimes[endIndex] > startTimes[startIndex])
                {
                    usedLaptops++;
                    startIndex++;
                }
                else if (endTimes[endIndex] <= startTimes[startIndex])
                {
                    endIndex++;
                    startIndex++;
                }
            }




            return usedLaptops;
        }

        /// <summary>
        /// Given an array of whole numbers containing a mix of both even and odd numbers , remove all even numbers and return every other available odd number.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static int[] ReturnOddNumbers(int[] arr)
        {
            //arr.Where(x => x % 2 != 0).ToArray();
            //var oddNumbers = new List<int>();
            var oddNum = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 != 0)
                {
                    oddNum[i] = arr[i];
                }
            }

            return oddNum;
        }

        /// <summary>
        /// write an optimised algorithm that computes basic arithmetic operations. e.g. (1+2*3) = 7
        /// </summary>
        /// <returns></returns>
        //public static int BasicArithmethic(string arr)
        //{
        //    if (!areBracketsBalanced(arr))
        //    {
        //        return 0;
        //    }
        //}

        static bool isMatchingPair(char character1,
                                  char character2)
        {
            if (character1 == '(' && character2 == ')')
                return true;
            else if (character1 == '{' && character2 == '}')
                return true;
            else if (character1 == '[' && character2 == ']')
                return true;
            else
                return false;
        }

        
        private static bool areBracketsBalanced(char[] exp)
        {
            Stack<char> st = new Stack<char>();

            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '{' || exp[i] == '('
                    || exp[i] == '[')
                    st.Push(exp[i]);
                if (exp[i] == '}' || exp[i] == ')'
                    || exp[i] == ']')
                {

                    if (st.Count == 0)
                    {
                        return false;
                    }

                    else if (!isMatchingPair(st.Pop(),exp[i]))
                    {
                        return false;
                    }
                }
            }

            if (st.Count == 0)
            {
                return true;
            }
            else
            {
                
                return false;
            }
        }
    }
}
