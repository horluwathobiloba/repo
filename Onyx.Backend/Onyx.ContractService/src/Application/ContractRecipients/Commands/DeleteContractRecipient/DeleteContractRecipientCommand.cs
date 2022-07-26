using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.DeleteContractRecipient
{
    public class DeleteContractRecipientCommand : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; } 
    }

    public class DeleteContractRecipientCommandHandler : IRequestHandler<DeleteContractRecipientCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteContractRecipientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteContractRecipientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ContractRecipients.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid Contract recipient!");
                }
                 
                _context.ContractRecipients.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractRecipientDto>(entity);
                return Result.Success("Contract recipient is now deleted!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipient status delete failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
