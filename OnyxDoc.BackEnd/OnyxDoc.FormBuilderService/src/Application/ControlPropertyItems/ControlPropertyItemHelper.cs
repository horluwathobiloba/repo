using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands;
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
    internal class ControlPropertyItemHelper
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public ControlPropertyItemHelper(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<ControlPropertyItem>> GetControlPropertyItems(UpdateControlPropertyItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<ControlPropertyItem>();

                var controlProperty = await _context.ControlProperties.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.ControlPropertyId);

                if (controlProperty == null || controlProperty.Id <= 0)
                {
                    throw new Exception("Invalid control property specified");
                }

                foreach (var item in request.ControlPropertyItems)
                {
                    var entity = item.Id > 0 ? null : await _context.ControlPropertyItems
                        .Where(x => x.SubscriberId == request.SubscriberId && x.ControlPropertyId == request.ControlPropertyId && x.Id == item.Id)
                        .FirstOrDefaultAsync();

                    var index = 0;
                    if (entity == null || item.Id <= 0)
                    {
                        //Auto Incremement Index: 
                        index++;

                        if (entity == null)
                        {
                            entity = new ControlPropertyItem
                            {
                                Name = item.Value,
                                Description = item.Description,
                                SubscriberId = request.SubscriberId,
                                SubscriberName = _authService.Subscriber?.Name,
                                ControlPropertyId = request.ControlPropertyId,
                                Index = item.Index,
                                Value = item.Value,
                                IsDefaultValue = item.IsDefaultValue,

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
                            entity.Name = item.Value;
                            entity.Description = item.Description;
                            entity.SubscriberName = _authService.Subscriber?.Name;
                            entity.ControlPropertyId = item.ControlPropertyId;
                            entity.Index = item.Index;
                            entity.Value = item.Value;
                            entity.IsDefaultValue = item.IsDefaultValue;

                            entity.Status = item.Status;
                            entity.StatusDesc = item.Status.ToString();
                            entity.LastModifiedBy = request.UserId;
                            entity.LastModifiedDate = DateTime.Now;
                        }
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

        public async Task<List<ControlPropertyItem>> GetControlPropertyItems(CreateControlPropertyItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<ControlPropertyItem>();

                var control = await _context.ControlPropertyItems.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.ControlPropertyId);

                if (control == null || control.Id <= 0)
                {
                    throw new Exception("Invalid control property specified");
                }

                var index = 0;
                foreach (var item in request.ControlPropertyItems)
                {
                    var entity = item.Id > 0 ? null : await _context.ControlPropertyItems
                        .Where(x => x.SubscriberId == request.SubscriberId && x.ControlPropertyId == request.ControlPropertyId && x.Id == item.Id)
                        .FirstOrDefaultAsync();

                    //Auto Incremement Index: 
                    index = index + 1;

                    if (entity == null)
                    {
                        entity = new ControlPropertyItem
                        {
                            Name = item.Value,
                            Description = item.Description,
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            ControlPropertyId = request.ControlPropertyId,
                            Index = item.Index,
                            Value = item.Value,
                            IsDefaultValue = item.IsDefaultValue,

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
