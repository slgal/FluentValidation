namespace FluentValidation.WebApi {
    using System;
	using System.Collections.Generic;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using Internal;
    using Validators;

    internal abstract class AbstractComparisonFluentValidationPropertyValidator<TValidator> : FluentValidationPropertyValidator 
        where TValidator: AbstractComparisonValidator {

        protected TValidator AbstractComparisonValidator
        {
            get { return (TValidator)Validator; }
        }

        protected abstract Object MinValue { get; }
        protected abstract Object MaxValue { get; }

        protected AbstractComparisonFluentValidationPropertyValidator(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule propertyDescription, IPropertyValidator validator) : base(metadata, validatorProviders, propertyDescription, validator) {
            ShouldValidate=false;
        }

	    protected abstract string GetDefaultMessage();
    }
}