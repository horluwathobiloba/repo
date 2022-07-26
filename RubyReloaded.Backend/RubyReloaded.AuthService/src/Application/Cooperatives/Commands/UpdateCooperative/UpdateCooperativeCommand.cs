using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Cooperatives.Commands.UpdateCooperative
{
    public class UpdateCooperativeCommand : IRequest<Result>
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
        public bool RequestToJoin { get; set; }
        public string Logo { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int Id { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class UpdateCooperativeCommandHandler : IRequestHandler<UpdateCooperativeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        public UpdateCooperativeCommandHandler(IApplicationDbContext context, IIdentityService identityService, IBase64ToFileConverter fileConverter, IEmailService emailService)
        {
            _context = context;
            _identityService = identityService;
            // _fileConverter = fileConverter;
            _emailService = emailService;
        }
        public async Task<Result> Handle(UpdateCooperativeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Cooperatives.FirstOrDefaultAsync(x => x.Id == request.Id);


              
                entity.CooperativeGoal = request.CooperativeGoal;
                entity.CooperativeType = request.CooperativeType;
                entity.CreatedDate = DateTime.Now;
                entity.TermsAndConditions = request.TermsAndConditions;
                entity.StatusDesc = Status.Active.ToString();
                entity.Status = Status.Active;
                entity.Email = request.Email;
                entity.License = request.License;
                entity.Location = request.Location;
                entity.PhoneNumber = request.PhoneNumber;
                entity.Name = request.Name;


                _context.Cooperatives.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Cooperative update was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
