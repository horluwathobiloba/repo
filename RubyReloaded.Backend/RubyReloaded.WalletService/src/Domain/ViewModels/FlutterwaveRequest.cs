namespace RubyReloaded.WalletService.Domain.ViewModels
{

    public class FlutterwaveRequest
    {
        public string Tx_Ref { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Redirect_Url { get; set; }

        public string Payment_Options { get; set; }

        public Meta Meta { get; set; }

        public Customer Customer { get; set; }

        public Customization Customizations { get; set; }
    }
    public class Customer
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }

    }

    public class Customization
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }

    }
    public class Meta
    {
        public int Consumer_Id { get; set; }
        public string Consumer_Mac { get; set; }

    }
}
