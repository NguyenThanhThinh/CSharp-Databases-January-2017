namespace BankSystemComplex.Data.Validators
{
    using Models;

    public class SavingAccountValidator
    {
        public ValidationResult IsValid(SavingAccount account)
        {
            ValidationResult result = new ValidationResult();

            if (account == null)
            {
                result.IsValid = false;
                result.ValidationErrors.Add("Account cant be null!");
                return result;
            }

            if (account.AccountNumber.Length != 10)
            {
                result.IsValid = false;
                result.ValidationErrors.Add("Account number length should be exactly 10 symbols!");
            }

            if (account.Balance < 0)
            {
                result.IsValid = false;
                result.ValidationErrors.Add("Account balance should be non-negative!");
            }

            if (account.InterestRate < 0)
            {
                result.IsValid = false;
                result.ValidationErrors.Add("Account interest rate should be non-negative!");
            }

            return result;
        }
    }
}