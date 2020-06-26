using FluentValidation;

namespace TQ.Authentication.API.Validation
{
    /// <summary>
    /// This is where the custom FluentValidation validators are stored.
    /// </summary>
    public static class CustomValidators
    {
        /// <summary>
        /// Must not start or end with white space.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <returns>a <see cref="IRuleBuilderOptions{T, string}"/></returns>
        public static IRuleBuilderOptions<T, string> NotStartOrEndWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var ruleBuilderOptions = ruleBuilder
                .Must(m => m != null && !m.StartsWith(" ") && !m.EndsWith(" "))
                .WithMessage("'{PropertyName}' must not start or end with white space.");

            return ruleBuilderOptions;
        }
    }
}