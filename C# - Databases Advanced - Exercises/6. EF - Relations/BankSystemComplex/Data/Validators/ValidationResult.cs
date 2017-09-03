namespace BankSystemComplex.Data.Validators
{
    using System.Collections.Generic;

    // Result of validation check.
    public class ValidationResult
    {
        // Initializes a new instance of the "ValidationResult" class.
        // Validation result by default is set to true!
        public ValidationResult()
        {
            this.IsValid = true;
            this.ValidationErrors = new List<string>();
        }

        // Gets the overall validation result.
        public bool IsValid { get; set; }

        // Gets all validation errors (if any).
        public ICollection<string> ValidationErrors { get; set; }
    }
}