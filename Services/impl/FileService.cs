using System;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class FileService(IConfiguration configuration, IWebHostEnvironment environment) : IFIleService
{
    private readonly IConfiguration _configuration = configuration;

    private readonly IWebHostEnvironment _environment = environment;

    public async Task<Stream> GetFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new FileNotFoundException("Ruta de archivo no válida");
        }

        var fullPath = Path.IsPathRooted(filePath)
            ? filePath
            : Path.Combine(GetStoragePath(), filePath);

        if(!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Archivo no encontrado", fullPath);
        }
        return await Task.FromResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
    }

    public async Task<string> SaveFile(IFormFile file)
    {
        if(file == null || file.Length == 0)
        {
            throw new FileNotFoundException("Archivo es nulo o esta vacío");
        }

        var uploadPath = GetStoragePath();
        if(!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }
        var fileName = Guid.NewGuid() + file.FileName;
        var filePath = Path.Combine(uploadPath, fileName);
        var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return fileName;
    }

    private string GetStoragePath()
    {
        var storagePath = _configuration.GetValue<string>("Storage:FileDirectory");
        if (string.IsNullOrWhiteSpace(storagePath))
        {
            return _environment.ContentRootPath;
        }
        return Path.IsPathRooted(storagePath)
            ? storagePath
            : Path.Combine(_environment.ContentRootPath, storagePath);
    }
}
