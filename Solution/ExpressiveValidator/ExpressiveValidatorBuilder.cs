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

        public ExpressiveValidatorBuilder<TObject, TError> IsNotEmpty(Expression<Func<TObject, string>> expression, Func<TError> errorProvider)
        {
            return Validate(expression, errorProvider, string.IsNullOrWhiteSpace);
        }

        public ExpressiveValidatorBuilder<TObject, TError> MinLength(
            Expression<Func<TObject, string>> expression, 
            Func<int, TError> errorProvider, 
            int length
        )
        {
            return Validate(
                expression, 
                () => errorProvider.Invoke(length), 
                value => value != null && value.Length >= length
            );
        }

        public ExpressiveValidatorBuilder<TObject, TError> Validate<TMember>(
            Expression<Func<TObject, TMember>> expression, 
            Func<TError> errorProvider,
            Func<TMember, bool> errorPredicate
        )
        {
            var memberExpression = (MemberExpression) expression.Body;
            var memberInfo = memberExpression.Member;

            if (!_memberValidators.TryGetValue(memberInfo, out var propertyValidators))
            {
                _memberValidators[memberInfo] = propertyValidators = new List<ExpressiveMemberValidator>();
            }

            var memberValidator = new ExpressiveMemberValidator(
                value => errorPredicate.Invoke((TMember) value), 
                () => errorProvider.Invoke(), 
                memberInfo
            );
            propertyValidators.Add(memberValidator);

            return this;
        }

        public ExpressiveValidator<TObject, TError> Build()
        {
            var allValidators = _memberValidators.Values().SelectMany(validators => validators);
            return new ExpressiveValidator<TObject, TError>(allValidators);
        }
    }
}
