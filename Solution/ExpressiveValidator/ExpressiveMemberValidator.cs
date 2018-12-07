using System;
using System.Reflection;

namespace ExpressiveValidator
{
    internal class ExpressiveMemberValidator
    {
        private readonly Func<object, object> _getValueFunc;
        private readonly Func<object, bool> _validateFunc;
        private readonly Func<object> _errorProvider;

        public ExpressiveMemberValidator(Func<object, bool> validateFunc, Func<object> errorProvider, MemberInfo memberInfo)
        {
            _validateFunc = validateFunc;
            _errorProvider = errorProvider;

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
                : _errorProvider.Invoke();

            return passedValidation;
        }
    }
}
