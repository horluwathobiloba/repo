using DriveApp.Dto;
using DriveApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DriveApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderRepository _folderRepository;

        public FoldersController(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }
        
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllFolders()
        {
            var result = await _folderRepository.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("get-by-id/{folderId}")]
        public async Task<IActionResult> GetFolders(int folderId)
        {
            var result = await _folderRepository.Get(folderId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("create-folder")]
        public async Task<IActionResult> CreateFolder(FolderRequestDto folder)
        {
            var result = await _folderRepository.Create(folder);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch("update-folder/{folderId}")]
        public async Task<IActionResult> UpdateFolder(int folderId, FolderRequestDto folder)
        {
            var result = await _folderRepository.Update(folderId, folder);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [HttpDelete("delete-folder/{folderId}")]
        public async Task<IActionResult> DeleteFolder(int folderId)
        {
            var result = await _folderRepository.Delete(folderId);
            if (result == true)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


    }
}
;