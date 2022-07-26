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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Branding.Commands.UpdateBranding
{
    public class UpdateBrandingCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string Logo { get; set; }
        public string ThemeColor { get; set; }
        public string ThemeColorCode { get; set; }
        public string FooterInformation { get; set; }
        public bool HasFooterInformation { get; set; }
    }

    public class UpdateBrandingCommandHandler : IRequestHandler<UpdateBrandingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _base64ToFileConverter;

        public UpdateBrandingCommandHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _base64ToFileConverter = base64ToFileConverter;
        }
        public async Task<Result> Handle(UpdateBrandingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);

                if (userCheck.user == null)
                {
                    return Result.Failure("Invalid Subscriber and User Specified");
                }
                var entity = await _context.Brandings
                    .Where(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid Document Branding specified.");
                }
                var logo = string.IsNullOrWhiteSpace(request.Logo) ? null : await _base64ToFileConverter.ConvertBase64StringToFile(request.Logo, userCheck.user.FirstName + "_" + userCheck.user.LastName + "_logo.png");
                entity.FooterInformation = request.FooterInformation;
                entity.HasFooterInformation = request.HasFooterInformation;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedByEmail = userCheck.user.Email;
                entity.LastModifiedById = request.UserId;
                entity.Logo = logo;
                entity.ThemeColor = request.ThemeColor;
                entity.ThemeColorCode = request.ThemeColorCode;

                _context.Brandings.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<BrandingDto>(entity);
                return Result.Success("Document Branding was updated successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document Branding update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }

}
