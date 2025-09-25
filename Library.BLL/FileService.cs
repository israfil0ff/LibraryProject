using Library.BLL.Interfaces;
using Library.DAL.Repositories;
using Library.DBO.FileDTOs;
using Microsoft.AspNetCore.Hosting;
using Library.Entities;

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
            var files = await _repository.GetAllFilesAsync();

            return files.Select(f => new FileDto
            {
                Id = f.Id,
                FileName = f.FileName,
                FilePath = f.FilePath,
                ContentType = f.ContentType,
                UploadedAt = f.UploadedAt,
                FileSize = f.FileSize,
                UserId = f.UserId
            });
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
                UploadedAt = file.UploadedAt,
                FileSize = file.FileSize,
                UserId = file.UserId
            };
        }

        public async Task<FileDto> UploadAsync(FileUploadDto dto, string userId)
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
                UploadedAt = DateTime.UtcNow,
                FileSize = dto.File.Length,
                UserId = userId
            };

            await _repository.AddFileAsync(entity);

            return new FileDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                FilePath = entity.FilePath,
                ContentType = entity.ContentType,
                UploadedAt = entity.UploadedAt,
                FileSize = entity.FileSize,
                UserId = entity.UserId
            };
        }

        public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
        {
            var file = await _repository.GetFileAsync(id);
            if (file == null) return false;

            // user yalnız öz faylını silə bilər
            if (!isAdmin && file.UserId != userId)
                return false;

            var physicalPath = Path.Combine(_env.WebRootPath ?? "wwwroot", file.FilePath.TrimStart('/').Replace("/", "\\"));
            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            return await _repository.DeleteFileAsync(id);
        }
    }
}
