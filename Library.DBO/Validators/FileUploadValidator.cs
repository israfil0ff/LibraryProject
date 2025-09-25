using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Library.DBO.FileDTOs;

namespace Library.BLL.Validations
{
    public class FileUploadValidator : AbstractValidator<FileUploadDto>
    {
        public FileUploadValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Fayl boş ola bilməz.")
                .Must(file => file.Length <= 5 * 1024 * 1024) // 5 MB limit
                .WithMessage("Fayl 5 MB-dan böyük ola bilməz.")
                .Must(file =>
                {
                    var allowedExtensions = new[] { ".jpg", ".png", ".pdf" };
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    return allowedExtensions.Contains(ext);
                })
                .WithMessage("Yalnız .jpg, .png və .pdf fayllarına icazə verilir.");
        }
    }
}
