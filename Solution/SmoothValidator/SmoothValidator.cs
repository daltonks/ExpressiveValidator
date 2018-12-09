using System.Collections.Generic;
using System.Linq;

namespace SmoothValidator
{
    public class SmoothValidator<TObject, TError> : ISmoothValidator<TObject, TError>
    {
        public static SmoothValidatorBuilder<TObject, TError> Builder()
        {
            return new SmoothValidatorBuilder<TObject, TError>(obj => (TObject) obj);
        }

        private readonly SmoothValidatorItem[] _validatorItems;

        internal SmoothValidator(IEnumerable<SmoothValidatorItem> validators)
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
                if (!validator.Validate(obj, out var itemErrors))
                {
                    errors.AddRange(itemErrors.Cast<TError>());
                }
            }

            return !errors.Any();
        }
    }
}
