using AutoMapper;

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

namespace RubyReloaded.AuthService.Application.SystemOwnerUser
{
    public class SystemOwnerUserRequest
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        // public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int RoleId { get; set; }
    }
    public class CreateSystemOwnerUsersCommand:IRequest<Result>
    {
        public List<SystemOwnerUserRequest> SystemOwnerUserRequests { get; set; }
    }

    public class CreateSystemOwnerUsersCommandHandler : IRequestHandler<CreateSystemOwnerUsersCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IStringHashingService _stringHashingService;
      
        public CreateSystemOwnerUsersCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper,
            IBase64ToFileConverter fileConverter, IEmailService emailService, IStringHashingService stringHashingService
            )
        {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
            _mapper = mapper;
            _fileConverter = fileConverter;
            _stringHashingService = stringHashingService;
            
        }

        public async Task<Result> Handle(CreateSystemOwnerUsersCommand request, CancellationToken cancellationToken)
        {
            try
            {
               
                var owner = await _context.SystemOwners.FirstOrDefaultAsync();
                var systemOwners = new List<Domain.Entities.SystemOwnerUsers>();
                await _context.BeginTransactionAsync();
                foreach (var item in request.SystemOwnerUserRequests)
                {

                    var user = await _identityService.GetUserByEmail(item.Email);
                    if (user.user != null)
                    {
                        return Result.Failure(new string[] { "User already exists with this email" });
                    }
                    //Chnage the roile table name 
                    var role = await _context.SystemOwnerRoles.FirstOrDefaultAsync(x => x.Id == item.RoleId);
                    var newUser = new Domain.Entities.User
                    {
                        UserAccessLevel = UserAccessLevel.SystemOwner,
                        Email = item.Email.Trim(),
                        FirstName = item.FirstName.Trim(),
                        LastName = item.LastName.Trim(),
                        // PhoneNumber = request.PhoneNumber,
                        // RoleId = role.Id,
                        Password = item.Password.Trim(),
                        StatusDesc = Status.Active.ToString(),
                        Status = Status.Active,
                        CreatedDate = DateTime.Now,
                        ProfilePicture = await _fileConverter.ConvertBase64StringToFile(item.ProfilePicture, item.FirstName + "_" + item.LastName + ".png"),
                    };

                    newUser.Name = string.Concat(newUser.FirstName, " ", newUser.LastName);
                    var result = await _identityService.CreateUserAsync(newUser);
                    await _context.SaveChangesAsync(cancellationToken);
                    if (!result.Result.Succeeded)
                    {
                        return Result.Failure(result.Result.Messages);
                    }
                    // we need to create a SystemOwnerUserMapping
                    var systemOwnerUser = new Domain.Entities.SystemOwnerUsers
                    {
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString(),
                        SystemOwnerId = owner.Id,
                        CreatedDate = DateTime.Now,
                        Email = item.Email,
                        Name = item.FirstName + " " + item.LastName,
                        UserId = result.UserId ,
                        RoleId=item.RoleId
                    };
                    systemOwners.Add(systemOwnerUser);
                }
                await _context.SystemOwnerUsers.AddRangeAsync(systemOwners);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Users Create Successfully",systemOwners);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure("Creation was not successful " + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
