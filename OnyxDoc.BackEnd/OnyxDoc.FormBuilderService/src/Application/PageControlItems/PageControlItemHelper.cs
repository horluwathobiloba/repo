using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Commands;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems
{
    internal class PageControlItemHelper
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public PageControlItemHelper(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<PageControlItem>> GetPageControlItems(UpdatePageControlItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PageControlItem>();

                var documentPage = await _context.DocumentPages.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.DocumentPageId);

                if (documentPage == null || documentPage.Id <= 0)
                {
                    throw new Exception("Invalid document page specified");
                }

                foreach (var item in request.PageControlItems)
                {
                    var entity = await _context.PageControlItems
                       .Where(x => x.SubscriberId == request.SubscriberId && x.DocumentPageId == request.DocumentPageId && x.Id == item.Id)
                       .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new PageControlItem
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            DocumentPageId = request.DocumentPageId,
                            ControlId = item.ControlId,
                            Height = item.Height,
                            Width = item.Width,
                            Transform = item.Transform,
                            Position = item.Position,

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
                        entity.ControlId = item.ControlId;
                        entity.Height = item.Height;
                        entity.Width = item.Width;
                        entity.Transform = item.Transform;
                        entity.Position = item.Position;

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

        public async Task<List<PageControlItem>> GetPageControlItems(CreatePageControlItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PageControlItem>();

                var documentPage = await _context.DocumentPages.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.DocumentPageId);

                if (documentPage == null || documentPage.Id <= 0)
                {
                    throw new Exception("Invalid document page specified");
                }

                foreach (var item in request.PageControlItems)
                {
                    PageControlItem entity = await _context.PageControlItems.Where(x => x.SubscriberId == request.SubscriberId
                        && x.DocumentPageId == request.DocumentPageId && x.Id == item.Id).FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new PageControlItem
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            DocumentPageId = request.DocumentPageId,
                            ControlId = item.ControlId,
                            Height = item.Height,
                            Width = item.Width,
                            Transform = item.Transform,
                            Position = item.Position,

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
