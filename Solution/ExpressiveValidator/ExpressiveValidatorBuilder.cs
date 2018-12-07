using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressiveValidator
{
    public class ExpressiveValidatorBuilder<TObject, TError>
    {
        private readonly OrderedDictionary<MemberInfo, List<ExpressiveMemberValidator>> _memberValidators 
            = new OrderedDictionary<MemberInfo, List<ExpressiveMemberValidator>>();

        public ExpressiveValidatorBuilder<TObject, TError> IsNotEmpty(Expression<Func<TObject, string>> expression, TError error)
        {
            return Validate(expression, error, string.IsNullOrWhiteSpace);
        }

        public ExpressiveValidatorBuilder<TObject, TError> Validate<TProperty>(
            Expression<Func<TObject, TProperty>> expression, 
            TError error,
            Func<TProperty, bool> errorPredicate
        )
        {
            var memberExpression = (MemberExpression) expression.Body;
            var memberInfo = memberExpression.Member;

            if (!_memberValidators.TryGetValue(memberInfo, out var propertyValidators))
            {
                _memberValidators[memberInfo] = propertyValidators = new List<ExpressiveMemberValidator>();
            }

            propertyValidators.Add(new ExpressiveMemberValidator(value => errorPredicate.Invoke((TProperty) value), error, memberInfo));

            return this;
        }

        public ExpressiveValidator<TObject, TError> Build()
        {
            var allValidators = _memberValidators.Values().SelectMany(validators => validators);
            return new ExpressiveValidator<TObject, TError>(allValidators);
        }
    }
}
