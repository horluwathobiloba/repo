using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues
{
    internal class PageControlItemPropertyValueHelper
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public PageControlItemPropertyValueHelper(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<PageControlItemPropertyValue>> GetPageControlItemPropertyValues(UpdatePageControlItemPropertyValuesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PageControlItemPropertyValue>();

                var pciProperty = await _context.PageControlItemProperties.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.PageControlItemPropertyId);

                if (pciProperty == null || pciProperty.Id <= 0)
                {
                    throw new Exception("Invalid page control item property specified");
                }

                foreach (var item in request.PageControlItemPropertyValues)
                {
                    var entity = item.Id > 0 ? null : await _context.PageControlItemPropertyValues.Where(x => x.SubscriberId == request.SubscriberId
                && x.PageControlItemPropertyId == request.PageControlItemPropertyId && x.Id == item.Id).FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new PageControlItemPropertyValue
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            Name = item.Value,
                            Description = item.Value,
                            PageControlItemPropertyId = request.PageControlItemPropertyId,
                            Value = item.Value,

                            CreatedByEmail = _authService.User?.Email,
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
                        entity.Name = item.Value;
                        entity.Description = item.Value;
                        entity.Value = item.Value;

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

        public async Task<List<PageControlItemPropertyValue>> GetPageControlItemPropertyValues(CreatePageControlItemPropertyValuesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PageControlItemPropertyValue>();

                var pciProperty = await _context.PageControlItemProperties.FirstOrDefaultAsync(x => x.Id == request.PageControlItemPropertyId);

                if (pciProperty == null || pciProperty.Id <= 0)
                {
                    throw new Exception("Invalid page control item property specified");
                }

                foreach (var item in request.PageControlItemPropertyValues)
                {
                    PageControlItemPropertyValue entity = await _context.PageControlItemPropertyValues.Where(x => x.SubscriberId == request.SubscriberId
                        && x.PageControlItemPropertyId == request.PageControlItemPropertyId && x.Id == item.Id).FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new PageControlItemPropertyValue
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            Name = item.Value,
                            Description = item.Value,
                            PageControlItemPropertyId = request.PageControlItemPropertyId,
                            Value = item.Value,

                            CreatedByEmail = _authService.User?.Email,
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
