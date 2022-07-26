using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager_BinaryTree.Actions
{
    public class Remove : Action
    {
        public Remove(IContactStore manager, Contact contact)
            : base(manager, contact)
        {
        }

        public override IEnumerable<Contact> Execute()
        {
            Contact removed;
            if (manager.Remove(this.contact, out removed))
            {
                return new List<Contact>(1) { removed };
            }

            return new List<Contact>(0);
        }
    }
}
