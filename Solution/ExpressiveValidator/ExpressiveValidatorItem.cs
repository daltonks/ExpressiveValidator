﻿using System;
using System.Reflection;

namespace ExpressiveValidator
{
    internal class ExpressiveValidatorItem
    {
        private readonly Func<object, object> _getValueFunc;
        private readonly Func<object, bool> _validateFunc;
        private readonly Func<object> _errorProvider;

        internal ExpressiveValidatorItem(
            Func<object, object> getValueFunc, 
            Func<object, bool> validateFunc, 
            Func<object> errorProvider)
        {
            _getValueFunc = getValueFunc;
            _validateFunc = validateFunc;
            _errorProvider = errorProvider;
        }

        public bool Validate(object obj, out object error)
        {
            var value = _getValueFunc.Invoke(obj);
            var passedValidation = _validateFunc.Invoke(value);

            error = passedValidation
                ? null
                : _errorProvider.Invoke();

            return passedValidation;
        }
    }
}
