using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Currencies.Commands.UpdateCurrencyConfiguration
{
    public class UpdateCurrencyConfigurationCommand:AuthToken,IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrganisationId { get; set; }
        public string UserId { get; set; }
    }
    public class UpdateCurrencyConfigurationHandler : IRequestHandler<UpdateCurrencyConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateCurrencyConfigurationHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateCurrencyConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.CurrencyConfigurations.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid currency type specified.");
                }

                //check if the name of the service types are
                var UpdatedEntityExists = await _context.CurrencyConfigurations.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (UpdatedEntityExists)
                {
                   return Result.Failure($"Another CurrencyConfiguration named '{request.Name}' already exists. Please change the name.");
                }

                entity.Name = request.Name;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.CurrencyConfigurations.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

               // var result = _mapper.Map<VendorTypeDto>(entity);
                return Result.Success("CurrencyConfiguration update was successful!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"CurrencyConfiguration update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }

}
