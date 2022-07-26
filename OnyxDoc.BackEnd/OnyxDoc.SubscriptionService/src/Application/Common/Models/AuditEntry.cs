using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int SubscriberId { get; set; }
        public string CreatedByEmail { get; set; }
        public string ControllerName { get; set; }
        public string MicroserviceName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditAction AuditAction { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public AuditTrail ToAudit()
        {
            var audit = new AuditTrail();
            audit.UserId = UserId;
            audit.RoleId = RoleId;
            audit.RoleName = RoleName;
            audit.AuditAction = AuditAction;
            audit.AuditActionDesc = AuditAction.ToString();
            audit.ControllerName = ControllerName;
            audit.CreatedDate = DateTime.Now;
            audit.CreatedByEmail = CreatedByEmail;
            audit.Status = Status.Active;
            audit.StatusDesc = Status.Active.ToString();
            audit.SubscriberId = SubscriberId;
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            audit.MicroserviceName = "subscription";
            return audit;
        }
    }
}
