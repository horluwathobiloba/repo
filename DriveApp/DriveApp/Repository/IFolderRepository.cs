using DriveApp.Dto;
using DriveApp.Model;
using System.Threading.Tasks;

namespace DriveApp.Repository
{
    public interface IFolderRepository
    {
        Task<Response<Folder>> GetAll();
        Task<Response<Folder>> Get(int id);
        Task<bool> Delete(int id);
        Task<Response<Folder>> Create(FolderRequestDto folder);
        Task<Response<Folder>> Update(int folderId, FolderRequestDto folder);
    }
}
