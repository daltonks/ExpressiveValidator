using System.Collections.Generic;
using System.Linq;

namespace ExpressiveValidator
{
    public class ExpressiveValidator<TObject, TError>
    {
        private readonly ExpressiveMemberValidator[] _validators;

        internal ExpressiveValidator(IEnumerable<ExpressiveMemberValidator> validators)
        {
            _validators = validators.ToArray();
        }

        public IEnumerable<TError> Validate(TObject obj)
        {
            foreach (var validator in _validators)
            {
                if (!validator.Validate(obj, out var error))
                {
                    yield return (TError) error;
                }
            }
        }
    }
}
