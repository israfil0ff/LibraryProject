using Library.BLL.Interfaces;
using Library.DAL.Repositories;
using Library.DBO.FileDTOs;
using Microsoft.AspNetCore.Hosting;
using Library.Entities;
using Library.DBO.HistoryDTOs; 

namespace Library.BLL.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _repository;
        private readonly IWebHostEnvironment _env;
        private readonly IHistoryService _historyService; 

        public FileService(IFileRepository repository, IWebHostEnvironment env, IHistoryService historyService)
        {
            _repository = repository;
            _env = env;
            _historyService = historyService;
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

            
            _historyService.AddHistory(new HistoryCreateDTO
            {
                EntityName = "File",
                EntityId = entity.Id,
                Action = "Upload",
                OldValue = null,
                NewValue = $"FileName: {entity.FileName}, Size: {entity.FileSize} bytes",
                Status = "Success",
                Message = "Yeni fayl uğurla yükləndi.",
                CreatedBy = userId
            });

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
            if (file == null)
                return false;

            
            if (!isAdmin && file.UserId != userId)
                return false;

            var physicalPath = Path.Combine(_env.WebRootPath ?? "wwwroot", file.FilePath.TrimStart('/').Replace("/", "\\"));
            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            var result = await _repository.DeleteFileAsync(id);

            
            _historyService.AddHistory(new HistoryCreateDTO
            {
                EntityName = "File",
                EntityId = id,
                Action = "Delete",
                OldValue = $"FileName: {file.FileName}, Path: {file.FilePath}",
                NewValue = null,
                Status = result ? "Success" : "Failed",
                Message = result ? "Fayl uğurla silindi." : "Fayl silinərkən xəta baş verdi.",
                CreatedBy = userId
            });

            return result;
        }
    }
}
