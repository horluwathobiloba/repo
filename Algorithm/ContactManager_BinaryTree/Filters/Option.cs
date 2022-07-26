﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager_BinaryTree.Filters
{
    internal class Option<T>
    {
        readonly T value;

        public Option()
        {
            this.HasValue = false;
        }

        public Option(T value)
        {
            this.HasValue = true;
            this.value = value;
        }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException("Attempt to retrieve Option<T> value when not set");
                }

                return value;
            }
        }

        public bool HasValue { get; }

        static public Option<T> Default()
        {
            return new Option<T>();
        }
    }
}
