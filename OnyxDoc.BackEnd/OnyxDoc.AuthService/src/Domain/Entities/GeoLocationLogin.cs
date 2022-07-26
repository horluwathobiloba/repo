using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class GeoLocationLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SubscriberCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CurrentLocationAddress { get; set; }
    }
}
