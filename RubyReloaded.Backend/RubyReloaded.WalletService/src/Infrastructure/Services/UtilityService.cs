using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Infrastructure.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IConfiguration _configuration;
        private readonly IAPIClientService _aPIClient;

        public UtilityService(IConfiguration configuration, IAPIClientService aPIClient)
        {
            _configuration = configuration;
            _aPIClient = aPIClient;
        }

      

        public string GenerateTransactionReference()
        {
            Guid reference = Guid.NewGuid();
            return string.Concat("REVENT_",reference.ToString());
        }
    }
}
