using DriveApp.Dto;
using DriveApp.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveApp.Repository
{
    public class FolderRepository : IFolderRepository
    {
        private readonly AppDbContext _context;

        public FolderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<Folder>> Create(FolderRequestDto folder)
        {
            var response = new Response<Folder>();
            var newFoler = new Folder()
            {
                Name = folder.Name
            };
            _context.Folders.Add(newFoler);
            await _context.SaveChangesAsync();
            response.DataT = newFoler;
            response.Success = true;
            response.Message = "Folder Created";

            return response;
        }

        public async Task<bool> Delete(int id)
        {
            var folder = await Get(id);
            if (folder == null)
            {
                return false;
            }
            _context.Remove(folder);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Response<Folder>> Get(int id)
        {
            var response = new Response<Folder>();
            var folder = await _context.Folders.Where(x => x.Id == id).ToListAsync();
            if (folder is null)
            {
                response.Message = "Invalid folder Id";
                response.Data = null;
                response.Success = false;
                return response;
            }
            response.Message = "Valid folder Id";
            response.Success = true;
            response.Data = folder;
            return response;
        }

        public async Task<Response<Folder>> GetAll()
        {
            var response = new Response<Folder>();
            var folders = await _context.Folders.ToListAsync();
            if (folders is null)
            {
                response.Message = "No folder yet";
                response.Data = null;
                response.Success = false;
                return response;
            }
            response.Message = "Folders available";
            response.Data = folders;
            response.Success = true;
            return response;
        }

        public async Task<Response<Folder>> Update(int folderId, FolderRequestDto folder)
        {
            var response = new Response<Folder>();

            var folderResponse = await _context.Folders.FindAsync(folderId);
            
            if (folderResponse is null)
            {
                response.Data = null;
                response.Message = "Invalid Id";
                response.Success = false;
            }
            folderResponse.Name = folder.Name;
             _context.Update(folderResponse);
            await _context.SaveChangesAsync();
            response.Message = "Update Successful";
            response.DataT = folderResponse;

            return response;
        }
    }
}
