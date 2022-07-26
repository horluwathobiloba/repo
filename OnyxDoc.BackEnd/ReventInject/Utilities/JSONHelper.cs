using ReventInject.Utilities.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ReventInject.Utilities
{
    public static class JSONHelper
    {
        //public static string ToJson<T>(this T obj)
        //{
        //    var jsonSerialiser = new JavaScriptSerializer();
        //    string json = jsonSerialiser.Serialize(obj);
        //    return json;
        //}

        //public static string ToJson<T>(IEnumerable<T> aList)
        //{
        //    var jsonSerialiser = new JavaScriptSerializer();
        //    string json = jsonSerialiser.Serialize(aList);
        //    return json;
        //}

        //public static T ConvertToObject<T>(string jsonData)
        //{
        //    var jsonSerialiser = new JavaScriptSerializer();
        //    T json = jsonSerialiser.Deserialize<T>(jsonData);
        //    return json;
        //}

        //public static T ToObject<T>(this string jsonData)
        //{
        //    var jsonSerialiser = new JavaScriptSerializer();
        //    T json = jsonSerialiser.Deserialize<T>(jsonData);
        //    return json;
        //}

        //public static async Task<T> DownloadSerializedJSONDataAsync<T>(string url) where T : new()
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var jsonData = string.Empty;
        //        try
        //        {
        //            jsonData = await httpClient.GetStringAsync(url);
        //        }
        //        catch (Exception)
        //        {
        //            return default(T);
        //        }
        //        return !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<T>(jsonData) : default(T);
        //    }
        //}

        //public static async Task<T> DownloadSerializedJSONDataAsync<T>(string url, APIType apiType,  string YouTubeAPIKey = "YouTube_API_KEY") where T : new()
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        //API called is YouTube, add whatever HTTP headers needed
        //        if (apiType == APIType.YouTube)
        //        {
        //            httpClient.DefaultRequestHeaders.Add("youtube-api-key", YouTubeAPIKey); //WebConfigurationManager.AppSettings["YouTube_API_KEY"]);
        //        }
        //        var jsonData = string.Empty;
        //        try
        //        {
        //            jsonData = await httpClient.GetStringAsync(url);
        //        }
        //        catch (Exception)
        //        {
        //            return default(T);
        //        }
        //        return !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<T>(jsonData) : default(T);
        //    }
        //}
    }
}
