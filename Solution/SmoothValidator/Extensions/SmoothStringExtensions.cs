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
                str => !String.IsNullOrWhiteSpace(str), 
                errorProvider
            );
        }

        // Length
        public static SmoothValidatorBuilder<string, TError> Length<TError>(
            this SmoothValidatorBuilder<string, TError> builder,
            int min,
            int max,
            Func<LengthRange, TError> errorProvider
        )
        {
            return builder.Length(() => new LengthRange(min, max), errorProvider);
        }

        // Length with provider
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
            int length,
            Func<int, TError> errorProvider
        )
        {
            return builder.MinLength(() => length, errorProvider);
        }

        // MinLength with provider
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
            int length,
            Func<int, TError> errorProvider
        )
        {
            return builder.MaxLength(() => length, errorProvider);
        }

        // MaxLength with provider
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
