using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.ViewModels
{
    public class InboxVm
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public List<string> Recipients { get; set; }
        public DateTime Time { get; set; }
        public EmailAction EmailAction { get; set; }
        public string DocumentUrl { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string Description { get; set; }
        public string SenderEmail { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
        public int DocumentId { get; set; }

    }
}
