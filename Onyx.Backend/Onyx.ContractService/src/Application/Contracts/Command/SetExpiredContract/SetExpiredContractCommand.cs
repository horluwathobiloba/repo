using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Command.SetExpiredContract
{
    public class SetExpiredContractCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int OrganisationId { get; set; }
    }

    public class SetExpiredContractCommandHandler : IRequestHandler<SetExpiredContractCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;

        public SetExpiredContractCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<Result> Handle(SetExpiredContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var verifyOrganization = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                if (user == null) return Result.Failure("User not found");

                var contract = await _context.Contracts.Where(x => x.Id == request.Id && x.UserId == request.UserId && x.OrganisationId == request.OrganisationId
                                && x.ContractStatus == Domain.Enums.ContractStatus.Active).FirstOrDefaultAsync();

                if (contract.ContractExpirationDate >= DateTime.Now)
                {
                    contract.ContractStatus = Domain.Enums.ContractStatus.Expired;
                    contract.ContractStatusDesc = Domain.Enums.ContractStatus.Expired.ToString();
                }


                _context.Contracts.Update(contract);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("This contract has just expired");
            }
            catch (Exception ex)
            {

                return Result.Failure($"Contract status update to expired failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
