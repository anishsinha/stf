using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class EmailAddressExAttribute : ValidationAttribute, IClientValidatable
    {
        private const string RegexPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        private string DisplayName { get; set; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, DisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DisplayName = validationContext.DisplayName;

            if (String.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                return null;
            }
            var email = value.ToString();

            return IsValidEmail(email) ? null : new ValidationResult(ErrorMessage);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            DisplayName = metadata.DisplayName;
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "emailaddress"
            };

            clientValidationRule.ValidationParameters.Add("pattern", "");

            return new[] { clientValidationRule };
        }

        private static bool IsValidEmail(string emailAddress)
        {
            return Regex.IsMatch(emailAddress, RegexPattern, RegexOptions.IgnoreCase);
        }
    }
}
