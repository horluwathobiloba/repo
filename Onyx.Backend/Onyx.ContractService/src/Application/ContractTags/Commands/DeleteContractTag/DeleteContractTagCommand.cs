using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTags.Commands.DeleteContractTag
{
    public class DeleteContractTagCommand:IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
    }


    public class DeleteContractTagCommandHandler : IRequestHandler<DeleteContractTagCommand, Result>
    {
        private readonly IApplicationDbContext _context;
       
        public DeleteContractTagCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteContractTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ContractTags.FirstOrDefaultAsync(x => x.Id == request.Id&&x.OrganisationId==request.OrganizationId);
                _context.ContractTags.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Tag deleted successfully",entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract tag delete failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
