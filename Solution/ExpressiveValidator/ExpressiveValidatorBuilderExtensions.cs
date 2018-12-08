using System;
using System.Drawing;

namespace ExpressiveValidator
{
    public static class ExpressiveValidatorBuilderExtensions
    {
        // generic Null
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

        // generic NotNull
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

        // string NotNullOrWhitespace
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

        // string Length
        public static SmoothValidatorBuilder<string, TError> Length<TError>(
            this SmoothValidatorBuilder<string, TError> builder,
            int min,
            int max,
            Func<LengthRange, TError> errorProvider
        )
        {
            return builder.Length(() => new LengthRange(min, max), errorProvider);
        }

        // string Length with provider
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

        // string MinLength
        public static SmoothValidatorBuilder<string, TError> MinLength<TError>(
            this SmoothValidatorBuilder<string, TError> builder,
            int length,
            Func<int, TError> errorProvider
        )
        {
            return builder.MinLength(() => length, errorProvider);
        }

        // string MinLength with provider
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
        
        // string MaxLength
        public static SmoothValidatorBuilder<string, TError> MaxLength<TError>(
            this SmoothValidatorBuilder<string, TError> builder,
            int length,
            Func<int, TError> errorProvider
        )
        {
            return builder.MaxLength(() => length, errorProvider);
        }
        
        // string MaxLength with provider
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
