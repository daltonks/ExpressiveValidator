using System;
using System.Collections.Generic;

namespace ExpressiveValidator.Example
{
    public class Program
    {
        static void Main(string[] args)
        {
            var createUserRequest = new CreateUserRequest
            {
                Email = "aaa",
                FirstName = "Dalton"
            };

            createUserRequest.Validate(out var validationErrors);

            Console.WriteLine(
                string.Join(Environment.NewLine, validationErrors)
            );

            Console.WriteLine("Done! Press enter to exit.");
            Console.ReadLine();
        }
    }

    public class CreateUserRequest
    {
        private static readonly ExpressiveValidator<CreateUserRequest, string> Validator =
            ExpressiveValidator<CreateUserRequest, string>
                .Builder()
                .ValidateChild(
                    request => request.Email,
                    builder => builder
                        .MaxLength(254, maxLength => $"Email cannot exceed {maxLength} characters.")
                        .Validate(value => value?.Contains("@") ?? false, () => "Email is invalid.")
                )
                .ValidateChild(
                    request => request.FirstName,
                    builder => builder.IsNotNullOrWhitespace(() => "First name cannot be empty.")
                )
                .ValidateChild(
                    request => request.LastName,
                    builder => builder.IsNotNullOrWhitespace(() => "Last name cannot be empty.")
                )
                .Build();

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Validate(out List<string> errors) => Validator.Validate(this, out errors);
    }
}
