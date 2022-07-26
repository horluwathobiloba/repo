using System.Collections.Generic;
using System.IO;

namespace ContactManager_Array
{
    internal interface IContactReader
    {
        IEnumerable<Contact> Read(Stream stream);
    }
}
