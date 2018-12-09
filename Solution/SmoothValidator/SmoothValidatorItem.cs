using System;
using System.Collections.Generic;

namespace SmoothValidator
{
    internal class SmoothValidatorItem
    {
        private readonly Func<object, object> _getValueFunc;
        private readonly Func<object, bool> _validateFunc;
        private readonly Func<object, List<object>> _errorProvider;

        internal SmoothValidatorItem(
            Func<object, object> getValueFunc, 
            Func<object, bool> validateFunc, 
            Func<object, List<object>> errorProvider)
        {
            _getValueFunc = getValueFunc;
            _validateFunc = validateFunc;
            _errorProvider = errorProvider;
        }

        public bool Validate(object obj, out List<object> errors)
        {
            var value = _getValueFunc.Invoke(obj);
            var passedValidation = _validateFunc.Invoke(value);

            errors = passedValidation
                ? null
                : _errorProvider.Invoke(value);

            return passedValidation;
        }
    }
}
