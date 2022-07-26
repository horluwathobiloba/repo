using System.Collections.Generic;

namespace ContactManager_Array
{
    public interface IContactStore
    {
        Contact Add(Contact contact);
        Contact Remove(Contact contact);

        IEnumerable<Contact> Contacts { get; }
    }
}
