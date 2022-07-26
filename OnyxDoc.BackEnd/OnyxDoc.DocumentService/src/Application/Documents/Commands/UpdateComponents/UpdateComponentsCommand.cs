using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Constants;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Domain.ViewModels;
using OnyxDoc.DocumentService.Infrastructure.Services;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Documents.Commands.UpdateComponents
{
    public class UpdateComponentsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentId { get; set; }
        public string Email { get; set; }
        public ComponentVm[] Components { get; set; }
    }

    public class UpdateComponentsCommandHandler : IRequestHandler<UpdateComponentsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAuthService _authService;

        public UpdateComponentsCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateComponentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
               // var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.OrganizationId);

                var document = await _context.Documents.Where(a => a.Id == request.DocumentId).FirstOrDefaultAsync();
                if (document == null)
                {
                    return Result.Failure($"Invalid document specified");
                }

                var existingComponents = new List<Component>();
                foreach (var componentForUpdateVm in request.Components)
                {
                    var component = await _context.Components.Where(a => a.DocumentId == request.DocumentId && a.Id == componentForUpdateVm.Id).FirstOrDefaultAsync();
                    if (component == null)
                    {
                        continue;
                    }
                    if (componentForUpdateVm.Value.Contains("data:"))
                    {
                        componentForUpdateVm.Value = componentForUpdateVm.Value.Split(',')[1];
                            var fileBytes = Convert.FromBase64String(componentForUpdateVm.Value);
                        component.Value = await _blobStorageService.UploadFileToBlobAsync("Image_"+DateTime.Now.Ticks, fileBytes, MimeTypes.Png);
                    }
                    else
                    {
                        component.Value = componentForUpdateVm.Value;
                    }
                    component.PageNumber = componentForUpdateVm.PageNumber;
                    component.LastModifiedBy = request.Email;
                    component.LastModifiedDate = DateTime.Now;
                    component.UserId = request.Email;
                    existingComponents.Add(component);
                }

                _context.Components.UpdateRange(existingComponents);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Components was updated successfully", existingComponents);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Component update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }

}
