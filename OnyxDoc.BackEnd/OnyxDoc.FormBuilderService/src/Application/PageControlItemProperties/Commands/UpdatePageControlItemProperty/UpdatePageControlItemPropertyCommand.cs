using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class UpdatePageControlItemPropertyCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int PageControlItemId { get; set; }
        public int ControlPropertyId { get; set; }
        public List<UpdatePageControlItemPropertyValueRequest> PageControlItemPropertyValues { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePageControlItemPropertyCommandHandler : IRequestHandler<UpdatePageControlItemPropertyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePageControlItemPropertyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePageControlItemPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = request.Id > 0 ? null : await _context.PageControlItemProperties.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.PageControlItemId == request.PageControlItemId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"No record found!");
                }

                await _context.BeginTransactionAsync();

                entity.PageControlItemId = request.PageControlItemId;
                entity.ControlPropertyId = request.ControlPropertyId;

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                if (request.PageControlItemPropertyValues != null && request.PageControlItemPropertyValues.Count > 0)
                {
                    var command = new UpdatePageControlItemPropertyValuesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        PageControlItemPropertyId = entity.Id,
                        PageControlItemPropertyValues = request.PageControlItemPropertyValues,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
                    };
                    var helper = new PageControlItemPropertyValueHelper(_context, _mapper, _authService);
                    var pciPropertyValues = await helper.GetPageControlItemPropertyValues(command, cancellationToken);
                    //entity.PageControlItemPropertyValues.AddRange(pciPropertyValues);
                    entity.PageControlItemPropertyValues = (pciPropertyValues);

                    #region Do not delete!
                    //    var handler = new UpdatePageControlItemPropertyValuesCommandHandler(_context, _mapper, _authService);
                    //    var pciPropertyValues = await handler.Handle(command, cancellationToken);

                    //    if (pciPropertyValues.Succeeded == false)
                    //    {
                    //        throw new Exception(pciPropertyValues.Error + pciPropertyValues.Message);
                    //    }
                    //    
                    #endregion
                }
                _context.PageControlItemProperties.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<PageControlItemPropertyDto>(entity);
                return Result.Success("Page control item property update was successful!", result);

            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
