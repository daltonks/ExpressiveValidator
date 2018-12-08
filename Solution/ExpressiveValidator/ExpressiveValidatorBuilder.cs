using System;
using System.Collections.Generic;

namespace ExpressiveValidator
{
    public class ExpressiveValidatorBuilder<TObject, TError>
    {
        private readonly Func<object, TObject> _getObjectFunc;
        private readonly List<ExpressiveValidatorItem> _validatorItems = new List<ExpressiveValidatorItem>();

        internal ExpressiveValidatorBuilder(Func<object, TObject> getObjectFunc)
        {
            _getObjectFunc = getObjectFunc;
        }

        public ExpressiveValidatorBuilder<TObject, TError> Validate(
            Func<TObject, bool> errorPredicate,
            Func<TError> errorProvider)
        {
            var validatorItem = new ExpressiveValidatorItem(
                obj => _getObjectFunc.Invoke(obj),
                value => errorPredicate.Invoke((TObject) value), 
                () => errorProvider.Invoke()
            );

            _validatorItems.Add(validatorItem);

            return this;
        }

        public ExpressiveValidatorBuilder<TObject, TError> ValidateChild<TValue>(
            Func<TObject, TValue> valueProvider,
            Action<ExpressiveValidatorBuilder<TValue, TError>> action
        )
        {
            var subBuilder = new ExpressiveValidatorBuilder<TValue, TError>(obj => valueProvider.Invoke((TObject)obj));
            action.Invoke(subBuilder);
            _validatorItems.AddRange(subBuilder._validatorItems);
            return this;
        }
        
        public ExpressiveValidator<TObject, TError> Build()
        {
            return new ExpressiveValidator<TObject, TError>(_validatorItems);
        }
    }
}
