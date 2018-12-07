using System;
using System.Linq.Expressions;

namespace ExpressiveValidator
{
    public static class ExpressiveValidatorBuilderExtensions
    {
        // Instance IsNotNull
        public static ExpressiveValidatorBuilder<TObject, TError> IsNotNull<TObject, TError>(
            this ExpressiveValidatorBuilder<TObject, TError> builder,
            Func<TError> errorProvider)
        {
            return builder.Validate(obj => obj != null, errorProvider);
        }

        // string member IsNotNullOrWhitespace
        public static ExpressiveValidatorBuilder<TObject, TError> IsNotNullOrWhitespace<TObject, TError>(
            this ExpressiveValidatorBuilder<TObject, TError> builder,
            Expression<Func<TObject, string>> memberExpression, 
            Func<TError> errorProvider)
        {
            return builder.ValidateMember(
                memberExpression, 
                str => !string.IsNullOrWhiteSpace(str), 
                errorProvider
            );
        }

        // string member MinLength
        public static ExpressiveValidatorBuilder<TObject, TError> MinLength<TObject, TError>(
            this ExpressiveValidatorBuilder<TObject, TError> builder,
            Expression<Func<TObject, string>> memberExpression, 
            Func<int, TError> errorProvider, 
            int length
        )
        {
            return builder.ValidateMember(
                memberExpression, 
                value => value != null && value.Length >= length, 
                () => errorProvider.Invoke(length)
            );
        }
    }
}
