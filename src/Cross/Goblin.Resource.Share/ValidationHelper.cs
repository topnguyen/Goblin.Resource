using System.Linq;
using FluentValidation;
using Goblin.Core.Errors;
using Microsoft.AspNetCore.Http;

namespace Goblin.Resource.Share
{
    public static class ValidationHelper
    {
        public static void Validate<TValidator, T>(T model) 
            where TValidator : AbstractValidator<T>, new()
            where T : class, new()
        {
            var validator = new TValidator();

            var validatorResults = validator.Validate(model);

            if (!validatorResults.IsValid)
            {
                var errorModel = new GoblinErrorModel
                {
                    Code = StatusCodes.Status400BadRequest.ToString(),
                    Message = string.Join("; ", validatorResults.Errors.Select(x => x.ErrorMessage?.Trim('.'))),
                    AdditionalData =
                        validatorResults.Errors.ToDictionary(x => x.PropertyName, x => (object) x.ErrorMessage)
                };

                throw new GoblinException(errorModel);
            }
        }
    }
}