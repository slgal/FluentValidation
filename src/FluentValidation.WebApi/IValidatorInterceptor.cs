namespace FluentValidation.WebApi {

	using Results;

	/// <summary>
	/// Specifies an interceptor that can be used to provide hooks that will be called before and after WebApi validation occurs.
	/// </summary>
	public interface IValidatorInterceptor {
		/// <summary>
		/// Invoked before WebApi validation takes place which allows the ValidationContext to be customized prior to validation.
		/// It should return a ValidationContext object.
		/// </summary>
		/// TODO param name="controllerContext">Controller Context</param>
		/// <param name="validationContext">Validation Context</param>
		/// <returns>Validation Context</returns>
		ValidationContext BeforeWebApiValidation(/*ControllerContext controllerContext, */ValidationContext validationContext);

		/// <summary>
		/// Invoked after WebApi validation takes place which allows the result to be customized.
		/// It should return a ValidationResult.
		/// </summary>
		/// TODO param name="controllerContext">Controller Context</param>
		/// <param name="validationContext">Validation Context</param>
		/// <param name="result">The result of validation.</param>
		/// <returns>Validation Context</returns>
		ValidationResult AfterWebApiValidation(/*ControllerContext controllerContext,*/ ValidationContext validationContext, ValidationResult result);
	}
}