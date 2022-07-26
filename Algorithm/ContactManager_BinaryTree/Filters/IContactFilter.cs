using System;
using System.Collections.Generic;

namespace ContactManager_BinaryTree.Filters
{
    internal interface IContactFilter : IComparable<Contact>
    {
        IEnumerable<Contact> Apply(IEnumerable<Contact> contacts);
    }
}
