using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractComments.Queries.GetContractComments;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractComments.Commands.CreateContractComment
{
    public class CreateContractCommentCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        public ContractCommentType ContractCommentType { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
    }

    public class CreateContractCommentCommandHandler : IRequestHandler<CreateContractCommentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateContractCommentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateContractCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                

                var entity = new Domain.Entities.ContractComment
                {
                    ContractId = request.ContractId,
                    Comment = request.Comment,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = request.OrganisationName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    CommentById = request.UserId,
                    ContractCommentType = request.ContractCommentType
                    

                };

                await _context.ContractComments.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractCommentDto>(entity);
                return Result.Success("Contract comment created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract comment creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
