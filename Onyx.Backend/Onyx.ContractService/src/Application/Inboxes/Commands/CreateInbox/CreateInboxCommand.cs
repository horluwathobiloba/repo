using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Inboxes.Queries.GetInboxes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Inboxs.Commands.CreateInbox
{
    public class CreateInboxCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string RecipeintEmail { get; set; }
        public string Body { get; set; }
        public bool Delivered { get; set; }
        public string Email { get; set; }
        public EmailAction EmailAction { get; set; }
    }

    public class CreateInboxCommandHandler : IRequestHandler<CreateInboxCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
         

        public CreateInboxCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;            
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateInboxCommand request, CancellationToken cancellationToken)
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
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //var user = await _authService.GetUserAsync(request.AccessToken, request.UserId);

                var entity = new Domain.Entities.Inbox
                {
                    Name = request.Name,
                    Body=request.Body,
                    Delivered=true,
                    ReciepientEmail=request.RecipeintEmail,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = request.OrganisationName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    Read=false,
                    EmailAction=request.EmailAction,
                    StatusDesc = Status.Active.ToString(),
                    CreatedByEmail=request.Email,
                    Sender = user == null ? request.UserId : user.Entity.FirstName + " " + user.Entity.LastName,
                };

                await _context.Inboxes.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                var result = _mapper.Map<InboxDto>(entity);
                return Result.Success("Inbox created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Inbox creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}

//var orgRresponse = await _authService.GetOrganisationAsync(request.OrganisationId);
                //var userRresponse = await _authService.GetUserAsync(request.UserId);

                //if (orgRresponse == null || !orgRresponse.Succeeded || orgRresponse.Entity == null)
                //{
                //    return Result.Failure($"Invalid organisation specified!");
                //}
                //var org = orgRresponse.Entity;
                //if (org.Name.ToLower() != request.OrganisationName.ToLower())
                //{
                //    return Result.Failure($"Invalid organisation name specified!");
                //}

                //if (userRresponse == null || !userRresponse.Succeeded || userRresponse.Entity.UserId != request.UserId)
                //{
                //    return Result.Failure($"Invalid user specified!");
                //}