namespace BankSystemComplex.Core
{
    using System;
    using System.Linq;
    using Commands;
    using Data.Validators;

    // Main goal is to read the input and based on it to execute specific commands.
    public class CommandDispatcher
    {
        // Chooses specific command to execute depending on the input given.
        public string Dispatch(string input)
        {
            string[] inputArgs = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            string commandName = input.Length != 0 ? inputArgs[0].ToLower() : string.Empty;

            inputArgs = inputArgs.Skip(1).ToArray();
            string output = string.Empty;
            switch (commandName)
            {
                case "register":
                    RegisterCommand register = new RegisterCommand() { UserValidator = new UserValidator() };
                    output = register.Execute(inputArgs);
                    break;
                case "login":
                    LoginCommand login = new LoginCommand();
                    output = login.Execute(inputArgs);
                    break;
                case "logout":
                    LogoutCommand logout = new LogoutCommand();
                    output = logout.Execute(inputArgs);
                    break;
                case "exit":
                    ExitCommand exit = new ExitCommand();
                    output = exit.Execute(inputArgs);
                    break;

                case "add":
                    AddAccountCommand add = new AddAccountCommand()
                    {
                        CheckingAccountValidator = new CheckingAccountValidator(),
                        SavingAccountValidator = new SavingAccountValidator()
                    };
                    output = add.Execute(inputArgs);
                    break;

                case "deposit":
                    DepositCommand deposit = new DepositCommand();
                    output = deposit.Execute(inputArgs);
                    break;

                case "withdraw":
                    WithdrawCommand withdraw = new WithdrawCommand();
                    output = withdraw.Execute(inputArgs);
                    break;

                // List Accounts
                case "listaccounts":
                    ListAccountsCommand list = new ListAccountsCommand();
                    output = list.Execute(inputArgs);
                    break;

                // Deduct Fee
                case "deductfee":
                    DeductFeeCommand deduct = new DeductFeeCommand();
                    output = deduct.Execute(inputArgs);
                    break;

                // Add Interest
                case "addinterest":
                    AddInterestCommand addInterest = new AddInterestCommand();
                    output = addInterest.Execute(inputArgs);
                    break;
                default:
                    throw new ArgumentException($"Command \"{commandName}\" not supported!");
            }

            return output;
        }
    }
}
