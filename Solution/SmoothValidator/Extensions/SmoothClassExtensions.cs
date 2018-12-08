using System;

namespace SmoothValidator.Extensions
{
    public static partial class SmoothExtensions
    {
        // Null
        public static SmoothValidatorBuilder<TObject, TError> Null<TObject, TError>(
            this SmoothValidatorBuilder<TObject, TError> builder,
            Func<TError> errorProvider
        ) where TObject : class
        {
            return builder.Validate(
                value => value == null, 
                errorProvider
            );
        }

        // NotNull
        public static SmoothValidatorBuilder<TObject, TError> NotNull<TObject, TError>(
            this SmoothValidatorBuilder<TObject, TError> builder,
            Func<TError> errorProvider
        ) where TObject : class
        {
            return builder.Validate(
                value => value != null, 
                errorProvider
            );
        }
    }
}
