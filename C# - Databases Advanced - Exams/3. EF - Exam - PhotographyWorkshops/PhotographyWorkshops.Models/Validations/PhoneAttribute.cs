using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PhotographyWorkshops.Models.Validations
{
    internal class PhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string phone = value.ToString();
            Regex regex = new Regex(@"\+\d{1,3}\/\d{8,10}");
            if (!regex.IsMatch(phone))
            {
                return false;
            }

            return true;
        }
    }
}
