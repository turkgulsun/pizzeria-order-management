namespace Pizzeria.Domain.Abstractions.FileAbstractions;

public interface IFileService
{
    Task<string> ReadFileContent(string filePath);
}