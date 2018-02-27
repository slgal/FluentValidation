namespace FluentValidation.WebApi
{
	using System.Collections.Generic;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using Internal;
	using Validators;

	internal class RangeFluentValidationPropertyValidator : FluentValidationPropertyValidator {
		InclusiveBetweenValidator RangeValidator {
			get { return (InclusiveBetweenValidator)Validator; }
		}
		
		public RangeFluentValidationPropertyValidator(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule propertyDescription, IPropertyValidator validator) : base(metadata, validatorProviders, propertyDescription, validator) {
			ShouldValidate=false;
		}		
	}
}