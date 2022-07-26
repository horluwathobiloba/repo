using System.Collections.Generic;

namespace ContactManager_Array.Actions
{
    public class Add : Action
    {
        public Add(IContactStore manager, Contact contact) : base(manager, contact)
        {
        }

        public override IEnumerable<Contact> Execute()
        {
            return new Contact[1] { manager.Add(contact) };
        }
    }
}
