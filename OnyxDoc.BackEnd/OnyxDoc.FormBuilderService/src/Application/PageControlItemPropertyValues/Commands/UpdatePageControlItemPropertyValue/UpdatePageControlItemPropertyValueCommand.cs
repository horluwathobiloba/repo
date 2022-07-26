using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    public class UpdatePageControlItemPropertyValueCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int PageControlItemPropertyId { get; set; }
        public string Value { get; set; } 
        public string UserId { get; set; }
    }

    public class UpdatePageControlItemPropertyValueCommandHandler : IRequestHandler<UpdatePageControlItemPropertyValueCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePageControlItemPropertyValueCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePageControlItemPropertyValueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.PageControlItemPropertyValues.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.PageControlItemPropertyId ==  request.PageControlItemPropertyId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid page control item property value specified.");
                }
                  
                entity.Name = request.Value;
                entity.Description = request.Value;
                entity.Value = request.Value;

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PageControlItemPropertyValues.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PageControlItemPropertyValueDto>(entity);
                return Result.Success("Page control item property value update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property value update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }


}
