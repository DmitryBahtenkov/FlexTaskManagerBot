using System.ComponentModel.DataAnnotations;
using FTM.Domain.Models.Base;

namespace FTM.Domain.Helpers;

public static class ObjectExtensions
{
    
    public static void ValidateAndThrow(this object obj)
    {
        var context = new ValidationContext(obj);
        var results = new List<ValidationResult>();

        if(!Validator.TryValidateObject(obj, context, results, true))
        {
            var errors = results.ToDictionary(x => x.MemberNames.First(), x => x.ErrorMessage);
            throw new Exceptions.ValidationException(errors);
        }   
    }
    
    public static Dictionary<string, string?> ValidateAndGetErrors(this object obj)
    {
        var context = new ValidationContext(obj);
        var results = new List<ValidationResult>();

        if(!Validator.TryValidateObject(obj, context, results, true))
        {
            return results.ToDictionary(x => x.MemberNames.First(), x => x.ErrorMessage);
        }

        return new Dictionary<string, string?>(0);
    }
}