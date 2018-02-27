namespace FluentValidation.WebApi
{
	using System.Collections.Generic;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using Internal;
	using Validators;

	internal class EqualToFluentValidationPropertyValidator : FluentValidationPropertyValidator {
		EqualValidator EqualValidator {
			get { return (EqualValidator)Validator; }
		}
		
		public EqualToFluentValidationPropertyValidator(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule rule, IPropertyValidator validator) : base(metadata, validatorProviders, rule, validator) {
			ShouldValidate = false;
		}		
	}
}