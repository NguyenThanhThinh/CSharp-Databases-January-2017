using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FootballBettingData.DataAnnotations
{
    class CountryIdAnnotation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string input = (string) value;
            Regex regex = new Regex(@"^[a-zA-Z]{3}$");
            if (!regex.IsMatch(input))
            {
                return false;
            }
            return true;
        }
    }
}
