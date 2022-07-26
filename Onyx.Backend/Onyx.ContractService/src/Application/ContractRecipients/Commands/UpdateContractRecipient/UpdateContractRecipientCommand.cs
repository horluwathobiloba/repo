using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipient
{
    public class UpdateContractRecipientCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public int ContractId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public int Rank { get; set; }
        public RecipientCategory RecipientCategory { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractRecipientCommandHandler : IRequestHandler<UpdateContractRecipientCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateContractRecipientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateContractRecipientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var modfiedEntityExists = await _context.ContractRecipients.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Email.ToLower() == request.Email.ToLower());

                if (modfiedEntityExists)
                {
                    return Result.Failure($"Another contract recipient with this email '{request.Email}' already exists. Please change the email.");
                }

                var entity = await _context.ContractRecipients
                    .Where(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id)
                    .Include(a => a.Contract)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid Contract recipient specified.");
                } 
               

                var rankExists = await _context.ContractRecipients
                       .AnyAsync(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId && a.Rank == request.Rank);

                var rank = 0; // get the last ranking if the requested rank already exists
                if (rankExists)
                {
                    var maxRank = await _context.ContractRecipients
                         .Where(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId)
                         .MaxAsync(r => r.Rank);
                    rank = maxRank + 1;
                }
                else
                {
                    rank = request.Rank;
                }
 
                entity.Email = request.Email;
                entity.RecipientCategory = request.RecipientCategory.ToString();
                entity.Designation = request.Designation;
                entity.Name = request.Name;
                entity.Rank = request.Rank;

                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.ContractRecipients.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractRecipientDto>(entity);
                return Result.Success("Contract recipient was updated successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipient update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
    }

}
