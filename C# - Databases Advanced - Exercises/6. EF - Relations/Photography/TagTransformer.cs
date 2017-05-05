namespace PhotographersDB
{
    public class TagTansformer
    {
        public static string Transform(string tagString)
        {
            if (!tagString.StartsWith("#"))
            {
                tagString = "#" + tagString;
            }

            if (tagString.Contains(" "))
            {
                tagString = tagString.Replace(" ", string.Empty);
            }

            if (tagString.Contains("\t"))
            {
                tagString = tagString.Replace("\t", string.Empty);
            }

            if (tagString.Length > 20)
            {
                tagString = tagString.Substring(0, 20);
            }

            return tagString;
        }
    }
}