using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetWalletByEntity:IRequest<Result>
    {
        public string SearchValue { get; set; }
    }
    public class GetWalletByEntityHandler : IRequestHandler<GetWalletByEntity, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletByEntityHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletByEntity request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SearchValue.Contains('@'))
                {
                    var entity = await _context.Wallets.FirstOrDefaultAsync(x => x.Email == request.SearchValue&&x.ProductCategory==Domain.Enums.ProductCategory.Cash);
                    if (entity == null)
                    {
                        return Result.Failure("Cannot resolve user wallet");
                    }
                    return Result.Success(entity);
                }
                if (request.SearchValue.Contains('+'))
                {

                    var entity = await _context.Wallets.FirstOrDefaultAsync(x => x.PhoneNumber == request.SearchValue && x.ProductCategory == Domain.Enums.ProductCategory.Cash);
                    if (entity==null)
                    {
                        return Result.Failure("Cannot resolve user wallet");
                    }
                    return Result.Success(entity);
                }
                //int val;
                if (long.TryParse(request.SearchValue,out long val))
                {
                    var entity = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletAccountNumber == request.SearchValue && x.ProductCategory == Domain.Enums.ProductCategory.Cash);
                    if (entity == null)
                    {
                        return Result.Failure("Cannot resolve user wallet");
                    }
                    return Result.Success(entity);
                }
                else
                {
                    var entity = await _context.Wallets.FirstOrDefaultAsync(x => x.UserName == request.SearchValue && x.ProductCategory == Domain.Enums.ProductCategory.Cash);
                    if (entity == null)
                    {
                        return Result.Failure("Cannot resolve user wallet");
                    }
                    return Result.Success(entity);
                }
                //return Result.Failure("Cannot resolve user wallet");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
