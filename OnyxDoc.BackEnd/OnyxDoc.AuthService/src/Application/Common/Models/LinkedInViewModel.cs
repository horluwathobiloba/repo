using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Common.Models
{
    public class LinkedInViewModel
    {
        public string access_token { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
        public string expires_in { get; set; }
    }

    public class Handle
    {
        public string emailAddress { get; set; }
    }

    public class RootElement
    {
        [JsonProperty("handle~")]
        public Handle Handle { get; set; }
        public string handle { get; set; }
    }

    public class EmailRoot
    {
        public List<RootElement> elements { get; set; }
    }

    public class Localized
    {
        public string en_US { get; set; }
    }

    public class PreferredLocale
    {
        public string country { get; set; }
        public string language { get; set; }
    }

    public class FirstName
    {
        public Localized localized { get; set; }
        public PreferredLocale preferredLocale { get; set; }
    }

    public class LastName
    {
        public Localized localized { get; set; }
        public PreferredLocale preferredLocale { get; set; }
    }

    public class Paging
    {
        public int count { get; set; }
        public int start { get; set; }
        public List<object> links { get; set; }
    }

    public class RawCodecSpec
    {
        public string name { get; set; }
        public string type { get; set; }
    }

    public class DisplaySize
    {
        public double width { get; set; }
        public string uom { get; set; }
        public double height { get; set; }
    }

    public class StorageSize
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public class StorageAspectRatio
    {
        public double widthAspect { get; set; }
        public double heightAspect { get; set; }
        public string formatted { get; set; }
    }

    public class DisplayAspectRatio
    {
        public double widthAspect { get; set; }
        public double heightAspect { get; set; }
        public string formatted { get; set; }
    }

    public class ComLinkedinDigitalmediaMediaartifactStillImage
    {
        public string mediaType { get; set; }
        public RawCodecSpec rawCodecSpec { get; set; }
        public DisplaySize displaySize { get; set; }
        public StorageSize storageSize { get; set; }
        public StorageAspectRatio storageAspectRatio { get; set; }
        public DisplayAspectRatio displayAspectRatio { get; set; }
    }

    public class Data
    {
        [JsonProperty("com.linkedin.digitalmedia.mediaartifact.StillImage")]
        public ComLinkedinDigitalmediaMediaartifactStillImage ComLinkedinDigitalmediaMediaartifactStillImage { get; set; }
    }

    public class Identifier
    {
        public string identifier { get; set; }
        public int index { get; set; }
        public string mediaType { get; set; }
        public string file { get; set; }
        public string identifierType { get; set; }
        public int identifierExpiresInSeconds { get; set; }
    }

    public class Element
    {
        public string artifact { get; set; }
        public string authorizationMethod { get; set; }
        public Data data { get; set; }
        public List<Identifier> identifiers { get; set; }
    }

    public class DisplayImage
    {
        public Paging paging { get; set; }
        public List<Element> elements { get; set; }
    }

    public class ProfilePicture
    {
        public string displayImage { get; set; }

        [JsonProperty("displayImage~")]
        public DisplayImage DisplayImage { get; set; }
    }

    public class Root
    {
        public FirstName firstName { get; set; }
        public LastName lastName { get; set; }
        public ProfilePicture profilePicture { get; set; }
        public string id { get; set; }
    }

}

