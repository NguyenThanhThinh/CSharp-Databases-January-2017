namespace MassDefect.App
{   
    using Import;
    using Export;
    using System.Data.Entity;

    class Application
    {
        static void Main(string[] args)
        {
           // Importing the data from JSON and XML files
           if (!Database.Exists("MassDefectContext"))
            {
                ImportJson.SolarSystems();
                ImportJson.Stars();
                ImportJson.Planets();
                ImportJson.Persons();
                ImportJson.Anomalies();
                ImportJson.AnomalyVictims();

                ImportXml.NewAnomalies();
            }

            // Tasks to export JSON files
            ExportJson.ExportPlanetsWhichAreNotAnomalyOrigins();
            ExportJson.ExportPeopleWhichHaveNotBeenVictims();
            ExportJson.ExportTopAnomaly();

            // Task to export XML file
            ExportXml.ExtractAllAnomaliesAndPeopleAffected();
        }

    }
}
