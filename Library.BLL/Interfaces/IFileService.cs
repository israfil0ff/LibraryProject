using Library.DBO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DBO.FileDTOs;



namespace Library.BLL.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<FileDto>> GetAllAsync();
        Task<FileDto?> GetByIdAsync(int id);
        Task<FileDto> UploadAsync(FileUploadDto dto);
        Task<bool> DeleteAsync(int id);
    }
}