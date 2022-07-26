using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReventInject.DataAccess
{
    public class PetaProxy
    {
        private static string ConString;
        public PetaProxy(string conStr)
        {
            ConString = conStr;
        }
        
        public Database AmDb = new PetaPoco.Database(ConString);

    }
}
