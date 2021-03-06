﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        public SmoothValidatorBuilder<TObject, TError> True(
            Func<TObject, bool> predicate,
            Func<TError> errorProvider
        )
        {
            var validatorItem = new SmoothValidatorItem(
                obj => _getObjectFunc.Invoke(obj),
                value => predicate.Invoke((TObject) value), 
                value => new List<object> { errorProvider.Invoke() }
            );

            _validatorItems.Add(validatorItem);

            return this;
        }

        public SmoothValidatorBuilder<TObject, TError> SubValidate<TValue>(
            Func<TObject, TValue> valueProvider,
            Action<SmoothValidatorBuilder<TValue, TError>> action
        )
        {
            var subBuilder = new SmoothValidatorBuilder<TValue, TError>(
                obj => valueProvider.Invoke(_getObjectFunc.Invoke(obj))
            );
            action.Invoke(subBuilder);
            _validatorItems.AddRange(subBuilder._validatorItems);
            return this;
        }

        public SmoothValidatorBuilder<TObject, TError> SubValidate<TValue>(
            Func<TObject, TValue> valueProvider,
            Func<ISmoothValidator<TValue, TError>> validatorProvider
        )
        {
            var validatorItem = new SmoothValidatorItem(
                obj => valueProvider.Invoke(_getObjectFunc.Invoke(obj)),
                value => validatorProvider.Invoke().Validate((TValue) value),
                value =>
                {
                    validatorProvider.Invoke().Validate((TValue) value, out var errors);
                    return errors.Cast<object>().ToList();
                }
            );

            _validatorItems.Add(validatorItem);

            return this;
        }
        
        public SmoothValidator<TObject, TError> Build()
        {
            return new SmoothValidator<TObject, TError>(_validatorItems);
        }
    }
}
