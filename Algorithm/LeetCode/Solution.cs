using System;
using System.Collections.Generic;

namespace LeetCode
{
    public class Solution
    {
        /// <summary>
        /// Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.
        /// You may assume that each input would have exactly one solution, and you may not use the same element twice.
        /// You can return the answer in any order.
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="Target"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int[] TwoSum(int[] nums, int target)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i +1 ; j < nums.Length; j++)
                {
                    if (nums[j] == target - nums[i])
                    {
                        return new int[] { i, j };
                    }
                }
            }

            throw new ArgumentException("No two sum solution");
        }



        /// <summary>
        /// Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.
        /// You may assume that each input would have exactly one solution, and you may not use the same element twice.
        /// You can return the answer in any order.
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="Target"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int[] TwoSum1(int[] nums, int target)
        {
            var dictionary = new Dictionary<int, int>();
            for (var i = 0; i < nums.Length; i++)
            {
                dictionary[nums[i]] = i;
            }

            for (var i = 0; i < nums.Length; i++)
            {
                var complement = target - nums[i];
                if (dictionary.ContainsKey(complement) && dictionary[complement] != i)
                {
                    return new[] { i, dictionary[complement] };
                }
            }

            throw new ApplicationException("No two sum solution");
        }

        /// <summary>
        /// Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.
        /// You may assume that each input would have exactly one solution, and you may not use the same element twice.
        /// You can return the answer in any order.
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private int[] TwoSum_Solution3(int[] nums, int target)
        {
            var dictionary = new Dictionary<int, int>();

            for (var i = 0; i < nums.Length; i++)
            {
                var complement = target - nums[i];

                if (dictionary.ContainsKey(complement))
                {
                    return new[] { dictionary[complement], i };
                }

                dictionary[nums[i]] = i;
            }

            throw new ApplicationException("No two sum solution");
        }
    }
}
