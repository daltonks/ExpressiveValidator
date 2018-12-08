using System;
using System.Linq.Expressions;

namespace ExpressiveValidator
{
    public static class ExpressiveValidatorBuilderExtensions
    {
        // string member IsNotNullOrWhitespace
        public static ExpressiveValidatorBuilder<TObject, TError> IsNotNull<TObject, TError>(
            this ExpressiveValidatorBuilder<TObject, TError> builder,
            Func<TError> errorProvider)
        {
            return builder.Validate(
                value => value != null, 
                errorProvider
            );
        }

        // string member IsNotNullOrWhitespace
        public static ExpressiveValidatorBuilder<string, TError> IsNotNullOrWhitespace<TError>(
            this ExpressiveValidatorBuilder<string, TError> builder,
            Func<TError> errorProvider)
        {
            return builder.Validate(
                str => !string.IsNullOrWhiteSpace(str), 
                errorProvider
            );
        }

        // string member MinLength
        public static ExpressiveValidatorBuilder<string, TError> MinLength<TError>(
            this ExpressiveValidatorBuilder<string, TError> builder,
            int length,
            Func<int, TError> errorProvider
        )
        {
            return builder.Validate(
                value => value != null && value.Length >= length, 
                () => errorProvider.Invoke(length)
            );
        }
        
        // string member MaxLength
        public static ExpressiveValidatorBuilder<string, TError> MaxLength<TError>(
            this ExpressiveValidatorBuilder<string, TError> builder,
            int length,
            Func<int, TError> errorProvider
        )
        {
            return builder.Validate(
                value => value == null || value.Length <= length, 
                () => errorProvider.Invoke(length)
            );
        }
    }
}
