using System;
using System.Collections.Generic;
using SmoothValidator.Extensions;
using SmoothValidator.Util;

namespace SmoothValidator.Example
{
    public class Program
    {
        static void Main(string[] args)
        {
            var createUserRequest = new CreateUserRequest
            {
                Email = "aaaa",
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
        private static readonly ISmoothValidator<CreateUserRequest, string> Validator =
            SmoothValidator<CreateUserRequest, string>
                .Builder()
                .SubValidate(
                    request => request.Email,
                    builder => builder
                        .Length(() => new LengthRange(5, 254), range => $"Email must be between {range} characters.")
                        .True(value => value?.Contains("@") ?? false, () => "Email is invalid.")
                )
                .SubValidate(
                    request => request.FirstName,
                    builder => builder.NotNullOrWhitespace(() => "First name cannot be empty.")
                )
                .SubValidate(
                    request => request.LastName,
                    builder => builder.NotNullOrWhitespace(() => "Last name cannot be empty.")
                )
                .Build();

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Validate(out List<string> errors) => Validator.Validate(this, out errors);
    }
}
