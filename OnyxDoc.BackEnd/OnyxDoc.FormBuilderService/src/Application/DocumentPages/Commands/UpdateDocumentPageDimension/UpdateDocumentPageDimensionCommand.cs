using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItems;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Commands;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    public class UpdateDocumentPageDimensionCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int DocumentId { get; set; }       

        #region Page Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion

        public List<UpdatePageControlItemRequest> PageControlItems { get; set; }

        public string UserId { get; set; }
    }

    public class UpdateDocumentPageDimensionCommandHandler : IRequestHandler<UpdateDocumentPageDimensionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateDocumentPageDimensionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateDocumentPageDimensionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                  
                var entity = await _context.DocumentPages.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.DocumentId == request.DocumentId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid document page specified.");
                }
                 
                entity.Position = request.Position;
                entity.Transform = request.Transform;
                entity.Height = request.Height;
                entity.Width = request.Width;

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                if (request.PageControlItems != null && request.PageControlItems.Count > 0)
                {
                    var command = new UpdatePageControlItemsCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        DocumentPageId = entity.Id,
                        PageControlItems = request.PageControlItems,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
                    };
                    var helper = new PageControlItemHelper(_context, _mapper, _authService);
                    var pciProperties = await helper.GetPageControlItems(command, cancellationToken);
                    entity.PageControlItems.AddRange(pciProperties);

                    #region Do not delete!
                    //var handler = new UpdatePageControlItemsCommandHandler(_context, _mapper, _authService);
                    //var pciResult = await handler.Handle(command, cancellationToken);
                    ////  var pciResult = _mediator.Send(command).Result;
                    //if (pciResult.Succeeded == false)
                    //{
                    //    throw new Exception(pciResult.Error + pciResult.Message);
                    //}
                    #endregion
                }

                _context.DocumentPages.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<DocumentPageDto>(entity);
                return Result.Success("Document page update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document page update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
