#region License
// Copyright (c) Jeremy Skinner (http://www.jeremyskinner.co.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at https://github.com/jeremyskinner/FluentValidation
#endregion

namespace FluentValidation.WebApi
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;
	using System.Web.Http.Metadata;
	using System.Web.Http.ModelBinding.Binders;
	using System.Web.Http.Validation;

	using FluentValidation.Attributes;
	using FluentValidation.Internal;
	using FluentValidation.Validators;

	public delegate ModelValidator FluentValidationModelValidationFactory(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, PropertyRule rule, IPropertyValidator validator);


	public class FluentValidationModelValidatorProvider : ModelValidatorProvider {
		public IValidatorFactory ValidatorFactory { get; set; }

		private Dictionary<Type, FluentValidationModelValidationFactory> validatorFactories = new Dictionary<Type, FluentValidationModelValidationFactory>() {
			{ typeof(INotNullValidator), (metadata, validatorProviders, rule, validator) => new RequiredFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) },
			{ typeof(INotEmptyValidator), (metadata, validatorProviders, rule, validator) => new RequiredFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) },
			// email must come before regex.
			{ typeof(IEmailValidator), (metadata, validatorProviders, rule, validator) => new EmailFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) },
			{ typeof(IRegularExpressionValidator), (metadata, validatorProviders, rule, validator) => new RegularExpressionFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) },
			{ typeof(ILengthValidator), (metadata, validatorProviders, rule, validator) => new StringLengthFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) },
			{ typeof(GreaterThanOrEqualValidator), (metadata, validatorProviders, rule, validator) => new MinFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) },
			{ typeof(LessThanOrEqualValidator), (metadata, validatorProviders, rule, validator) => new MaxFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) },
			{ typeof(EqualValidator), (metadata, validatorProviders, rule, validator) => new EqualToFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) },
			{ typeof(CreditCardValidator), (metadata, validatorProviders, rule, validator) => new CreditCardFluentValidationPropertyValidator(metadata, validatorProviders, rule, validator) }
		};

		/// <summary>
		/// Enabling this maintains compatibility with FluentValidation 6.4, where discovery of validators was limited to top level models. 
		/// </summary>
		public bool DisableDiscoveryOfPropertyValidators { get; set; } = false;

		public FluentValidationModelValidatorProvider(IValidatorFactory validatorFactory = null) {
			ValidatorFactory = validatorFactory ?? new AttributedValidatorFactory();			
		}

		/// <summary>
		/// Initializes the FluentValidationModelValidatorProvider using the default options and adds it in to the ModelValidatorProviders collection.
		/// </summary>
		public static void Configure(HttpConfiguration configuration, Action<FluentValidationModelValidatorProvider> configurationExpression = null) {
			configurationExpression = configurationExpression ?? delegate { };

			var provider = new FluentValidationModelValidatorProvider();
			configurationExpression(provider);
		//	configuration.Services.Replace(typeof(IBodyModelValidator), new FluentValidationBodyModelValidator());
			configuration.Services.Add(typeof(ModelValidatorProvider), provider);			
		}

		public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
		{
			if (DisableDiscoveryOfPropertyValidators && IsValidatingProperty(metadata))
			{				
				return new List<ModelValidator>();
			}

			IValidator validator = CreateValidator(metadata);

			if (validator == null)
			{				
				return new List<ModelValidator>();
			}

			return GetValidatorsForModel(metadata, validatorProviders, validator);
		}

		protected IEnumerable<ModelValidator> GetValidatorsForProperty(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, IValidator validator)
		{
			var modelValidators = new List<ModelValidator>();

			if (validator != null)
			{
				var descriptor = validator.CreateDescriptor();

				var validatorsWithRules = from rule in descriptor.GetRulesForMember(metadata.PropertyName)
										  let propertyRule = (PropertyRule)rule
										  let validators = rule.Validators
										  where validators.Any()
										  from propertyValidator in validators
										  let modelValidatorForProperty = GetModelValidator(metadata, validatorProviders, propertyRule, propertyValidator)
										  where modelValidatorForProperty != null
										  select modelValidatorForProperty;

				modelValidators.AddRange(validatorsWithRules);
			}
			/*
			if (validator != null && metadata.IsRequired && AddImplicitRequiredValidator)
			{
				bool hasRequiredValidators = modelValidators.Any(x => x.IsRequired);

				//If the model is 'Required' then we assume it must have a NotNullValidator. 
				//This is consistent with the behaviour of the DataAnnotationsModelValidatorProvider
				//which silently adds a RequiredAttribute
				/*
				if (!hasRequiredValidators)
				{
					modelValidators.Add(CreateNotNullValidatorForProperty(metadata, context));
				}
			} */
			return modelValidators;
		}
		protected virtual ModelValidator GetModelValidator(ModelMetadata meta, IEnumerable<ModelValidatorProvider> providers, PropertyRule rule, IPropertyValidator propertyValidator)
		{
			var type = propertyValidator.GetType();

			var factory = validatorFactories
				.Where(x => x.Key.IsAssignableFrom(type))
				.Select(x => x.Value)
				.FirstOrDefault() ?? ((metadata, validatorProviders, description, validator) => new FluentValidationPropertyValidator(metadata, validatorProviders, description, validator));

			return factory(meta, providers, rule, propertyValidator);
		}
		protected virtual IValidator CreateValidator(ModelMetadata metadata)
		{
			if (IsValidatingProperty(metadata))
			{
				return ValidatorFactory.GetValidator(metadata.ModelType);//ContainerType
			}
			return ValidatorFactory.GetValidator(metadata.ModelType);
		}

		//public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
		//{
		//	if (DisableDiscoveryOfPropertyValidators && IsValidatingProperty(metadata)) {
		//		yield break;
		//	}

		//	IValidator validator = ValidatorFactory.GetValidator(metadata.ModelType);
			
		//	if (validator == null) {
		//		yield break;
		//	}

		//	yield return new FluentValidationModelValidator(metadata, validatorProviders, validator);
		//}

		protected virtual IEnumerable<ModelValidator> GetValidatorsForModel(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders, IValidator validator)
		{
			if (validator != null)
			{
				yield return new FluentValidationModelValidator(metadata, validatorProviders, validator);
			}
		}

		protected virtual bool IsValidatingProperty(ModelMetadata metadata) {
			return metadata.ContainerType != null && !string.IsNullOrEmpty(metadata.PropertyName);
		}
	}
}