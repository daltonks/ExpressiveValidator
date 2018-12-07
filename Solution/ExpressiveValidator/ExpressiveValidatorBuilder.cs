using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressiveValidator
{
    public class ExpressiveValidatorBuilder<TObject, TError>
    {
        private readonly List<ExpressiveValidatorItem> _validatorItems = new List<ExpressiveValidatorItem>();

        internal ExpressiveValidatorBuilder() { }
        
        public ExpressiveValidatorBuilder<TObject, TError> Validate(
            Func<TObject, bool> errorPredicate,
            Func<TError> errorProvider
        )
        {
            var validatorItem = ExpressiveValidatorItem.FromInstance(
                value => errorPredicate.Invoke((TObject) value), 
                () => errorProvider.Invoke()
            );

            _validatorItems.Add(validatorItem);

            return this;
        }

        public ExpressiveValidatorBuilder<TObject, TError> ValidateMember<TMember>(
            Expression<Func<TObject, TMember>> memberExpression,
            Func<TMember, bool> errorPredicate,
            Func<TError> errorProvider)
        {
            var bodyExpression = (MemberExpression) memberExpression.Body;
            var memberInfo = bodyExpression.Member;
            
            var validatorItem = ExpressiveValidatorItem.FromMember(
                memberInfo,
                value => errorPredicate.Invoke((TMember) value), 
                () => errorProvider.Invoke()
            );

            _validatorItems.Add(validatorItem);

            return this;
        }
        
        public ExpressiveValidator<TObject, TError> Build()
        {
            return new ExpressiveValidator<TObject, TError>(_validatorItems);
        }
    }
}
