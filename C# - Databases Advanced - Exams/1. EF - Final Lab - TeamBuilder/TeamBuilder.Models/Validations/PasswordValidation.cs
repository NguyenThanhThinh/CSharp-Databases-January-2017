namespace TeamBuilder.Models.Validations
{
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    internal class PasswordAttribute : ValidationAttribute
    {
        public bool ContainsUppercase { get; set; }

        public bool ContainsDigit { get; set; }

        public override bool IsValid(object value)
        {
            string password = value.ToString();
            if (this.ContainsUppercase && !password.Any(char.IsUpper))
            {
                return false;
            }

            if (this.ContainsDigit && !password.Any(char.IsDigit))
            {
                return false;
            }
            
            return true;
        }
    }
}
