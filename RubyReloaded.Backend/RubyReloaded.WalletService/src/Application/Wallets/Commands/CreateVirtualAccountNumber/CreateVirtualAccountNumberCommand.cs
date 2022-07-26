//using MediatR;
//using RubyReloaded.WalletService.Application.Common.Interfaces;
//using RubyReloaded.WalletService.Application.Common.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace RubyReloaded.WalletService.Application.Wallets.Commands.CreateVirtualAccountNumber
//{
//    public class CreateVirtualAccountNumberCommand:IRequest<Result>
//    {
//        public string AccountNae { get; set; }
//    }

//    public class CreateVirtualAccountNumberComandHandler : IRequestHandler<CreateVirtualAccountNumberCommand, Result>
//    {
//        private readonly IAPIClientService _apiClient;
//        public CreateVirtualAccountNumberComandHandler(IAPIClientService aPIClientService)
//        {
//            _apiClient = aPIClientService;
//        }
//        public Task<Result> Handle(CreateVirtualAccountNumberCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var response=
//            }
//            catch (Exception ex)
//            {

//                throw;
//            }
//        }
//    }
//}
