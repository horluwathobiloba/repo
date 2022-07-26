using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
    public class TokenDto
    {
        public int ExpiresIn { get; set; }
        public string AccessToken { get; set; }
    }
}
