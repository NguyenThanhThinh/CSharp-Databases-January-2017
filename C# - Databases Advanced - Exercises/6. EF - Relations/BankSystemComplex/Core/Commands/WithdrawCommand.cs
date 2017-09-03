namespace BankSystemComplex.Core.Commands
{
    using Data;
    using Data.Models;
    using System;
    using System.Linq;

    public class WithdrawCommand
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

            // withdraw <Account number> <money>
            string accountNumber = input[0];
            decimal amount = decimal.Parse(input[1]);
            if (amount <= 0)
            {
                throw new ArgumentException("Withdraw amount should be positive!");
            }

            decimal currentBalance;

            using (BankSystemContext context = new BankSystemContext())
            {
                User user = context.Users.Attach(AuthenticationManager.GetCurrentUser());
                SavingAccount savingAccount = user.SavingAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                CheckingAccount checkingAccount = user.CheckingAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (savingAccount == null && checkingAccount == null)
                {
                    throw new ArgumentException($"Account {accountNumber} does not exist!");
                }

                if (savingAccount != null)
                {
                    savingAccount.Balance -= amount;
                    currentBalance = savingAccount.Balance;
                }
                else
                {
                    checkingAccount.Balance -= amount;
                    currentBalance = checkingAccount.Balance;
                }

                context.SaveChanges();
            }

            return $"Account {accountNumber} - ${currentBalance}";
        }
    }
}