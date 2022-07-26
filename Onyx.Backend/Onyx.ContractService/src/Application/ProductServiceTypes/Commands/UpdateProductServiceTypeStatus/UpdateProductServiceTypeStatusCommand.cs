using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.UpdateProductServiceTypeStatus
{
    public class UpdateProductServiceTypeStatusCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeVendorStatusCommandHandler : IRequestHandler<UpdateProductServiceTypeStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;

        public ChangeVendorStatusCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateProductServiceTypeStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entity = await _context.ProductServiceTypes.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid product service type!");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Domain.Enums.Status.Inactive;
                        message = "Product service type is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Product service type  was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "Product service type  activation was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure($"ProductServiceType status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }

}
