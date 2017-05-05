namespace Gringotts
{
    using System;
    using System.Data.Entity.Validation;
    using Models;

    class StartUp
    {
        static void Main(string[] args)
        {
            //The migrations were not needed for this exercises. I just tried different things with them;

            var context = new GringottsContext();

            WizzardDeposit dumbledoreDeposit = new WizzardDeposit()
            {
                FirstName = "Albus",
                LastName = "Dubledore",
                Age = 15,
                MagicWandCreator = "Antioch Peverell",
                MagicWandSize = 15,
                DepositStartDate = new DateTime(2017, 03, 01),
                DepositExpirationDate = new DateTime(2020, 03, 01),
                DepositAmount = 20000.24m,
                DepositCharge = 0.2,
                IsDepositExpired = false
            };

            context.WizzardDeposits.Add(dumbledoreDeposit);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var ev in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        ev.Entry.Entity.GetType().Name, ev.Entry.State);
                    foreach (var ve in ev.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
