namespace FluentValidation.WebApi
{
	using System.Collections.Generic;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using Internal;
	using Resources;
	using Validators;

	internal class RequiredFluentValidationPropertyValidator : FluentValidationPropertyValidator {
		public RequiredFluentValidationPropertyValidator(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule rule, IPropertyValidator validator) : base(metadata, validatorProviders, rule, validator) {
			bool isNonNullableValueType = !TypeAllowsNullValue(metadata.ModelType);
			bool nullWasSpecified = metadata.Model == null;

			ShouldValidate = isNonNullableValueType && nullWasSpecified;
		}		

		public override bool IsRequired {
			get { return true; }
		}
	}
}