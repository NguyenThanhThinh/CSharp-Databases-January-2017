namespace WeddingsPlanner.Export
{
    internal class Export
    {
        private static void Main(string[] args)
        {
            JsonMethods.ExportOrderedAgencies();
            JsonMethods.ExportGuestList();

            XmlMethods.ExportVenuesInSofia();
            XmlMethods.ExportAgenciesByTown();
        }
    }
}