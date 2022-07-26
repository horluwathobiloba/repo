using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contract.Commands.CreateDocumentSigningLink
{
    public class CreateDocumentSigningLinkCommand : AuthToken, IRequest<Result>
    {
        public int ContractId { get; set; }
        public string SigningAppUrl { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class CreateDocumentSigningLinkCommandHandler : IRequestHandler<CreateDocumentSigningLinkCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IAuthService _authService;

        public CreateDocumentSigningLinkCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService,
                                                 IStringHashingService stringHashingService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _stringHashingService = stringHashingService;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateDocumentSigningLinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);

                var contract = await _context.Contracts.Where(x => x.OrganisationId == request.OrganizationId &&
                                      x.Id == request.ContractId).FirstOrDefaultAsync();
                if (contract == null)
                {
                    return Result.Failure($"Invalid Contract Id");
                }
                //Create Hash
                var hashValue = (request.Email + DateTime.Now).ToString();
                //create token hash
                var hashedValue = _stringHashingService.CreateDESStringHash(hashValue);
                request.SigningAppUrl += "?key=" + hashedValue;

                return Result.Success("Document link created successfully", new { email = request.Email, appurl = request.SigningAppUrl });
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document link creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
