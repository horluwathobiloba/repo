namespace RubyReloaded.WalletService.Domain.Enums
{
    public enum ResponseCode
    {
        Success= 00,
        DuplicateTransaction = 01,
        RejectedTransaction=02,
        SystemFailure = 03
    }
}
