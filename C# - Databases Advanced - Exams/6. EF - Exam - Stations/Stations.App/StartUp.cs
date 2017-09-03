namespace Stations.App
{
    using Data;
    using Export;
    using Import;
    using System.IO;

    internal class StartUp
    {
        private static void Main()
        {
            StationsContext context = new StationsContext();
            HelperMethods helperMethods = new HelperMethods(context);

            Import import = new Import(helperMethods);

            import.ImportStations(File.ReadAllText("../../../datasets/stations.json"));
            import.ImportClasses(File.ReadAllText("../../../datasets/classes.json"));
            import.ImportTrains(File.ReadAllText("../../../datasets/trains.json"));
            import.ImportTrips(File.ReadAllText("../../../datasets/trips.json"));

            import.ImportCards(File.ReadAllText("../../../datasets/cards.xml"));
            import.ImportTickets(File.ReadAllText("../../../datasets/tickets.xml"));

            Export export = new Export(helperMethods);

            export.ExportDelayedTrains("../../../datasets/", "01/01/2017");
            export.ExportCardsTicket("../../../datasets/", "Debilitated");
        }
    }
}