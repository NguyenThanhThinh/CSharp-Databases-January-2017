namespace BankSystemComplex.Core.Commands
{
    using Data;
    using Data.Models;
    using System;
    using System.Linq;

    public class DepositCommand
    {
        public string Execute(string[] input)
        {
            if (input.Length != 2)
            {
                throw new ArgumentException("Input is not valid!");
            }

            if (!AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException("You should log in first!");
            }

            // Deposit <Account number> <money>
            string accountNumber = input[0];
            decimal amount = decimal.Parse(input[1]);
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount should be positive!");
            }

            decimal currentBalance;

            using (BankSystemContext context = new BankSystemContext())
            {
                // Try to find the account specified.
                User user = context.Users.Attach(AuthenticationManager.GetCurrentUser());
                SavingAccount savingAccount = user.SavingAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                CheckingAccount checkingAccount = user.CheckingAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                // If not any accounts were not found throw exception.
                if (savingAccount == null && checkingAccount == null)
                {
                    throw new ArgumentException($"Account {accountNumber} does not exist!");
                }

                if (savingAccount != null)
                {
                    savingAccount.Balance += amount;
                    currentBalance = savingAccount.Balance;
                }
                else
                {
                    // NOTE: What about if we have both saving and checking account?
                    checkingAccount.Balance += amount;
                    currentBalance = checkingAccount.Balance;
                }

                context.SaveChanges();
            }

            return $"Account {accountNumber} - ${currentBalance}";
        }
    }
}