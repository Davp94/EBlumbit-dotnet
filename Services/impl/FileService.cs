using System;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class FileService(IConfiguration configuration, IWebHostEnvironment environment) : IFIleService
{
    private readonly IConfiguration _configuration = configuration;

    private readonly IWebHostEnvironment _environment = environment;

    public async Task<Stream> GetFileAsync(string filePath)
    {
        if(string.IsNullOrWhiteSpace(filePath))
        {
            throw new FileNotFoundException("Ruta de archivo no válida");
        }
        var fullPath = Path.Combine(_environment.ContentRootPath, filePath);
        if(!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Archivo no encontrado", fullPath);
        }
        return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
    }

    public async Task<string> SaveFile(IFormFile file)
    {
        if(file == null || file.Length == 0)
        {
            throw new FileNotFoundException("Archivo es nulo o esta vacío");
        }

        var storagePath = _configuration.GetValue<string>("Storage:FileDirectory");
        var uploadPath = Path.Combine(_environment.ContentRootPath, storagePath);
        if(!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }
        var fileName = Guid.NewGuid() + file.FileName;
        var filePath = Path.Combine(uploadPath, fileName);
        var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        var savedFilePath = Path.Combine(uploadPath, fileName);
        return savedFilePath;
    }
}
