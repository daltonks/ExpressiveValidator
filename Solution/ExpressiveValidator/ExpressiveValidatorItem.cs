using System;
using System.Reflection;

namespace ExpressiveValidator
{
    internal class ExpressiveValidatorItem
    {
        private readonly Func<object, object> _getValueFunc;
        private readonly Func<object, bool> _validateFunc;
        private readonly Func<object> _errorProvider;

        public static ExpressiveValidatorItem FromMember(
            MemberInfo memberInfo, 
            Func<object, bool> validateFunc,
            Func<object> errorProvider)
        {
            Func<object, object> getValueFunc;

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    getValueFunc = obj => propertyInfo.GetValue(obj);
                    break;
                case FieldInfo fieldInfo:
                    getValueFunc = obj => fieldInfo.GetValue(obj);
                    break;
                default:
                    throw new ArgumentException($"{memberInfo} is not a property or field!");
            }

            return new ExpressiveValidatorItem(getValueFunc, validateFunc, errorProvider);
        }

        public static ExpressiveValidatorItem FromInstance(
            Func<object, bool> validateFunc,
            Func<object> errorProvider)
        {
            return new ExpressiveValidatorItem(o => o, validateFunc, errorProvider);
        }

        private ExpressiveValidatorItem(
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
