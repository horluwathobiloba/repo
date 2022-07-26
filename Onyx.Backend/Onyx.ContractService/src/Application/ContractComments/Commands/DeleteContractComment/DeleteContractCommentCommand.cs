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

namespace Onyx.ContractService.Application.ContractComments.Commands.DeleteContractComment
{
    public class DeleteContractCommentCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public int Id { get; set; }
    }


    public class DeleteContractCommentCommandHandler : IRequestHandler<DeleteContractCommentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public DeleteContractCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteContractCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ContractComments.FirstOrDefaultAsync(x => x.Id == request.Id);
                _context.ContractComments.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Comments deleted successfully",entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract comment delete failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
