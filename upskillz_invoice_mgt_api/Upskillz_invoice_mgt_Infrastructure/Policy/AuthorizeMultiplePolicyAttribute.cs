using Microsoft.AspNetCore.Mvc;

namespace Upskillz_invoice_mgt_Infrastructure.Policy
{
    public class AuthorizeMultiplePolicyAttribute : TypeFilterAttribute
    {
        public AuthorizeMultiplePolicyAttribute(string[] policies) : base(typeof(AuthorizeMultiplePolicyFilter))
        {
            Arguments = new object[] { policies };
        }
    }
}
