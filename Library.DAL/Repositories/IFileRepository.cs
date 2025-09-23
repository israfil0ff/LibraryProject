using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Repositories
{
    public interface IFileRepository
    {
        Task<FileEntity> AddFileAsync(FileEntity file);
        Task<bool> DeleteFileAsync(int id);
        Task<FileEntity?> GetFileAsync(int id);
    }
}
