using Library.BLL.Interfaces;
using Library.DAL.Repositories;
using Library.DBO.FileDTOs;
using Library.Entities;
using Microsoft.AspNetCore.Hosting;

namespace Library.BLL.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _repository;
        private readonly IWebHostEnvironment _env;

        public FileService(IFileRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public async Task<IEnumerable<FileDto>> GetAllAsync()
        {
            var files = await Task.FromResult(_repository.GetType()
                .GetProperty("Files") != null ? _repository.GetType() : null);

            return files switch
            {
                null => new List<FileDto>(),
                _ => await Task.FromResult(_repository.GetType()
                    .GetProperty("Files") != null ? new List<FileDto>() : new List<FileDto>())
            };
        }

        public async Task<FileDto?> GetByIdAsync(int id)
        {
            var file = await _repository.GetFileAsync(id);
            if (file == null) return null;

            return new FileDto
            {
                Id = file.Id,
                FileName = file.FileName,
                FilePath = file.FilePath,
                ContentType = file.ContentType,
                UploadedAt = file.UploadedAt
            };
        }

        public async Task<FileDto> UploadAsync(FileUploadDto dto)
        {
            var uploadsPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsPath);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.File.FileName);
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var entity = new FileEntity
            {
                FileName = dto.File.FileName,
                FilePath = "/uploads/" + uniqueFileName,
                ContentType = dto.File.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            await _repository.AddFileAsync(entity);

            return new FileDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                FilePath = entity.FilePath,
                ContentType = entity.ContentType,
                UploadedAt = entity.UploadedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var file = await _repository.GetFileAsync(id);
            if (file == null) return false;

            var physicalPath = Path.Combine(_env.WebRootPath ?? "wwwroot", file.FilePath.TrimStart('/').Replace("/", "\\"));
            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            return await _repository.DeleteFileAsync(id);
        }
    }
}
