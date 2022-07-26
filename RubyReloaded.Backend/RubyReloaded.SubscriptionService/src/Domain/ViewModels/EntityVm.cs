using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Domain.ViewModels
{
    public class EntityVm<T> where T : new()
    {
        T obj;

        public EntityVm()
        {
            obj = new T();
        }

        public bool Succeeded { get; set; }
        public T Entity { get; set; }
        public string[] Messages { get; set; }
        public string Message { get; set; }
    }
}