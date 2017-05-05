namespace BankSystemComplex.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Data.Models;

    // Subtract fee tax from specific account on our current user.
    public class DeductFeeCommand
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

            // DeductFee <Account number> 
            string accountNumber = input[0];

            decimal currentBalance;

            using (BankSystemContext context = new BankSystemContext())
            {
                User user = context.Users.Attach(AuthenticationManager.GetCurrentUser());

                CheckingAccount checkingAccount = user.CheckingAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (checkingAccount == null)
                {
                    throw new ArgumentException($"Account {accountNumber} does not exist!");
                }

                checkingAccount.Balance -= checkingAccount.Fee;
                currentBalance = checkingAccount.Balance;

                context.SaveChanges();
            }

            return $"Account fee deducted {accountNumber} - ${currentBalance}";
        }
    }
}
