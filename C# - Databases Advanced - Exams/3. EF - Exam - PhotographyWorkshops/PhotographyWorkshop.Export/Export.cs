namespace PhotographyWorkshop.Export
{
    internal class Export
    {
        private static void Main(string[] args)
        {
            Json.ExportOrderedPhotograpers();
            Json.ExportLandscapePhotographers();

            Xml.ExportPhotographersWithSameCameraMake();
            Xml.ExportWorkshopsByLocation();
        }
    }
}