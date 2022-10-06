using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredWithCustomLabelAttribute : RequiredAttribute, IClientValidatable
    {
        private string DisplayName { get; set; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, DisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DisplayName = validationContext.DisplayName;
            if(string.IsNullOrEmpty(DisplayName))
            {
                DisplayName = validationContext.MemberName;
            }
            return base.IsValid(value, validationContext);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(Web.Mvc.ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = string.Format(ErrorMessageString, metadata.DisplayName),
                ValidationType = "required"
            };
        }
    }
}
