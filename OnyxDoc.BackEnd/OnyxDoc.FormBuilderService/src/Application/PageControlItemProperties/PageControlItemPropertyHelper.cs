using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties
{
    internal class PageControlItemPropertyHelper
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public PageControlItemPropertyHelper(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<PageControlItemProperty>> GetPageControlItemProperties(UpdatePageControlItemPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PageControlItemProperty>();

                var pageControlItem = await _context.PageControlItems.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.PageControlItemId);

                if (pageControlItem == null || pageControlItem.Id <= 0)
                {
                    throw new Exception("Invalid page control item specified");
                }
               
                foreach (var item in request.PageControlItemProperties)
                {
                    var entity = item.Id > 0 ? null : await _context.PageControlItemProperties
                      .Where(x => x.SubscriberId == request.SubscriberId && x.PageControlItemId == request.PageControlItemId && x.Id == item.Id)
                      .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new PageControlItemProperty
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            PageControlItemId = request.PageControlItemId,
                            ControlPropertyId = item.ControlPropertyId,
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
                        entity.PageControlItemId = request.PageControlItemId;
                        entity.ControlPropertyId = item.ControlPropertyId;

                        entity.UserId = request.UserId;
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

        public async Task<List<PageControlItemProperty>> GetControls(CreatePageControlItemPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PageControlItemProperty>();

                var PageControlItemProperty = await _context.Controls.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.PageControlItemId);

                if (PageControlItemProperty == null || PageControlItemProperty.Id <= 0)
                {
                    throw new Exception("Invalid control specified");
                }

                foreach (var item in request.PageControlItemProperties)
                {
                    PageControlItemProperty entity = await _context.PageControlItemProperties.Where(x => x.SubscriberId == request.SubscriberId
                        && x.PageControlItemId == request.PageControlItemId && x.Id == item.Id).FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new PageControlItemProperty
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            PageControlItemId = request.PageControlItemId,
                            ControlPropertyId = item.ControlPropertyId,

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
