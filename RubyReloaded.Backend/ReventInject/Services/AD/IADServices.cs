namespace ReventInject.Services.AD
{

    public interface IADServices
    {
        /// <summary>
        /// Authenticate users using their windows domain username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Login(string userName, string password);

        /// <summary>
        /// Authenticate users using their windows domain username and password and return the display name.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param> 
        /// <param name="display_name"></param>
        /// <returns></returns>
        bool Authenticate(string userName, string password, ref string display_name);

        ADUser Authenticate(string userName, string password);

        ADUser AuthenticateByEmail(string email, string password);

        /// <summary>
        /// This method will attempt to log in a user based on the username and password to ensure that they have been set up 
        /// within the Active Directory. This is the basic UserName, Password check.
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        bool IsUserValid(string UserName);

        /// <summary>
        /// This method will attempt to log in a user based on the username and password to ensure that they have been set up 
        /// within the Active Directory. This is the basic UserName, Password check.
        /// </summary>
        /// <param name="email"></param>
        bool IsUserValidByEmail(string email);

        /// <summary>
        /// Gets the display name as entered in the AD using username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        string getFullName(string username);


        /// <summary>
        /// Gets the display name as entered in the AD using email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        string getFullNameByEmail(string email);
        
    }
    
    public enum UserDomain
    {
        FBNQuestMB = 1,
        FBNQuest = 2,
        Local = 3
    }

}
