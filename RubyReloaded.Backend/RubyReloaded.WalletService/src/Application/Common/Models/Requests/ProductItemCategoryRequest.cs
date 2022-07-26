using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class ProductItemCategoryRequest
    {
        public string Name { get; set; }
        public string DefaultImageUrl { get; set; }
        public string Description { get; set; }
    }
}
