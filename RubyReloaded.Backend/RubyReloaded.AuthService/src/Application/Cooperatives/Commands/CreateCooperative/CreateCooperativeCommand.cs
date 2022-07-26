using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Cooperatives.Commands.CreateCooperative
{
    public class CreateCooperativeCommand:IRequest<Result>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string TermsAndConditions { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string License { get; set; }
        public string CooperativeGoal { get; set; }
        public CooperativeType CooperativeType { get; set; }
        public CooperativeSettings CooperativeSettings { get; set; }
        public string State { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class CreateCooperativeCommandHandler : IRequestHandler<CreateCooperativeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
       // private readonly IBase64ToFileConverter _fileConverter;
        private readonly IEmailService _emailService;
        public CreateCooperativeCommandHandler(IApplicationDbContext context, IIdentityService identityService, IBase64ToFileConverter fileConverter, IEmailService emailService)
        {
            _context = context;
            _identityService = identityService;
           // _fileConverter = fileConverter;
            _emailService = emailService;
        }
        public async Task<Result> Handle(CreateCooperativeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                CooperativeSettings cooperativeSettings = request.CooperativeSettings;
                var cooperative = new Cooperative
                {
                  
                    CooperativeGoal = request.CooperativeGoal,
                    CooperativeType = request.CooperativeType,
                    CreatedDate = DateTime.Now,
                    TermsAndConditions = request.TermsAndConditions,
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    Email = request.Email,
                    License = request.License,
                    Location = request.Location,
                    PhoneNumber = request.PhoneNumber,
                    Name = request.Name,
                    
                };
                await _context.Cooperatives.AddAsync(cooperative);
               var res= await _context.SaveChangesAsync(cancellationToken);

                var coopId = cooperative.Id;
                request.CooperativeSettings.CooperativeId=coopId;
                var entitySetting=await _context.CooperativeSettings.AddAsync(cooperativeSettings);
                await _context.SaveChangesAsync(cancellationToken);
                cooperative.CooperativeSettingId = cooperativeSettings.Id;
                cooperative.CooperativeSetting = cooperativeSettings;
                _context.Cooperatives.Update(cooperative);
                await _context.CommitTransactionAsync();
                return Result.Success(cooperative);
            }
            catch (Exception ex)
            {
              _context.RollbackTransaction();
                return Result.Failure(new string[] { "Cooperative creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
