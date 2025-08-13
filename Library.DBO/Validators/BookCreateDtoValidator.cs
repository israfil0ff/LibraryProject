using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Library.DBO;

namespace Library.API.Validators;

public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
{
    public BookCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Kitabın adı boş ola bilməz")
            .Length(2, 100).WithMessage("Kitabın adı 2 ilə 100 simvol arasında olmalıdır");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("AuthorId 0-dan böyük olmalıdır");
    }
}
