using Newtonsoft.Json;
using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
    public class EntityVm<T> where T : new()
    {
        T obj;

        public EntityVm()
        {
            obj = new T();
        }

        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
        [JsonProperty("entity")]
        public T Entity { get; set; }

        [JsonProperty("messages")]
        public string[] Messages { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}