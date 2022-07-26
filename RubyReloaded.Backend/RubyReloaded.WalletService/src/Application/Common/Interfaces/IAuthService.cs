using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IAuthService
    {
       
        public UserDto User { get; set; }
        public List<UserDto> Users { get; set; }
        public string AuthToken { get; set; }


        Task<EntityVm<List<UserDto>>> GetUsersAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0);
        Task<EntityVm<UserDto>> GetUserAsync(string authToken, int subscriberId, string userRecordId, string userId);
        Task<EntityVm<List<UserDto>>> GetAllUsersAsync(string authToken, int subscriberId, string userId, int skip = 0, int take = 0);
        Task<GetUserByIdResponse> GetUserById(string authtoken, string userId);
    }
}

