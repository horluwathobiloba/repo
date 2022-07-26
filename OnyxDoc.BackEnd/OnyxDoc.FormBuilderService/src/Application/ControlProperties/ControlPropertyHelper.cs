using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Commands;
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
    internal class ControlPropertyHelper
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public ControlPropertyHelper(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<List<ControlProperty>> GetControlProperties(UpdateControlPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<ControlProperty>();

                var control = await _context.Controls.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.ControlId);

                if (control == null || control.Id <= 0)
                {
                    throw new Exception("Invalid control specified");
                }

                foreach (var item in request.ControlProperties)
                {
                    var entity = item.Id > 0 ? null : await _context.ControlProperties
                        .Where(x => x.SubscriberId == request.SubscriberId && x.ControlId == request.ControlId && x.Id == item.Id)
                        .FirstOrDefaultAsync();

                    var index = 0;
                    if (entity == null || item.Id <= 0)
                    {
                        //Auto Incremement Index: 
                        index++;

                        if (entity == null)
                        {
                            entity = new ControlProperty
                            {
                                Name = item.Name,
                                SubscriberId = item.SubscriberId,
                                SubscriberName = _authService.Subscriber?.Name,
                                ControlId = request.ControlId,
                                Description = item.Description,
                                DisplayName = item.DisplayName,
                                Index = item.Index,
                                ControlPropertyType = item.ControlPropertyType,
                                ControlPropertyTypeDesc = item.ControlPropertyType.ToString(),
                                ControlPropertyValueType = item.ControlPropertyValueType,
                                ControlPropertyValueTypeDesc = item.ControlPropertyValueType.ToString(),
                                ParentPropertyId = item.ParentPropertyId,
                                PropertyTips = item.PropertyTips,
                                ShowInContextMenu = item.ShowInContextMenu,
                               

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
                            entity.Name = item.Name;
                            entity.SubscriberName = _authService.Subscriber?.Name;
                            entity.ControlId = request.ControlId;
                            entity.Description = item.Description;
                            entity.DisplayName = item.DisplayName;
                            entity.PropertyTips = item.PropertyTips;
                            entity.ControlPropertyType = item.ControlPropertyType;
                            entity.ControlPropertyTypeDesc = item.ControlPropertyType.ToString();
                            entity.ControlPropertyValueType = item.ControlPropertyValueType;
                            entity.ControlPropertyValueTypeDesc = item.ControlPropertyValueType.ToString();
                            entity.ParentPropertyId = item.ParentPropertyId;
                            entity.ShowInContextMenu = item.ShowInContextMenu;

                            entity.LastModifiedBy = request.UserId;
                            entity.LastModifiedDate = DateTime.Now;
                        }

                        foreach (var controlPropertyItem in item.ControlPropertyItems)
                        {
                            if (entity.ControlPropertyItems == null || entity.ControlPropertyItems.Count == 0)
                            {
                                entity.ControlPropertyItems = new List<ControlPropertyItem>();
                                entity.ControlPropertyItems.Add(new ControlPropertyItem
                                {
                                    CreatedBy = request.UserId,
                                    CreatedByEmail = _authService.User?.Email,
                                    Status = Status.Active,
                                    StatusDesc = Status.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                    SubscriberId = request.SubscriberId,
                                    Description = controlPropertyItem.Description,
                                    IsDefaultValue = controlPropertyItem.IsDefaultValue,
                                    Value = controlPropertyItem.Value

                                });
                            }
                            else
                            {
                                entity.ControlPropertyItems.Add(new ControlPropertyItem
                                {
                                    CreatedBy = request.UserId,
                                    CreatedByEmail = _authService.User?.Email,
                                    Status = Status.Active,
                                    StatusDesc = Status.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                    SubscriberId = request.SubscriberId,
                                    Description = controlPropertyItem.Description,
                                    IsDefaultValue = controlPropertyItem.IsDefaultValue,
                                    Value = controlPropertyItem.Value
                                });
                            }
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

        public async Task<List<ControlProperty>> GetControlProperties(CreateControlPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<ControlProperty>();

                var control = await _context.Controls.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.ControlId);

                if (control == null || control.Id <= 0)
                {
                    throw new Exception("Invalid control specified");
                }

                var index = 0;
                foreach (var item in request.ControlProperties)
                {
                    var entity = item.Id > 0 ? null : await _context.ControlProperties
                        .Where(x => x.SubscriberId == request.SubscriberId && x.ControlId == request.ControlId && x.Id == item.Id)
                        .FirstOrDefaultAsync();

                    //Auto Incremement Index: 
                    index++;

                    if (entity == null)
                    {
                        entity = new ControlProperty
                        {
                            Name = item.Name,
                            SubscriberId = item.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            ControlId = request.ControlId,
                            Description = item.Description,
                            DisplayName = item.DisplayName,
                            Index = item.Index,
                            ControlPropertyType = item.ControlPropertyType,
                            ControlPropertyTypeDesc = item.ControlPropertyType.ToString(),
                            ControlPropertyValueType = item.ControlPropertyValueType,
                            ControlPropertyValueTypeDesc = item.ControlPropertyValueType.ToString(),
                            ParentPropertyId = item.ParentPropertyId,
                            PropertyTips = item.PropertyTips,
                            ShowInContextMenu = item.ShowInContextMenu,

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
