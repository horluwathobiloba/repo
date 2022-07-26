using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SuperAdmin.Command.CreateSuperAdmin
{

    public class SystemOwnerUserDto
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        // public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int RoleId { get; set; }
    }
    public class CreateSystemOwnerUserCommand:IRequest<Result> 
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
       // public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int RoleId { get; set; }
    }
    public class CreateSystemOwnerUserCommandHandler : IRequestHandler<CreateSystemOwnerUserCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;
        public CreateSystemOwnerUserCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper,
            IBase64ToFileConverter fileConverter, IEmailService emailService, IStringHashingService stringHashingService,
            IConfiguration configuration)
        {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
            _mapper = mapper;
            _fileConverter = fileConverter;
            _stringHashingService = stringHashingService;
            _configuration = configuration;
        }

        public async  Task<Result> Handle(CreateSystemOwnerUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdBy = "";
                await _context.BeginTransactionAsync();
                var user = await _identityService.GetUserByEmail(request.Email);
                if (user.user != null)
                {
                    return Result.Failure(new string[] { "User already exists with this email" });
                }
                //Chnage the roile table name 
                var role = await _context.SystemOwnerRoles.FirstOrDefaultAsync(x => x.Id==request.RoleId);
                var owner = await _context.SystemOwners.FirstOrDefaultAsync();
                var newUser = new Domain.Entities.User
                {
                    UserAccessLevel=UserAccessLevel.SystemOwner,
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                   // PhoneNumber = request.PhoneNumber,
                    // RoleId = role.Id,
                    Password = request.Password.Trim(),
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy,
                    ProfilePicture = await _fileConverter.ConvertBase64StringToFile(request.ProfilePicture, request.FirstName + "_" + request.LastName + ".png"),
                };

                newUser.Name = string.Concat(newUser.FirstName, " ", newUser.LastName);
                var result = await _identityService.CreateUserAsync(newUser);
                if (!result.Result.Succeeded)
                {
                    return Result.Failure(result.Result.Messages);
                }
                // we need to create a SystemOwnerUserMapping
                await _context.SaveChangesAsync(cancellationToken);
                var systemOwnerUser = new Domain.Entities.SystemOwnerUsers
                {
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    SystemOwnerId = owner.Id,
                    CreatedDate = DateTime.Now,
                    Email = request.Email,
                    Name = request.FirstName + " " + request.LastName,
                    UserId = newUser.UserId,
                };
                await _context.SystemOwnerUsers.AddAsync(systemOwnerUser);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("SystemOwnerUser Created Successfully");
                
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

}
