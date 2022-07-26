
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
    public class PaystackProductVm
    {
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public PaystackAccountVm Plan { get; set; }
    }
}
