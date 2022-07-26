﻿using System;

namespace DataStructures
{
    public class Queue<T>
    {
        readonly Deque<T> store = new Deque<T>();

        public void Enqueue(T value)
        {
            store.EnqueueTail(value);
        }

        public T Dequeue()
        {
            return store.DequeueHead();
        }

        public T Peek()
        {
            T value;
            if (store.PeekHead(out value))
            {
                return value;
            }

            throw new InvalidOperationException();
        }

        public int Count
        {
            get { return store.Count; }
        }
    }
}
