using System.ComponentModel.DataAnnotations;
using FTM.Domain.Exceptions;
using FTM.Domain.Helpers;
using FTM.Domain.Models.Base;
using ValidationException = FTM.Domain.Exceptions.ValidationException;

namespace FTM.Domain.Models.UserModel.DTO;

public record CreateUserDto(
    [Required, EmailAddress] string Email,
    [Required] string UserName,
    [Required] string Password,
    [Required] string ConfirmPassword
) : IValidatable
{
    public void Validate()
    {
        var errors = this.ValidateAndGetErrors();
        if (Password != ConfirmPassword)
        {
            errors[nameof(ConfirmPassword)] = "Password is not confirmed";
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}

public record CreateSimpleUserDto([Required, EmailAddress] string Email) : IValidatable;