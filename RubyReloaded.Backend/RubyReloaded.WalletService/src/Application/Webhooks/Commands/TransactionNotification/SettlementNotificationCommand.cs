using AutoMapper.Configuration;
using MediatR;
using Newtonsoft.Json;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using RubyReloaded.WalletService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Webhooks.Commands.SettlementNotificationCommand
{
   

    public class SettlementNotificationCommand : IRequest<ProvidusWebhookResponse>
    {
        public string JsonResponse { get; set; }
    }

    public class SettlementNotificationCommandHandler : IRequestHandler<SettlementNotificationCommand, ProvidusWebhookResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
       

        public SettlementNotificationCommandHandler(IApplicationDbContext context, IAPIClientService aPIClient)
        {
            _context = context;
            _aPIClient = aPIClient;
           
        }
        public async Task<ProvidusWebhookResponse> Handle(SettlementNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var settlementNotification = JsonConvert.DeserializeObject(request.JsonResponse);
                if (settlementNotification == null)
                {
                    throw new Exception("Invalid Settlement Notification");
                }
                else
                {
                    ProvidusWebhookResponse response = new ProvidusWebhookResponse { };
                    return response;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}



