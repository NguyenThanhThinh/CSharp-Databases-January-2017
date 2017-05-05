namespace PhotographersDB
{
    using System.ComponentModel.DataAnnotations;

    public class TagAttribute : ValidationAttribute
    {
        public override bool IsValid(object tag)
        {
            string tagString = (string)tag;

            if (!tagString.StartsWith("#"))
            {
                return false;
            }

            if (tagString.Contains(" ") || tagString.Contains("\t"))
            {
                return false;
            }

            if (tagString.Length > 20)
            {
                return false;
            }

            return true;
        }
    }
}