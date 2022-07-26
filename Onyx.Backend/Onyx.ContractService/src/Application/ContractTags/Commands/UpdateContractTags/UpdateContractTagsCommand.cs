using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTag.Queries.GetContractTags;
using Onyx.ContractService.Application.ContractTags.Queries.GetContractTags;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTags.Commands.UpdateContractTags
{
    public class UpdateContractTagsCommand : AuthToken, IRequest<Result>
    {
        public List<UpdateContractTagRequest> ContractTags { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractTagsCommandHandler : IRequestHandler<UpdateContractTagsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateContractTagsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateContractTagsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<Domain.Entities.ContractTag>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.ContractTags)
                {
                    //check if the name of the task tag already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.ContractTags
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != item.Id && x.Name.ToLower().Trim() == item.Name.ToLower().Trim());

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"Another task tag named {item.Name} already exists. Please change the name and try again.");
                    }
                    var entity = await _context.ContractTags.Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new Domain.Entities.ContractTag
                        {
                            Name = item.Name,
                            OrganisationId = request.OrganisationId,
                            OrganisationName =  _authService.Organisation?.OrganisationName,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                    }
                    else
                    {
                        entity.Name = item.Name;
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.ContractTags.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                var result = _mapper.Map<List<ContractTagDto>>(list);
                return Result.Success("Task tag update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Task Tag update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
