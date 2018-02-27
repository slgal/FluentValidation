namespace FluentValidation.WebApi
{
    using Internal;
	using System.Collections.Generic;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using Validators;

    internal class MinFluentValidationPropertyValidator : AbstractComparisonFluentValidationPropertyValidator<GreaterThanOrEqualValidator> {

        protected override object MinValue {
            get { return AbstractComparisonValidator.ValueToCompare;  }
        }

        protected override object MaxValue {
            get { return null; }
        }

        public MinFluentValidationPropertyValidator(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule propertyDescription, IPropertyValidator validator)
            : base(metadata, validatorProviders, propertyDescription, validator) {
        }

	    protected override string GetDefaultMessage() {
		    return ValidatorOptions.LanguageManager.GetStringForValidator<GreaterThanOrEqualValidator>();
	    }
    }
}