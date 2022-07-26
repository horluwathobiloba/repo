using System;
using System.Collections;

namespace UpSkillzInject.Utilities
{
    public enum DateSystem
    {
        MS_DOS,
        Mac
    }

    public enum DateCulture
    {
        en_GB = 1,
        en_US = 2
    }

    public enum APIType
    {
        YouTube = 1,
        Others = 2
    }

    #region "AuditTrail/WorkFlow/Approval Enums"

    public enum StatementFormat
    {
        CSV = 1,
        Excel = 2,
        Text = 3,
        PDF = 4,
        XML = 5,
        Word = 6
        // MT940 = 7
    }


    public enum WorkflowAction
    {
        Approve = 1,
        Decline = 2,
        Cancel = 3
    }

    public enum WorkFlowStatus
    {
        Pending = 1,
        Processing = 2,
        Completed = 3,
        Cancelled = 4,
        Declined = 5,
        PartlyCompleted = 6
    }

    public enum ActionType
    {
        Add = 1,
        Update = 2,
        Delete = 3,
        AddUpdate = 4,
        AddDelete = 5,
        UpdateDelete = 6,
        AddUpdateDelete = 7
    }

    public enum AuditAction
    {
        All = 0,
        Save = 1,
        Update = 2,
        Delete = 3
    }

    public enum AuditStatus
    {
        All = 0,
        Successful = 1,
        Failed = 2
    }

    public enum ApprovalStatus
    {
        All = 0,
        Approved = 1,
        Pending = 2,
        Declined = 3,
        Cancelled = 4
    }

    #endregion

    #region "Transaction Upload Enum"

    public enum Delimiter
    {
        Comma = 1,
        Pipe = 2,
        Exclamation = 3
    }

    public enum UploadFileType
    {
        CSV = 1,
        Excel = 2,
        XML = 3,
        API = 4,
        MT103 = 5,
        SWIFT = 6,
        MT940 = 7

    }

    #endregion

    #region "Security Enums"

    /// <summary>
    /// The Encryption Type to use
    /// </summary>
    /// <remarks>Same as the Algorithm Enum in the Crypto class</remarks>
    public enum EncryptionType : int
    {
        Rijndael = 1,
        TripleDES = 2,
        RSA = 3,
        RC2 = 4,
        DES = 5,
        RNG = 6,
        SHA1 = 7,
        MD5 = 9,
        SHA256 = 10,
        SHA384 = 11,
        SHA512 = 12,
        DSA = 13,
        Base64 = 14
    }

    public enum AuthenticationType
    {
        ActiveDirectory = 1,
        Application = 2
    }

    #endregion

    #region User/Role Management Enums

    public enum ConfigCategory
    {
        UserCategory = 1,
        RoleCategory = 2
    }


    public enum LoginStatus
    {
        Active = 1,
        InActive = 2
    }

    public enum FunctionType
    {
        All = 0,
        Add = 1,
        Edit = 2,
        Delete = 3,
        Disable = 4,
        Enable = 5,
        Approve = 6,
        View = 7

    }

    public enum ExportFormatType
    {
        PDF = 1,
        CSV = 2,
        Excel = 3,
        Word = 4
    }

    //public enum Status
    //{
    //    Active = 1,
    //    InActive = 2,
    //    Deleted = 3,
    //    Unverified = 4,
    //    Blocked = 5,
    //    Declined = 6
    //}


    #endregion

    //Class ItemCollection implements IEnumerable interface
    public class ItemCollection : IEnumerable
    {
        private String[] itemId;
        //Constructor to create and populate itemId String array
        public ItemCollection(int noOfItem)
        {
            itemId = new String[noOfItem];
            for (int i = 0; i <= itemId.Length - 1; i++)
            {
                itemId[i] = i.ToString();
            }
        }
        //Implementation of method GetEnumerator of IEnumerable interface
        public virtual IEnumerator GetEnumerator()
        {
            return new ItemIterator(this);
        }
        //Inner class ItemIterator, implements IEnumerator
        public class ItemIterator : IEnumerator
        {
            //Declare a variable of type ItemCollection,
            //to keep reference to enclosing class instance
            private ItemCollection itemCollection;
            //Declare a integer pointer and Set to -1, so that
            //first call to MoveNext moves the enumerator over
            //the first element of the collection.
            private int index = -1;
            //Pass an instance of enclosing class
            public ItemIterator(ItemCollection ic)
            {
                //Save enclosing class reference
                itemCollection = ic;
            }
            //After an enumerator is created or after a Reset,
            //an enumerator is positioned before the first element
            //of the collection, and the first call to MoveNext
            //moves the enumerator over the first element of the
            //collection.
            public bool MoveNext()
            {
                index += 1;
                if (index < itemCollection.itemId.Length)
                {
                    return true;
                }
                else
                {
                    index = -1;
                    return false;
                }
            }
            //Return the current object, in our case Item Id string
            //from itemId[] array. Throws InvalidOperationException exception
            //if index pointing to wrong position
            public object Current
            {
                get
                {
                    if (index <= -1)
                    {
                        throw new InvalidOperationException();
                    }
                    return itemCollection.itemId[index];
                }
            }
            //Reset pointer to -1
            public void Reset()
            {
                index = -1;
            }
        }
    }
}
