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

namespace RubyReloaded.AuthService.Application.SystemOwner.UpdateSystemOwner.UpdateSystemOwnerCommand
{
    public class UpdateSystemOwnerCommand:IRequest<Result>
    {
        public int SystemOwnerId { get; set; }
        public string ContactEmail { get; set; }
        public string RCNumber { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string Name { get; set; }
        public string LoggedInUser { get; set; }
    }
    public class UpdateSystemOwnerCommandHandler : IRequestHandler<UpdateSystemOwnerCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateSystemOwnerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateSystemOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.SystemOwners.FirstOrDefaultAsync(x => x.Id == request.SystemOwnerId);
                if (entity is null)
                {
                    return Result.Failure("Name Already Exist");
                }

                entity.ContactEmail = request.ContactEmail;
                entity.Status = Status.Active;
                entity.StatusDesc = Status.Active.ToString();
                entity.Name = request.Name;
                entity.ContactPhoneNumber = request.ContactPhoneNumber;
                entity.CreatedDate = DateTime.Now;
               
                _context.SystemOwners.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure("Ajo update was not successful" +ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
