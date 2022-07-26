using AutoMapper;
using AutoMapper.Configuration;
using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.User.Command.UpdateUser
{
    public class UpdateUserCommand:IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
         public string UserName { get; set; }
 //       public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
       // public DateTime LastAccessedDate { get; set; }
        //public string UserId { get; set; }
        public string Country { get; set; }
        public Gender Gender { get; set; }
        public string State { get; set; }
        public string ProfilePicture { get; set; }
    
        public string LoggedInUserId { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;
        //private readonly IConfiguration _configuration;

        public UpdateUserCommandHandler(IApplicationDbContext context,IIdentityService identityService,IMapper mapper, IBase64ToFileConverter fileConverter)
        {
            _fileConverter = fileConverter;
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var getUserForUpdate = await _identityService.GetUserByEmail(request.Email);

                if (getUserForUpdate.user == null)
                {
                    return Result.Failure(new string[] { "Invalid Staff details for update" });
                }


                var index = request.Email.IndexOf('@');
                getUserForUpdate.user.Address = string.IsNullOrEmpty(request.Address)?getUserForUpdate.user.Address: request.Address.Trim();
                getUserForUpdate.user.LastModifiedDate = DateTime.Now;
                getUserForUpdate.user.DateOfBirth = request.DateOfBirth;
                getUserForUpdate.user.Email = request.Email.Trim();
                getUserForUpdate.user.FirstName = request.FirstName.Trim();
                getUserForUpdate.user.LastName = request.LastName;
                getUserForUpdate.user.UserName = request.UserName;
                //getUserForUpdate.user.PhoneNumber = request.PhoneNumber;
                getUserForUpdate.user.Gender = request.Gender;
                getUserForUpdate.user.Country = request.Country;
                getUserForUpdate.user.State = request.State;
                getUserForUpdate.user.ProfilePicture=string.IsNullOrEmpty(request.ProfilePicture)? getUserForUpdate.user.ProfilePicture : await _fileConverter.ConvertBase64StringToFile(request.ProfilePicture, DateTime.Now.Ticks + "_" + request.FirstName + "_" + request.LastName + ".png");
                getUserForUpdate.user.Name = string.Concat(getUserForUpdate.user.FirstName, " ", getUserForUpdate.user.LastName);
                await _identityService.UpdateUserAsync(getUserForUpdate.user);
                await _context.SaveChangesAsync(cancellationToken);
//                var userResult = _mapper.Map<UserVm>(getUserForUpdate.user);
                return Result.Success("User details updated successfully", getUserForUpdate.user);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User update was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
            
    }
}
