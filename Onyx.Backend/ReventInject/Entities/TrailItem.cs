using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReventInject.Entities
{

    [DataContract, Serializable()]
    public class TrailItem
    {

        private string _Name;
        private string _ValueBefore;

        private string _ValueAfter;
        public TrailItem(string name, string valueBefore, string valueAfter)
        {
            this.Name = name;
            this.ValueBefore = valueBefore;
            this.ValueAfter = valueAfter;
        }

        public TrailItem()
            : this("", "", "")
        {
        }

        public TrailItem(string name)
            : this(name, "", "")
        {
        }

        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        [DataMember()]
        public string ValueBefore
        {
            get { return _ValueBefore; }
            set { _ValueBefore = value; }
        }

        [DataMember()]
        public string ValueAfter
        {
            get { return _ValueAfter; }
            set { _ValueAfter = value; }
        }

    }

}
