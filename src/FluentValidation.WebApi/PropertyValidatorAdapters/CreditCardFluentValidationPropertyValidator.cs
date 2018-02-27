namespace FluentValidation.WebApi
{
	using System.Collections.Generic;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using Internal;
	using Resources;
	using Validators;

	internal class CreditCardFluentValidationPropertyValidator : FluentValidationPropertyValidator {
		public CreditCardFluentValidationPropertyValidator(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule rule, IPropertyValidator validator) : base(metadata, validatorProviders, rule, validator) {
			ShouldValidate=false;
		}
	}
}