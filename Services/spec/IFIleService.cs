using System;

namespace EBlumbit.Services.spec;

public interface IFIleService
{
    Task<string> SaveFile(IFormFile file);

    Task<Stream> GetFileAsync(string filePath);
}
