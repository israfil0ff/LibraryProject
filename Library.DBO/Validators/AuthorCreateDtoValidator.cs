using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Library.DBO;

namespace Library.API.Validators;

public class AuthorCreateDtoValidator : AbstractValidator<AuthorCreateDto>
{
    public AuthorCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Author adı boş ola bilməz")
            .Length(2, 50).WithMessage("Author adı 2 ilə 50 simvol arasında olmalıdır");
    }
}
