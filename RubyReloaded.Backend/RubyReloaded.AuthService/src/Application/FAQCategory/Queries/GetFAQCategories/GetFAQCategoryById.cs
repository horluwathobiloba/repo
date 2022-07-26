using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.FAQCategory.Queries.GetFAQCategories
{
    public class GetFAQCategoryById : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetFAQCategoryByIdHandler : IRequestHandler<GetFAQCategoryById, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetFAQCategoryByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetFAQCategoryById request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.FAQCategories.FirstOrDefaultAsync(a => a.Id == request.Id && a.Status == Status.Active);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.FAQCategory), request.Id);
                }

                //if (entity.Status == Status.Deactivated)
                //{
                //    throw new NotFoundException($"PaymentChannel id={request.Id} does not exist. Please contact support.");
                //}
                // var paymentChannel = _mapper.Map<PaymentChannelDto>(entity);
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }

        }
    }
}
