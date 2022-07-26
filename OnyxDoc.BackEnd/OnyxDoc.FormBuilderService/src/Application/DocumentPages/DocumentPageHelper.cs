using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Commands;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages
{
    internal class DocumentPageHelper
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DocumentPageHelper(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<DocumentPage>> GetDocumentPages(UpdateDocumentPagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<DocumentPage>();

                var document = await _context.Documents.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.DocumentId);

                if (document == null || document.Id <= 0)
                {
                    throw new Exception("Invalid document specified");
                }

                foreach (var item in request.DocumentPages)
                {
                    var entity = item.Id > 0 ? null : await _context.DocumentPages
                      .Where(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId && x.Id == item.Id)
                      .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new DocumentPage
                        {
                            Name = item.Name,
                            SubscriberId = item.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            DocumentId = item.DocumentId,
                            DisplayName = item.DisplayName,
                            FooterData = item.FooterData,
                            HeaderData = item.HeaderData,
                            PageIndex = item.PageIndex,
                            PageLayout = item.PageLayout,
                            PageNumber = item.PageNumber,
                            Watermark = item.Watermark,

                            Position = item.Position,
                            Transform = item.Transform,
                            Height = item.Height,
                            Width = item.Width,

                            UserId = request.UserId,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                    }
                    else
                    {
                        entity.Name = item.Name;
                        entity.SubscriberName = _authService.Subscriber?.Name;
                        entity.DocumentId = item.DocumentId;
                        entity.DisplayName = item.DisplayName;
                        entity.FooterData = item.FooterData;
                        entity.HeaderData = item.HeaderData;
                        entity.PageIndex = item.PageIndex;
                        entity.PageLayout = item.PageLayout;
                        entity.PageNumber = item.PageNumber;
                        entity.Watermark = item.Watermark;

                        entity.Position = item.Position;
                        entity.Transform = item.Transform;
                        entity.Height = item.Height;
                        entity.Width = item.Width;

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DocumentPage>> GetDocumentPages(CreateDocumentPagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<DocumentPage>();

                var pageControlItem = await _context.PageControlItems.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.DocumentId);

                if (pageControlItem == null || pageControlItem.Id <= 0)
                {
                    throw new Exception("Invalid page control item specified");
                }

                foreach (var item in request.DocumentPages)
                {
                    DocumentPage entity = await _context.DocumentPages.Where(x => x.SubscriberId == request.SubscriberId
                        && x.DocumentId == request.DocumentId && x.Id == item.Id).FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new DocumentPage
                        {
                            Name = item.Name,
                            SubscriberId = item.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            DocumentId = item.DocumentId,
                            DisplayName = item.DisplayName,
                            FooterData = item.FooterData,
                            HeaderData = item.HeaderData,
                            PageIndex = item.PageIndex,
                            PageLayout = item.PageLayout,
                            PageNumber = item.PageNumber,
                            Watermark = item.Watermark,

                            Position = item.Position,
                            Transform = item.Transform,
                            Height = item.Height,
                            Width = item.Width,

                            UserId = request.UserId,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                        list.Add(entity);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
