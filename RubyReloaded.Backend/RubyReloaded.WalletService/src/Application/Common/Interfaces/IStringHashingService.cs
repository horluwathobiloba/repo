namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IStringHashingService
    {
        public string CreateDESStringHash(string input);
        public object DecodeDESStringHash(string input);
    }
}
