using System;
using System.Linq;
using System.Text;

namespace IntroductionToEntityFramework
{
    internal class FirstLetter
    {
        public static void GetFirstLetter(GringottsContext gingottsContext)
        {
            StringBuilder content = new StringBuilder();

            var uniqueWizardFirstLetters =
                gingottsContext.WizzardDeposits.Where(x => x.DepositGroup == "Troll Chest")
                    .Select(x => x.FirstName.Substring(0, 1))
                    .Distinct()
                    .ToList();

            foreach (var letter in uniqueWizardFirstLetters)
            {
                content.AppendLine(letter);
            }

            Console.WriteLine(content);
        }
    }
}