using ContactManager_Array.Filters;
using System;
using System.Collections.Generic;

namespace ContactManager_Array
{
    public class ContactStore : IContactStore
    {
        readonly Contact[] contacts = new Contact[100];
        int contactCount = 0;
        int nextId = 1;
        public IEnumerable<Contact> Contacts
        {
            get
            {
                for (int i = 0; i < contactCount; i++)
                {
                    yield return contacts[i];
                }
            }
        }

        public Contact Add(Contact contact)
        {
            if (contact == null)
            {
                Log(LogLevel.Error, "Add: null contact provided (skipping)");
                throw new ArgumentNullException("Contacts cannot be null");
            }

            Contact withId = Contact.CreateWithId(nextId++, contact);

            Log(LogLevel.Info, "Add: adding new contact with ID {0} ({1} {2})", withId.ID, withId.FirstName, withId.LastName);
            Contact added = InsertSorted(withId);

            if (added != null)
            {
                Log(LogLevel.Info, "Add: complete ({0})", added.ID);
            }
            else
            {
                Log(LogLevel.Info, "Add: failed ({0}) - see error log", withId.ID);
            }

            return added;
        }

        public IEnumerable<Contact> Add(IEnumerable<Contact> contacts)
        {
            if (contacts == null)
            {
                throw new ArgumentNullException("contacts");
            }

            foreach (Contact c in contacts)
            {
                Add(c);
            }

            return Contacts;
        }

        public IEnumerable<Contact> Load(IEnumerable<Contact> newContacts)
        {
            for (int i = 0; i < contacts.Length; i++)
            {
                contacts[i] = null;
            }

            nextId = 0;
            contactCount = 0;

            return Add(newContacts);
        }

        private void Swap(int index1, int index2)
        {
            Contact temp = contacts[index1];
            contacts[index1] = contacts[index2];
            contacts[index2] = temp;
        }

        private Contact InsertSorted(Contact contact)
        {
            if (contactCount == contacts.Length)
            {
                Log(LogLevel.Error, "Add: inserting contact failed - contact list full");
                return null;
            }

            // place it at the end
            contacts[contactCount] = contact;

            // walk it forward to it's appropriate spot
            for (int i = contactCount; i > 0; i--)
            {
                if (contacts[i].CompareTo(contacts[i - 1]) <= 0)
                {
                    Swap(i, i - 1);
                }
            }

            contactCount++;
            return contact;
        }

        public Contact Remove(Contact contact)
        {

            // place it at the end
            contacts[contactCount] = contact;

            // walk it forward to it's appropriate spot
            for (int i = contactCount; i > 0; i--)
            {
                if (contacts[i].CompareTo(contacts[i - 1]) <= 0)
                {
                    Swap(i, i - 1);
                }
            }

            contactCount++;
            return contact;
        }

        public IEnumerable<Contact> Search(ContactFieldFilter filter)
        {
            return filter.Apply(this.Contacts);
        }


        protected virtual void Log(LogLevel level, string message)
        {
            OnMessageLogged(new LogMessageEventArgs(level, message));
        }

        protected virtual void Log(LogLevel level, string format, params object[] args)
        {
            OnMessageLogged(new LogMessageEventArgs(level, format, args));
        }

        protected virtual void OnMessageLogged(LogMessageEventArgs args)
        {
            MessageLogged?.Invoke(this, args);
        }

        public event EventHandler<LogMessageEventArgs> MessageLogged;
    }
}
