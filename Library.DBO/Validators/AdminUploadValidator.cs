using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Library.DBO;
using Library.DBO.FileDTOs;

namespace Library.BLL.Validations
{
    public class AdminUploadValidator : AbstractValidator<AdminUploadDTO>
    {
        public AdminUploadValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Fayl boş ola bilməz.")
                .Must(file => file.Length <= 10 * 1024 * 1024) // admin üçün 10 MB
                .WithMessage("Fayl 10 MB-dan böyük ola bilməz.")
                .Must(file =>
                {
                    var allowedExtensions = new[] { ".jpg", ".png", ".pdf", ".docx" };
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    return allowedExtensions.Contains(ext);
                })
                .WithMessage("Yalnız .jpg, .png, .pdf, .docx fayllarına icazə verilir.");

            RuleFor(x => x.TargetUserId)
                .NotEmpty().WithMessage("TargetUserId boş ola bilməz.");
        }
    }
}

