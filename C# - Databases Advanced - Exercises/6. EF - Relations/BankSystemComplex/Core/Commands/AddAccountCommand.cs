namespace BankSystemComplex.Core.Commands
{
    using System;
    using Data;
    using Utilities;
    using Data.Models;
    using Data.Validators;

    // Adds account to currently logged in user.
    public class AddAccountCommand
    {
        // Checking account requirements validator.
        public CheckingAccountValidator CheckingAccountValidator { get; set; }

        // Saving account requirements validator.
        public SavingAccountValidator SavingAccountValidator { get; set; }

        public string Execute(string[] input)
        {
            if (input.Length != 3)
            {
                throw new ArgumentException("Input is not valid!");
            }

            // Add SavingAccount <initial balance> <interest rate>
            if (!AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException("You should log in first!");
            }

            string accountType = input[0];

            string accountNumber = AccountNumberGenerator.GenerateAccountNumber();
            decimal balance = decimal.Parse(input[1]);
            decimal rateOrFee = decimal.Parse(input[2]);

            // Check type of account to add and throw exception if type is not valid.
            if (accountType.Equals("CheckingAccount", StringComparison.OrdinalIgnoreCase))
            {
                CheckingAccount checkingAccount = new CheckingAccount()
                {
                    AccountNumber = accountNumber,
                    Balance = balance,
                    Fee = rateOrFee
                };

                ValidationResult valresult = this.CheckingAccountValidator.IsValid(checkingAccount);

                if (!valresult.IsValid)
                {
                    throw new ArgumentException(string.Join("\n", valresult.ValidationErrors));
                }

                using (BankSystemContext context = new BankSystemContext())
                {
                    // Attaching currently logged-in user means that we are working with the same user entity as in our database.
                    User user = AuthenticationManager.GetCurrentUser();
                    context.Users.Attach(user);
                    checkingAccount.User = user;

                    context.CheckingAccounts.Add(checkingAccount);
                    context.SaveChanges();
                }
            }
            else if (accountType.Equals("SavingAccount", StringComparison.OrdinalIgnoreCase))
            {
                SavingAccount savingAccount = new SavingAccount()
                {
                    AccountNumber = accountNumber,
                    Balance = balance,
                    InterestRate = rateOrFee
                };

                ValidationResult valresult = this.SavingAccountValidator.IsValid(savingAccount);

                if (!valresult.IsValid)
                {
                    throw new ArgumentException(string.Join("\n", valresult.ValidationErrors));
                }

                using (BankSystemContext context = new BankSystemContext())
                {
                    User user = AuthenticationManager.GetCurrentUser();
                    context.Users.Attach(user);
                    savingAccount.User = user;

                    context.SavingAccounts.Add(savingAccount);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new ArgumentException($"Invalid account type {accountType}!");
            }

            return $"Account with number {accountNumber} successfully added.";
        }
    }
}
