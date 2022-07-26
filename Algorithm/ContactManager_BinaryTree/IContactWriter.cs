using System.Collections.Generic;
using System.IO;

namespace ContactManager_BinaryTree
{
    internal interface IContactWriter
    {
        void Write(Stream stream, IEnumerable<Contact> contacts);
    }
}
