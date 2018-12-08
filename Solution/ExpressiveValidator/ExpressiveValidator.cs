using System.Collections.Generic;
using System.Linq;

namespace ExpressiveValidator
{
    public class ExpressiveValidator<TObject, TError>
    {
        public static ExpressiveValidatorBuilder<TObject, TError> Builder()
        {
            return new ExpressiveValidatorBuilder<TObject, TError>(obj => (TObject) obj);
        }

        private readonly ExpressiveValidatorItem[] _validatorItems;

        internal ExpressiveValidator(IEnumerable<ExpressiveValidatorItem> validators)
        {
            _validatorItems = validators.ToArray();
        }

        public bool Validate(TObject obj)
        {
            return _validatorItems.All(
                validator => validator.Validate(obj, out _)
            );
        }

        public bool Validate(TObject obj, out List<TError> errors)
        {
            errors = new List<TError>();

            foreach (var validator in _validatorItems)
            {
                if (!validator.Validate(obj, out var error))
                {
                    errors.Add((TError) error);
                }
            }

            return errors.Any();
        }
    }
}
