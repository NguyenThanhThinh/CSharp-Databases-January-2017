namespace Users
{
    using System.Linq;
    using System.Text.RegularExpressions;

    partial class User
    {
        private bool CheckIfLowerLetterIsContained(string password)
        {
            foreach (char letter in password)
            {
                if (char.IsLower(letter))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfUpperLetterIsContained(string password)
        {
            foreach (char letter in password)
            {
                if (char.IsUpper(letter))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfDigitIsContained(string password)
        {
            foreach (char letter in password)
            {
                if (char.IsDigit(letter))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfSpecialSymbolIsContained(string password)
        {
            char[] specialSymbols = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '<', '>', '?' };
            foreach (char letter in password)
            {
                if (specialSymbols.Contains(letter))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfEmailIsValid(string email)
        {
            string regularExpressionString = @"^([a-zA-Z0-9]+(-|\.|_)?)*[a-zA-Z0-9]+@[a-zA-Z0-9]+(-|\.|_)?[a-zA-Z0-9]+((\.)?[a-zA-Z0-9]+)((\.)?[a-zA-Z0-9]+)?$";
            Regex regex = new Regex(regularExpressionString);
            if (!regex.IsMatch(email))
            {
                return false;
            }

            return true;
        }
    }
}
