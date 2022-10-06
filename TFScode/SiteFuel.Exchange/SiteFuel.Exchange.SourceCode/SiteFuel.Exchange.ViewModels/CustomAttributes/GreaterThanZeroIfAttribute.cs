using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class GreaterThanZeroIfAttribute : ValidationAttribute, IClientValidatable
    {

        private string DisplayName { get; set; }

        private string _dependentProperty;

        private object _targetValue;

        public GreaterThanZeroIfAttribute(string dependentProperty, object targetValue)
        {
            this._dependentProperty = dependentProperty;
            this._targetValue = targetValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, DisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DisplayName = validationContext.DisplayName;

            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(this._dependentProperty);
            if (field != null)
            {
                // get the value of the dependent property
                var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);
                if ((dependentvalue == null && this._targetValue == null) ||
                        (dependentvalue != null && dependentvalue.Equals(_targetValue)))
                {
                    return IsGreaterThanZero(Convert.ToDecimal(value)) ? null : new ValidationResult(ErrorMessage);
                }
            }
            return null;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            // build the ID of the property
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(this._dependentProperty);
            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object (our Person), and hence the same level as the dependent property.
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            return depProp;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            DisplayName = metadata.PropertyName;

            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "greaterthanzeroif",
            };

            string depProp = BuildDependentPropertyId(metadata, context as ViewContext);

            rule.ValidationParameters.Add("dependentproperty", depProp);
            rule.ValidationParameters.Add("targetvalue", _targetValue);
            rule.ValidationParameters.Add("fieldvalue", "");
            yield return rule;
        }
        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    DisplayName = metadata.PropertyName;
        //    var clientValidationRule = new ModelClientValidationRule()
        //    {
        //        ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
        //        ValidationType = "greaterthanzeroif"
        //    };

        //    clientValidationRule.ValidationParameters.Add("fieldvalue", "");

        //    return new[] { clientValidationRule };
        //}

        private static bool IsGreaterThanZero(decimal value)
        {
            return value != 0;
        }
    }
}
