namespace OnyxDoc.SubscriptionService.Domain.Enums
{
    public enum PaymentStatus
    {
        Initiated = 1,
        Processing = 2,
        Success = 3,
        Failed = 4,
        Reversed= 5,
        Cancelled = 6,
        RequiresAction =7,
        RequiresCapture = 8,
        RequiresConfirmation = 9,
        RequiresPaymentMethod = 10
    }
}
