namespace Sales
{
    internal class StartUp
    {
        private static void Main(string[] args)
        {
            //The inserted data is in the Seed method of Migrations.Configuration;
            //The SQL queries for 'Customer's default Age = 20' are in 'AlterCustomersAddAgeDefaultValueScript.sql';

            //3. Created Sales database with needed models;
            //4. In folder Migrations - Added manual migration for new column Desription with default value in model Product;
            //5. In folder Migrations - Added manual migration to update column Date with default value in model Sale;
            //6. In folder Migrations - Added automatic migration for new columns FirstName and Lastname in model Customers;
            //   You can see that it was created and it exists in the database in table 'dbo.__MigrationHistory';
            //7. In folder Migrations - Added manual migration to update column Age with default value in model Customers;
            //8. In folder Migrations - Reverting the last migration and adding it again in order to create the migration script (the SQL queries) for
            //   that migration, located in 'AlterCustomersAddAgeDefaultValueScript.sql';
        }
    }
}