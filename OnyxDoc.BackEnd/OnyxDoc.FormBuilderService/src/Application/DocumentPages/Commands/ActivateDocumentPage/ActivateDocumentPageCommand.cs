﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    public class ActivateDocumentPageCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string UserId { get; set; }
    }

    public class ActivateDocumentPageCommandHandler : IRequestHandler<ActivateDocumentPageCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public ActivateDocumentPageCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(ActivateDocumentPageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdateDocumentPageStatusCommand()
                {
                    AccessToken = request.AccessToken,
                    Id = request.Id,
                    DocumentId = request.DocumentId,
                    Status = Status.Active,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId
                };
                var result = await new UpdateDocumentPageStatusCommandHandler( _context, _mapper, _authService).Handle(command, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document page activation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
