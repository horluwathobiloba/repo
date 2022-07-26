using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractComments.Queries.GetContractComments
{
    public class GetContractCommentByIdQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
    }

    public class GetContractCommentByIdQueryHandler : IRequestHandler<GetContractCommentByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractCommentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetContractCommentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                } 

                var contractComment = await _context.ContractComments.FirstOrDefaultAsync(a => a.OrganisationId == request.OrganisationId && a.Id == request.Id);
                if (contractComment == null)
                {
                    return Result.Failure("Invalid comment Id");
                }

                var result = _mapper.Map<ContractCommentDto>(contractComment);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving comments. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
