using System.Collections.Generic;
using System.IO;

namespace ContactManager_Array
{
    public interface IContactWriter
    {
        void Write(Stream stream, IEnumerable<Contact> contacts);
    }
}
