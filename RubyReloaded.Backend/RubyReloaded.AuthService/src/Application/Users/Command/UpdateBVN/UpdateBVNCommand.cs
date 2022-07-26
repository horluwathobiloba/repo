using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.UpdateBVN
{
    public class UpdateBVNCommand:IRequest<Result>
    {
        public string BVN { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateBVNCommandHandler : IRequestHandler<UpdateBVNCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAPIClient _aPIClient;
        public readonly IStringHashingService _stringHashingService;

        public UpdateBVNCommandHandler(IApplicationDbContext context,
            IIdentityService identityService, 
            IConfiguration configuration, 
            IAPIClient aPIClient, 
            IStringHashingService stringHashingService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
            _identityService = identityService;
            _configuration = configuration;
            _stringHashingService = stringHashingService;
            _aPIClient = aPIClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result> Handle(UpdateBVNCommand request, CancellationToken cancellationToken)
        {
            var bvn = _stringHashingService.CreateMD5StringHash(request.BVN);
            var result = await _identityService.UpdateBVN(request.UserId, bvn);
            var user = await _identityService.GetUserById(request.UserId);
            if (result.Succeeded)
            {
                // var apiUrl = _configuration["WalletService:CreateWallet"];
                //var requestBody = new
                //{
                //    productCategory = 1,
                //    openingBalance = 10,
                //    closingingBalance = 0,
                //    balance = 0,
                //    userId = request.UserId,
                //    email = user.user.Email,
                //    phoneNumber = user.user.PhoneNumber,
                //    accountName = user.user.FirstName
                //};
                var apiUrl = _configuration["WalletService:CreateCustomerAccount"];
                var requestBody = new
                {
                    name = user.user.FirstName + " " + user.user.LastName,
                    customerId=user.user.UserId,
                    currencyCode="NGN",
                };
                //pass the token
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Remove(0,7);
                var response =await _aPIClient.PostAPIUrl(apiUrl, token, requestBody,false);
                if (string.IsNullOrEmpty(response))
                {
                   return Result.Failure("Wallet Creation failed");
                }
                return Result.Success(result.Entity);


            }
            return Result.Failure(result.Message);
        }
    }
}
