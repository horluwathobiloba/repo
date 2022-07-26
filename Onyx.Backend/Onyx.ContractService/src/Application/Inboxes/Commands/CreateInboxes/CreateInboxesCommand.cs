using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes
{
    public class CreateInboxesCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }

        public List<CreateInboxRequest> Inboxes { get; set; }
        public string UserId { get; set; }
    }

    public class CreateInboxesCommandHandler : IRequestHandler<CreateInboxesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateInboxesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateInboxesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = false;
                UserVm user = null;
                if (!request.UserId.Contains("@"))
                {
                    user = await _authService.GetUserAsync(request.AccessToken, request.UserId);
                    response = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                }
                var list = new List<Inbox>();

                await _context.BeginTransactionAsync();

                foreach (var inbox in request.Inboxes)
                {
                    
                    
                    var entity = new Inbox
                    {
                        Name=inbox.Name,
                        Body=inbox.Body,
                        ReciepientEmail=inbox.ReciepientEmail,
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        Delivered=true,
                        CreatedByEmail=inbox.Email,
                        StatusDesc = Status.Active.ToString(),
                        Sender= user == null ? request.UserId : user.Entity.FirstName+" "+user.Entity.LastName,
                        EmailAction=inbox.EmailAction
                    };
                    list.Add(entity);
                }
                await _context.Inboxes.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);

                await _context.CommitTransactionAsync();

                return Result.Success("Inboxes created successfully!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Inboxes creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
