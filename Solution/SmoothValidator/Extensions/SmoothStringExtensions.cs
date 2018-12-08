using System;
using SmoothValidator.Util;

namespace SmoothValidator.Extensions
{
    public static partial class SmoothExtensions
    {
        // NotNullOrWhitespace
        public static SmoothValidatorBuilder<string, TError> NotNullOrWhitespace<TError>(
            this SmoothValidatorBuilder<string, TError> builder,
            Func<TError> errorProvider
        )
        {
            return builder.Validate(
                str => !string.IsNullOrWhiteSpace(str), 
                errorProvider
            );
        }

        // Length
        public static SmoothValidatorBuilder<string, TError> Length<TError>(
            this SmoothValidatorBuilder<string, TError> builder,
            Func<LengthRange> lengthRangeProvider,
            Func<LengthRange, TError> errorProvider
        )
        {
            return builder
                .MinLength(() => lengthRangeProvider.Invoke().Min, _ => errorProvider.Invoke(lengthRangeProvider.Invoke()))
                .MaxLength(() => lengthRangeProvider.Invoke().Max, _ => errorProvider.Invoke(lengthRangeProvider.Invoke()));
        }

        // MinLength
        public static SmoothValidatorBuilder<string, TError> MinLength<TError>(
            this SmoothValidatorBuilder<string, TError> builder,
            Func<int> lengthProvider,
            Func<int, TError> errorProvider
        )
        {
            return builder.Validate(
                value => value != null && value.Length >= lengthProvider.Invoke(), 
                () => errorProvider.Invoke(lengthProvider.Invoke())
            );
        }

        // MaxLength
        public static SmoothValidatorBuilder<string, TError> MaxLength<TError>(
            this SmoothValidatorBuilder<string, TError> builder,
            Func<int> lengthProvider,
            Func<int, TError> errorProvider
        )
        {
            return builder.Validate(
                value => value == null || value.Length <= lengthProvider.Invoke(), 
                () => errorProvider.Invoke(lengthProvider.Invoke())
            );
        }
    }
}
