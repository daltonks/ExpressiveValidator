using System;
using System.Reflection;

namespace ExpressiveValidator
{
    internal class ExpressiveMemberValidator
    {
        private readonly Func<object, object> _getValueFunc;
        private readonly Func<object, bool> _validateFunc;
        private readonly object _validationError;

        public ExpressiveMemberValidator(Func<object, bool> validateFunc, object validationError, MemberInfo memberInfo)
        {
            _validateFunc = validateFunc;
            _validationError = validationError;

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    _getValueFunc = obj => propertyInfo.GetValue(obj);
                    break;
                case FieldInfo fieldInfo:
                    _getValueFunc = obj => fieldInfo.GetValue(obj);
                    break;
            }
        }

        public bool Validate(object obj, out object error)
        {
            var value = _getValueFunc.Invoke(obj);
            var passedValidation = _validateFunc.Invoke(value);

            error = passedValidation
                ? null
                : _validationError;

            return passedValidation;
        }
    }
}
