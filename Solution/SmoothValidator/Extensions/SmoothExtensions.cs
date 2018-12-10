using System;
using System.Collections.Generic;

namespace SmoothValidator.Extensions
{
    public static partial class SmoothExtensions
    {
        // Equals
        public static SmoothValidatorBuilder<TObject, TError> Equals<TObject, TError>(
            this SmoothValidatorBuilder<TObject, TError> builder,
            Func<TObject> objectProvider,
            Func<TError> errorProvider
        )
        {
            return builder.True(
                value => EqualityComparer<TObject>.Default.Equals(value, objectProvider.Invoke()), 
                errorProvider
            );
        }

        // DoesNotEqual
        public static SmoothValidatorBuilder<TObject, TError> DoesNotEqual<TObject, TError>(
            this SmoothValidatorBuilder<TObject, TError> builder,
            Func<TObject> objectProvider,
            Func<TError> errorProvider
        )
        {
            return builder.True(
                value => !EqualityComparer<TObject>.Default.Equals(value, objectProvider.Invoke()), 
                errorProvider
            );
        }
    }
}
