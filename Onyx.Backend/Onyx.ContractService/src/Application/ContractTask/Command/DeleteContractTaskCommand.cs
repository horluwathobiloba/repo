using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTask.Command
{
    public class DeleteContractTaskCommand :AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
    }
    public class DeleteContractTaskCommandHandler : IRequestHandler<DeleteContractTaskCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public DeleteContractTaskCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeleteContractTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //check if it exist
                var entity = await _context.ContractTasks.FirstOrDefaultAsync(x => x.Id == request.Id && x.OrganisationId == request.OrganisationId&&x.Status==Domain.Enums.Status.Active);
                if (entity == null)
                {
                    return Result.Failure($"Task with this Id does not exist or task status is not active.");
                }

                //user.Status = Domain.Enums.Status.Deactivated;
                _context.ContractTasks.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Tasks deleted successfully.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Tasks delete failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }
}