using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IBankService
    {
        Task<List<ProvidusWebCategory>> GetWebCategories();
        Task<List<AirtimeVM>> GetAirtimeCategories();
        Task<List<AirtimeVM>> GetElectricityServiceCategories();
        Task<List<AirtimeVM>> GetDataServicesCategories();
        Task<T> GetAirtimePaymentUIMap<T>(int airtimeCategoryId);
        Task<T> GetDataServicesPayment<T>(int bill);

        Task<T> GetElectricityServicePayment<T>(int bill);
        Task<T> GetBillPaymentCategories<T>();
        Task<T> GetBillPaymentCategoryOptions<T>(int categoryId);
        Task<T> GetBillPaymentFields<T>(int billId);
        Task MakePayment();
    }
}
