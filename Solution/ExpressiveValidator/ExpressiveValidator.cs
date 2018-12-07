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

        public bool Validate(TObject obj, out List<TError> errors)
        {
            errors = new List<TError>();

            foreach (var validator in _validators)
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
