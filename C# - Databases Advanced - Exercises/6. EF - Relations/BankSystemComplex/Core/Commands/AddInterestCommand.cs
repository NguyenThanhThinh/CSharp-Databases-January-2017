namespace BankSystemComplex.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Data.Models;

    // Adding interest rate to specified account on our currently logged user.
    public class AddInterestCommand
    {
        public string Execute(string[] input)
        {
            if (input.Length != 1)
            {
                throw new ArgumentException("Input is not valid!");
            }

            if (!AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException("You should log in first!");
            }

            // AddInterest <Account number> 
            string accountNumber = input[0];

            decimal currentBalance;

            using (BankSystemContext context = new BankSystemContext())
            {
                User user = context.Users.Attach(AuthenticationManager.GetCurrentUser());
                SavingAccount savingAccount = user.SavingAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (savingAccount == null)
                {
                    throw new ArgumentException($"Account {accountNumber} does not exist!");
                }

                savingAccount.Balance += savingAccount.Balance * savingAccount.InterestRate;
                currentBalance = savingAccount.Balance;

                context.SaveChanges();
            }

            return $"Account interest added {accountNumber} - ${currentBalance}";
        }
    }
}
