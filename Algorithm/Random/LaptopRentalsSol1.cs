using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random
{
    public class LaptopRentalsSol1
    {
        public static int LaptopRentals(int[][] times)
        {
            
            if (times.Length == 0)
            {
                return 0;
            }
            var listOfTimes = times.Select(l =>l.ToList()).ToList();
            listOfTimes.Sort((a, b) => a[0].CompareTo(b[0]));
            List<List<int>> timesWhenLaptopIsUsed = new List<List<int>>();
            timesWhenLaptopIsUsed.Add(listOfTimes[0]);
            MinHeap heap = new MinHeap(timesWhenLaptopIsUsed);
            for (int idx = 1; idx < listOfTimes.Count; idx++)
            {
                List<int> currentInterval = listOfTimes[idx];
                if (heap.peek()[1] <= currentInterval[0])
                {
                    heap.remove();
                }
                heap.insert(currentInterval);
            }
            return timesWhenLaptopIsUsed.Count - 1;
        }
    }



    public class MinHeap
    {
        List<List<int>> heap = new List<List<int>>();
        public MinHeap(List<List<int>> array)
        {
            heap = buildHeap(array);
        }
        public List<List<int>> buildHeap(List<List<int>> array)
        {
            int firstParentIdx = (array.Count - 2) / 2;
            for (int currentIdx = firstParentIdx; currentIdx >= 0; currentIdx--)
            {
                siftDown(currentIdx, array.Count - 1, array);
            }
            return array;
        }
        public void siftDown(int currentIdx, int endIdx, List<List<int>> heap)
        {
            int newCurrentIdx = currentIdx;
            int childOneIdx = currentIdx * 2 + 1;
            while (childOneIdx <= endIdx)
            {
                int childTwoIdx =
                (newCurrentIdx * 2 + 2 <= endIdx) ? newCurrentIdx * 2 + 2 : -1;
                int idxToSwap;
                if (childTwoIdx != -1 &&
                heap[childTwoIdx][1] < heap[childOneIdx][1])
                {
                    idxToSwap = childTwoIdx;
                }
                else
                {
                    idxToSwap = childOneIdx;
                }
                if (heap[idxToSwap][1] < heap[currentIdx][1])
                {
                    swap(newCurrentIdx, idxToSwap, heap);
                    newCurrentIdx = idxToSwap;
                    childOneIdx = newCurrentIdx * 2 + 1;
                }
                else
                {
                    return;
                }
            }
        }
        public void siftUp(int currentIdx, List<List<int>> heap)
        {
            int newCurrentIdx = currentIdx;
            int parentIdx = (currentIdx - 1) / 2;
            while (newCurrentIdx > 0 && heap[newCurrentIdx][1] < heap[parentIdx][1])
            {
                swap(newCurrentIdx, parentIdx, heap);
                newCurrentIdx = parentIdx;
                parentIdx = (newCurrentIdx - 1) / 2;
            }
        }
        public List<int> peek()
        {
            return heap[0];
        }
        public List<int> remove()
        {
            swap(0, heap.Count - 1, heap);
            List<int> valueToRemove = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            siftDown(0, heap.Count - 1, heap);
            return valueToRemove;
        }
        public void insert(List<int> value)
        {
            heap.Add(value);
            siftUp(heap.Count - 1, heap);
        }
        public void swap(int i, int j, List<List<int>> heap)
        {
            List<int> temp = heap[j];
            heap[j] = heap[i];
            heap[i] = temp;
        }
    }
}
