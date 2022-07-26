namespace OnyxDoc.DocumentService.Domain.Enums
{
    public enum DocumentStatus
    {
        Draft = 1,
        Sent = 2,
        WaitingForOthers = 3,
        Completed = 4,
        Rejected = 5,
        //Processing = 1,
        //PendingSignatories = 2,
        //Active = 3,
        //Rejected = 4,
        Expired = 6
        //Cancelled = 6,
        //Terminated = 7,
        //PendingActivation = 8
    }
}
