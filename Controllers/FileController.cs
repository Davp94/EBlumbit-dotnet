using EBlumbit.Dto.Common;
using EBlumbit.Services.impl;
using EBlumbit.Services.spec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController(IFIleService fileService) : ControllerBase
    {
        private readonly IFIleService _fileService = fileService;

        [HttpGet]
        public async Task<IActionResult> retrieveFile([FromQuery] FileRequest fileRequest)
        {
            var stream = await _fileService.GetFileAsync(fileRequest.FilePath);
            var contentType = "application/octet-stream";
            return File(stream, contentType);
        }
    }
}
