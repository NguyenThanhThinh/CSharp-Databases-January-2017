namespace WeddingsPlanner.Export
{
    class Export
    {
        static void Main(string[] args)
        {
            JsonMethods.ExportOrderedAgencies();
            JsonMethods.ExportGuestList();

            XmlMethods.ExportVenuesInSofia();
            XmlMethods.ExportAgenciesByTown();
        }
    }
}
