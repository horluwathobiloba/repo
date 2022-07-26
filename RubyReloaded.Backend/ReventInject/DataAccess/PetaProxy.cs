using PetaPoco;

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
