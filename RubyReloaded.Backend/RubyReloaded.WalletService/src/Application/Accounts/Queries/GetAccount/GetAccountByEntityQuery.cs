﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Accounts.Queries.GetAccount
{
    public class GetAccountByEntityQuery : IRequest<Result>
    {
        public string SearchValue { get; set; }
    }
    public class GetAccountByEntityQueryHandler : IRequestHandler<GetAccountByEntityQuery, Result>
    {

        private readonly IApplicationDbContext _context;
        public GetAccountByEntityQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAccountByEntityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SearchValue.Contains('@'))
                {
                    var entity = await _context.Accounts.Include(x=>x.Product).FirstOrDefaultAsync(x => x.Email == request.SearchValue && x.Product.ProductCategory == Domain.Enums.ProductCategory.Cash);
                    if (entity == null)
                    {
                        return Result.Failure("Cannot resolve user accounts");
                    }
                    return Result.Success(entity);
                }
                if (request.SearchValue.Contains('+'))
                {

                    var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.PhoneNumber == request.SearchValue && x.Product.ProductCategory == Domain.Enums.ProductCategory.Cash);
                    if (entity == null)
                    {
                        return Result.Failure("Cannot resolve user accounts");
                    }
                    return Result.Success(entity);
                }
                //int val;
                if (long.TryParse(request.SearchValue, out long val))
                {
                    var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == request.SearchValue && x.Product.ProductCategory == Domain.Enums.ProductCategory.Cash);
                    if (entity == null)
                    {
                        return Result.Failure("Cannot resolve user accounts");
                    }
                    return Result.Success(entity);
                }
                else
                {
                    var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == request.SearchValue && x.Product.ProductCategory == Domain.Enums.ProductCategory.Cash);
                    if (entity == null)
                    {
                        return Result.Failure("Cannot resolve user accounts");
                    }
                    return Result.Success(entity);
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
