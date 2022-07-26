using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    public class CreatePageControlItemPropertyValueCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PageControlItemPropertyId { get; set; }
        public string Value { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePageControlItemPropertyValueCommandHandler : IRequestHandler<CreatePageControlItemPropertyValueCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePageControlItemPropertyValueCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreatePageControlItemPropertyValueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var pciProperty = await _context.PageControlItemProperties.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.Id == request.PageControlItemPropertyId);

                if (pciProperty == null || pciProperty.Id <= 0)
                {
                    return Result.Failure("Invalid page control item property specified.");
                } 

                var entity = new PageControlItemPropertyValue
                {
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,

                    Name = request.Value,
                    Description = request.Value,
                    PageControlItemPropertyId = request.PageControlItemPropertyId,
                    Value = request.Value,

                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PageControlItemPropertyValues.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PageControlItemPropertyValueDto>(entity);
                return Result.Success("Page control item property value created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property value creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
