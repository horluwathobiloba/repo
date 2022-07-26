using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager_Array.Filters
{
    public class ContactFieldFilter : IContactFilter
    {

        internal Option<int> ID = Option<int>.Default();
        internal Option<string> FirstName = Option<string>.Default();
        internal Option<string> LastName = Option<string>.Default();
        internal Option<string> StreetAddress = Option<string>.Default();
        internal Option<string> City = Option<string>.Default();
        internal Option<string> State = Option<string>.Default();
        internal Option<string> PostalCode = Option<string>.Default();

        public ContactFieldFilter()
        {
        }

        public IEnumerable<Contact> Apply(IEnumerable<Contact> contacts)
        {
            return contacts.Where(c => this.Equals(c)).ToArray();
        }

        public void SetID(int id)
        {
            ID = new Option<int>(id);
        }

        public void SetFirstName(string firstName)
        {
            FirstName = new Option<string>(firstName);
        }

        public void SetLastName(string lastName)
        {
            LastName = new Option<string>(lastName);
        }

        public void SetStreetAddress(string streetAddress)
        {
            StreetAddress = new Option<string>(streetAddress);
        }

        public void SetCity(string city)
        {
            City = new Option<string>(city);
        }

        public void SetState(string state)
        {
            State = new Option<string>(state);
        }

        public void SetPostalCode(string postalCode)
        {
            PostalCode = new Option<string>(postalCode);
        }

        public int CompareTo(Contact other)
        {
            if (ID.HasValue)
            {
                if (other.ID.HasValue)
                {
                    return ID.Value.CompareTo(other.ID.Value);
                }

                return 1;
            }

            if (FirstName.HasValue)
            {
                if (!string.Equals(FirstName.Value, other.FirstName, StringComparison.OrdinalIgnoreCase))
                    return FirstName.Value.CompareTo(other.FirstName);
            }

            if (LastName.HasValue)
            {
                if (!string.Equals(LastName.Value, other.LastName, StringComparison.OrdinalIgnoreCase))
                    return LastName.Value.CompareTo(other.LastName);
            }

            if (StreetAddress.HasValue)
            {
                if (!string.Equals(StreetAddress.Value, other.StreetAddress, StringComparison.OrdinalIgnoreCase))
                    return StreetAddress.Value.CompareTo(other.StreetAddress);
            }

            if (City.HasValue)
            {
                if (!string.Equals(City.Value, other.City, StringComparison.OrdinalIgnoreCase))
                    return City.Value.CompareTo(other.City);
            }

            if (State.HasValue)
            {
                if (!string.Equals(State.Value, other.State, StringComparison.OrdinalIgnoreCase))
                    return State.Value.CompareTo(other.State);
            }

            if (PostalCode.HasValue)
            {
                if (!string.Equals(PostalCode.Value, other.PostalCode, StringComparison.OrdinalIgnoreCase))
                    return PostalCode.Value.CompareTo(other.PostalCode);
            }

            return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Contact))
                return false;

            Contact other = (Contact)obj;

            return CompareTo(other) == 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (ID.HasValue) sb.AppendFormat("{0}ID={1}", (sb.Length > 0) ? " && " : string.Empty, ID.Value);
            if (FirstName.HasValue) sb.AppendFormat("{0}FirstName={1}", (sb.Length > 0) ? " && " : string.Empty, FirstName.Value);
            if (LastName.HasValue) sb.AppendFormat("{0}LastName={1}", (sb.Length > 0) ? " && " : string.Empty, LastName.Value);
            if (StreetAddress.HasValue) sb.AppendFormat("{0}StreetAddress={1}", (sb.Length > 0) ? " && " : string.Empty, StreetAddress.Value);
            if (City.HasValue) sb.AppendFormat("{0}City={1}", (sb.Length > 0) ? " && " : string.Empty, City.Value);
            if (State.HasValue) sb.AppendFormat("{0}State={1}", (sb.Length > 0) ? " && " : string.Empty, State.Value);
            if (PostalCode.HasValue) sb.AppendFormat("{0}PostalCode={1}", (sb.Length > 0) ? " && " : string.Empty, PostalCode.Value);

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public bool HasFilter
        {
            get
            {
                return (ID.HasValue || FirstName.HasValue || LastName.HasValue || StreetAddress.HasValue || City.HasValue || State.HasValue || PostalCode.HasValue);
            }
        }
    }
}
