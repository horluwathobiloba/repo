namespace RubyReloaded.WalletService.Domain.ViewModels
{
    public class EntityVm<T> where T : new()
    {
        T obj;

        public EntityVm()
        {
            obj = new T();
        }

        public bool succeeded { get; set; }
        public T entity { get; set; }
        public string[] messages { get; set; }
        public string message { get; set; }
    }
}