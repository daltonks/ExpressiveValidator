using System;
using System.Collections.Generic;

namespace SmoothValidator
{
    public class SmoothValidatorBuilder<TObject, TError>
    {
        private readonly Func<object, TObject> _getObjectFunc;
        private readonly List<SmoothValidatorItem> _validatorItems = new List<SmoothValidatorItem>();

        internal SmoothValidatorBuilder(Func<object, TObject> getObjectFunc)
        {
            _getObjectFunc = getObjectFunc;
        }

        public SmoothValidatorBuilder<TObject, TError> Validate(
            Func<TObject, bool> predicate,
            Func<TError> errorProvider)
        {
            var validatorItem = new SmoothValidatorItem(
                obj => _getObjectFunc.Invoke(obj),
                value => predicate.Invoke((TObject) value), 
                () => errorProvider.Invoke()
            );

            _validatorItems.Add(validatorItem);

            return this;
        }

        public SmoothValidatorBuilder<TObject, TError> SubValidate<TValue>(
            Func<TObject, TValue> valueProvider,
            Action<SmoothValidatorBuilder<TValue, TError>> action
        )
        {
            var subBuilder = new SmoothValidatorBuilder<TValue, TError>(obj => valueProvider.Invoke((TObject)obj));
            action.Invoke(subBuilder);
            _validatorItems.AddRange(subBuilder._validatorItems);
            return this;
        }
        
        public SmoothValidator<TObject, TError> Build()
        {
            return new SmoothValidator<TObject, TError>(_validatorItems);
        }
    }
}
