namespace Gringotts.Client
{
    using Data;
    using System;
    using System.Linq;

    internal class GringottsMain
    {
        private static void Main(string[] args)
        {
            GringottsContext context = new GringottsContext();

            // Task 19
            //DepositsSumForOllivanderFamily(context);

            // Task 20
            //DepositFilter(context);
        }

        private static void DepositsSumForOllivanderFamily(GringottsContext context)
        {
            var depositGroups = context.WizzardDeposits
                .Where(w => w.MagicWandCreator == "Ollivander family")
                .GroupBy(w => w.DepositGroup)
                .Select(group => new
                {
                    DepositGroupName = group.Key,
                    TotalDepositSum = group.Sum(deposit => deposit.DepositAmount)
                });

            foreach (var depositGroup in depositGroups)
            {
                Console.WriteLine($"{depositGroup.DepositGroupName} - {depositGroup.TotalDepositSum}");
            }
        }

        private static void DepositFilter(GringottsContext context)
        {
            var depositGroups = context.WizzardDeposits
                .Where(w => w.MagicWandCreator == "Ollivander family")
                .GroupBy(w => w.DepositGroup)
                .Select(group => new
                {
                    DepositGroupName = group.Key,
                    TotalDepositSum = group.Sum(deposit => deposit.DepositAmount)
                })
                .Where(group => group.TotalDepositSum < 150000)
                .OrderByDescending(arg => arg.TotalDepositSum);

            foreach (var depositGroup in depositGroups)
            {
                Console.WriteLine($"{depositGroup.DepositGroupName} - {depositGroup.TotalDepositSum}");
            }
        }
    }
}