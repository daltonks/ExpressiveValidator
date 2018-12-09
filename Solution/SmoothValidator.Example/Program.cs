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
        public static readonly SmoothValidator<CreateUserRequest, string> Validator = 
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
                .SubValidate(
                    request => request.SubStuff,
                    () => SubStuffy.Validator
                )
                .Build();

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public SubStuffy SubStuff { get; set; } = new SubStuffy();

        public bool Validate(out List<string> errors) => Validator.Validate(this, out errors);

        public class SubStuffy
        {
            public static readonly SmoothValidator<SubStuffy, string> Validator =
                SmoothValidator<SubStuffy, string>
                    .Builder()
                    .NotNull(() => "Substuff cannot be null")
                    .SubValidate(
                        subStuff => subStuff.Thingy,
                        thingyBuilder => thingyBuilder.True(value => value, () => "All substuff thingies must be true.")
                    )
                    .Build();
                    
            public bool Thingy = false;
        }
    }
}
