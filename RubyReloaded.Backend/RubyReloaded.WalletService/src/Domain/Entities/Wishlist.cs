using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class Wishlist : AuditableEntity
    {

        public int ProductId { get; set; }
        public WishCategory WishCategory { get; set; }
        public ContributionFrequency ContributionFrequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InterestRate { get; set; }
        public int AccountId { get; set; }
        public WishListStatus WishListStatus { get; set; }
        public string WishListStatusDesc { get; set; }
        public decimal TargetAmount { get; set; }
        public string ImageUrl { get; set; }
        public decimal WishlistBalance { get; set; }
        public string WishlistSavingsPeriod { get; set; }
        public decimal WishlistExtensionAmount { get; set; }
        public DateTime WishlistExtensionDate { get; set; }
        public WishlistFundingSource WishlistFundingSource { get; set; }
        public ContributionFrequency WishlistExtensionFrequency { get; set; }
        public int DaysLeft { get; set; }
        public decimal RecurringContribution { get; set; }

    }
}
