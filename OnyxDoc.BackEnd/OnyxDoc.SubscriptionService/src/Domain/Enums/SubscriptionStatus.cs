namespace OnyxDoc.SubscriptionService.Domain.Enums
{
    public enum SubscriptionStatus
    {
        FreeTrial = 1,
        ProcessingPayment = 2,
        Active = 3,
        Expired = 4,
        Cancelled = 5,
        PendingActivation
    }
}
