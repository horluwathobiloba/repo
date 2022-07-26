using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Branding.Queries.GetBranding;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Branding.Commands.CreateBranding
{
    public class CreateBrandingCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string Logo { get; set; }
        public string ThemeColor { get; set; }
        public string ThemeColorCode { get; set; }
        public string FooterInformation { get; set; }
        public bool HasFooterInformation { get; set; }
        public string UserId { get; set; }
    }


    public class CreateBrandingCommandHandler : IRequestHandler<CreateBrandingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _base64ToFileConverter;

        public CreateBrandingCommandHandler(IApplicationDbContext context, IIdentityService identityService, IBase64ToFileConverter base64ToFileConverter, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _base64ToFileConverter = base64ToFileConverter;
        }

        public async Task<Result> Handle(CreateBrandingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);

                if (userCheck.user == null)
                {
                    return Result.Failure("Invalid Subscriber and User Specified");
                }
                await _context.BeginTransactionAsync();

                //get previously existing branding and make it inactive
                var existingBranding = await _context.Brandings.Where(a=>a.SubscriberId == request.SubscriberId).ToListAsync();
                if (existingBranding != null && existingBranding.Count > 0)
                {
                    List<Domain.Entities.Branding> brandings = new List<Domain.Entities.Branding>();
                    foreach (var branding in existingBranding)
                    {
                        branding.Status = Status.Inactive;
                        branding.StatusDesc = Status.Inactive.ToString();
                        brandings.Add(branding);
                    }
                    _context.Brandings.UpdateRange(brandings);
                }


                /*var tobe = await _context.Subscribers*/

                var subscriber = await _context.Subscribers.FirstOrDefaultAsync(b => b.Id == request.SubscriberId);
                var entity = new Domain.Entities.Branding
                {
                    SubscriberId = request.SubscriberId,
                    HasFooterInformation = request.HasFooterInformation,
                    FooterInformation = request.FooterInformation,
                    Logo  = string.IsNullOrWhiteSpace(request.Logo) ? null : await _base64ToFileConverter.ConvertBase64StringToFile(request.Logo,  subscriber.ContactEmail + "_logo.png"),
                    ThemeColor = request.ThemeColor,
                    ThemeColorCode = request.ThemeColorCode,
                    CreatedById = request.UserId,
                    CreatedByEmail = userCheck.user.Email,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.Brandings.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                var result = _mapper.Map<BrandingDto>(entity);
                return Result.Success("Branding was created successfully", entity);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Branding creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }

}

