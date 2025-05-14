using FluentValidation;
using Fundo.Applications.Domain.Models;

namespace Fundo.Applications.Domain.Validations;

public class UserValidationLogin : AbstractValidator<RequestLogin>
{
    public UserValidationLogin()
    {
        RuleFor(x => x).Cascade(CascadeMode.Continue)
          .Must(x => !string.IsNullOrEmpty(x.User)).WithMessage("Please set User")
          .Must(x => !string.IsNullOrEmpty(x.Password)).WithMessage("Please set Password");
    }
}
