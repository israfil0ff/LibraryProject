using Library.Entities;
using Library.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly LibraryDbContext _context;

        public FileRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<FileEntity> AddFileAsync(FileEntity file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return file;
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null) return false;

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FileEntity?> GetFileAsync(int id)
        {
            return await _context.Files.FindAsync(id);
        }
    }
}
