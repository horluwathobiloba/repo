using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SystemOwner.CreateSystemOwner
{
    public class CreateSystemOwnerCommand : IRequest<Result>
    {
        public string ContactEmail { get; set; }
        public string RCNumber { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string Name { get; set; }
    }

    public class CreateSystemOwnerCommandHandler : IRequestHandler<CreateSystemOwnerCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public CreateSystemOwnerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<Result> Handle(CreateSystemOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var systemOwner = new Domain.Entities.SystemOwner
                {
                    Name = request.Name,
                    Status = Domain.Enums.Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    ContactEmail =request.ContactEmail,
                    RCNumber=request.RCNumber,
                    ContactPhoneNumber=request.ContactPhoneNumber
                    };
                await _context.SystemOwners.AddAsync(systemOwner);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(systemOwner);
            }
            catch (Exception ex)
            {
                return Result.Failure( "Ajo Member adding was not successful  "+ ex?.Message ?? ex?.InnerException.Message );
            }
        }
    }
}
