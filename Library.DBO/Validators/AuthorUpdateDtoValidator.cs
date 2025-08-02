using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Library.DBO;

namespace Library.API.Validators;

public class AuthorUpdateDtoValidator : AbstractValidator<AuthorUpdateDto>
{
    public AuthorUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id düzgün verilməyib");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Author adı boş ola bilməz")
            .Length(2, 50).WithMessage("Author adı 2 ilə 50 simvol arasında olmalıdır");
    }
}
