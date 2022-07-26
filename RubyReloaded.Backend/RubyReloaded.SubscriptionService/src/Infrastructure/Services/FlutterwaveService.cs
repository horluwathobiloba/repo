using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using RubyReloaded.SubscriptionService.Domain.ViewModels;

namespace RubyReloaded.SubscriptionService.Infrastructure.Services
{
    public class FlutterwaveService : IFlutterwaveService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _client;

        public FlutterwaveService(IConfiguration configuration, IBearerTokenService bearerTokenService, IRestClient client)
        {
            _bearerTokenService = bearerTokenService;
            _configuration = configuration;
            _client = client;
        }

       public async Task<string> CardPayments()
        {
           
            return "";
        }
  
    }
}
