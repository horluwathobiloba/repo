using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoExpert
{
    public class Arrays_Easy
    {
        public static int[] TwoNumberSum(int[] array, int targetSum)
        {
            var listOfNumbers = array.ToList();
            var result = new List<int>();
            foreach (var item in listOfNumbers)
            {
                int checkNum = targetSum - item;
                if (result.Contains(checkNum))
                {
                    return new[] { checkNum, item };
                }
                result.Add(item);

            }

            return new int[0];
        }
        
        public static int[] TwoNumberSum1(int[] array, int targetSum)
        {
            var arrayLength = array.Length;
            for (int i = 0; i < arrayLength - 1; i++)
            {
                int firstNum = array[i];
                for (int j = i + 1; j < arrayLength; j++)
                {
                    int secontNum = array[j];
                    if (firstNum + secontNum == targetSum)
                    {
                        return new int[] { firstNum, secontNum };
                    }
                }
            }
            return new int[0];
        }
        
        public static int[] TwoNumberSum2(int[] array, int targetSum)
        {
            HashSet<int> set = new HashSet<int>();
            foreach (var item in set)
            {
                int checkNum = targetSum - item;
                if (set.Contains(checkNum))
                {
                    return new[] { checkNum, item };
                }
                set.Add(item);
            }
            
            return new int[0];
        }
        
        public static int[] TwoNumberSum3(int[] array, int targetSum)
        {
            Array.Sort(array);
            int left = 0;
            int right = array.Length - 1;
            while (left < right)
            {
                int currentSum = array[left] + array[right];
                if (currentSum == targetSum)
                {
                    return new int[]{ array[left], array[right] };
                }
                else if (currentSum < targetSum)
                {
                    left++;
                }
                else
                {
                    right--;
                }
            }
            return new int[0];
        }
    }
}
