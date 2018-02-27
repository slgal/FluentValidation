namespace FluentValidation.WebApi
{
	using System.Collections.Generic;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using Internal;
    using Validators;

    internal class MaxFluentValidationPropertyValidator : AbstractComparisonFluentValidationPropertyValidator<LessThanOrEqualValidator> {

        protected override object MinValue {
            get { return null; }
        }

        protected override object MaxValue {
            get { return AbstractComparisonValidator.ValueToCompare; }
        }

        public MaxFluentValidationPropertyValidator(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule propertyDescription, IPropertyValidator validator)
            : base(metadata, validatorProviders, propertyDescription, validator) {
        }

	    protected override string GetDefaultMessage() {
		    return ValidatorOptions.LanguageManager.GetStringForValidator<LessThanOrEqualValidator>() ;
	    }
    }
}