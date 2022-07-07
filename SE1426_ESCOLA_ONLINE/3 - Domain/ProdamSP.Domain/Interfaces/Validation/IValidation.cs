using ProdamSP.Domain.Validation;

namespace ProdamSP.Domain.Interfaces.Validation
{
    public interface IValidation<in TEntity>
    {
        ValidationResult Valid(TEntity entity);
    }
}