using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Commands.CreateDocumentSigningLink
{
    public class CreateDocumentSigningLinkCommand : AuthToken, IRequest<Result>
    {
        public int DocumentId { get; set; }
        public string SigningAppUrl { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public int SubscriberId { get; set; }
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
              
                //Create Hash
                var hashValue = (request.Email + DateTime.Now).ToString();
                //create token hash
                var hashedValue = _stringHashingService.CreateDESStringHash(hashValue);
                request.SigningAppUrl += "?key=" + hashedValue;

                return Result.Success("Document link created successfully", new { email = request.Email, appurl = request.SigningAppUrl });
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document link creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
