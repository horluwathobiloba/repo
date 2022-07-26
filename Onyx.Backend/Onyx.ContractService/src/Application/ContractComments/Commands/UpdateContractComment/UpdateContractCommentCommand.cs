using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractComments.Queries.GetContractComments;
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractComments.Commands.UpdateContractComment
{
    public class UpdateContractCommentCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public int ContractId { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractCommentCommandHandler : IRequestHandler<UpdateContractCommentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateContractCommentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateContractCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
               // var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.ContractComments.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid contract comment specified.");
                }

                entity.Comment = request.Comment;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.ContractComments.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractCommentDto>(entity);
                return Result.Success("Contract comment update was successful!", result);
            }
            catch (Exception ex)
            {
               return Result.Failure($"Contract comment update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
