using FTM.Domain.Helpers;

namespace FTM.Domain.Models.Base;

public interface IValidatable
{
    public void Validate()
    {
        this.ValidateAndThrow();
    }
}