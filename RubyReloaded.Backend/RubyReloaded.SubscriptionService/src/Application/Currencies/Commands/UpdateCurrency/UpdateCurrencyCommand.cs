using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.Currencies.Queries;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.Currencies.Commands
{
    public class UpdateCurrencyCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public bool IsDefault { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateCurrencyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.Currencies.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid currenncy specified.");
                }

                entity.Name = request.CurrencyCode.ToString();        
                entity.CurrencyCode = request.CurrencyCode;
                entity.CurrencyCodeDesc = request.CurrencyCode.ToString();
                entity.IsDefault = request.IsDefault;
                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.Currencies.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<CurrencyDto>(entity);
                return Result.Success("Currency update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Currency update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
