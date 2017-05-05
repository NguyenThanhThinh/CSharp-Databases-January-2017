namespace BankSystemComplex.Data.Validators
{
    using System.Text.RegularExpressions;
    using Models;

    public class UserValidator
    {
        public ValidationResult IsValid(User user)
        {
            ValidationResult result = new ValidationResult();
            if (user == null)
            {
                result.IsValid = false;
                result.ValidationErrors.Add("User cannot be null!");

                // If user is null there is no way that we cant it's username or password
                // (they simply ain't set) so we return directly our result.
                return result;
            }

            Regex usernameRegex = new Regex(@"^[a-zA-Z]+[a-zA-Z0-9]{2,}$");
            if (!usernameRegex.IsMatch(user.Username))
            {
                result.IsValid = false;
                result.ValidationErrors.Add("Username not valid!");
            }

            // For this regex check this article: http://stackoverflow.com/questions/1559751/regex-to-make-sure-that-the-string-contains-at-least-one-lower-case-char-upper.
            Regex passwordRegex = new Regex(@"^(?=[a-zA-Z0-9]*[A-Z])(?=[a-zA-Z0-9]*[a-z])(?=[a-zA-Z0-9]*[0-9])[a-zA-Z0-9]{6,}$");
            if (!passwordRegex.IsMatch(user.Password))
            {
                result.IsValid = false;
                result.ValidationErrors.Add("Password not valid!");
            }

            // Regex explanation:
            // ^ - string should start with
            // [a-zA-Z0-9]+[-|_|\.]? - any alphanumeric group (longer than 1 symbol) followed by "-" or "_" or "." exactly one time. Will match: "Dash1-"
            // ({upperPartHere})* - the upper match may happen zero or more times. Will match: "Dash1-Dot1-Hyphen1-"
            // [a-zA-Z0-9]+ - after upper match should follow at least one alphanumeric character. Will match: "Dash1-Dot1-Hyphen1".
            // The rest of the regex follows the same logic.
            Regex emailRegex = new Regex(@"^([a-zA-Z0-9]+[-|_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[-]?)*[a-zA-Z0-9]+\.([a-zA-Z0-9]+[-]?)*[a-zA-Z0-9]+$");
            if (!emailRegex.IsMatch(user.Email))
            {
                result.IsValid = false;
                result.ValidationErrors.Add("Email not valid!");
            }

            return result;
        }
    }
}
