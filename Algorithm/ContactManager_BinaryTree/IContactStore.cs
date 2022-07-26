using System.Collections.Generic;

namespace ContactManager_BinaryTree
{
    public interface IContactStore
    {
        Contact Add(Contact contact);
        bool Remove(Contact contact, out Contact removed);

        IEnumerable<Contact> Contacts { get; }
    }
}
