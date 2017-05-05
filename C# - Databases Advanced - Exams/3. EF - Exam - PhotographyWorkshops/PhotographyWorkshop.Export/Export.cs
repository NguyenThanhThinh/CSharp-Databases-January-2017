namespace PhotographyWorkshop.Export
{
    class Export
    {
        static void Main(string[] args)
        {
            Json.ExportOrderedPhotograpers();
            Json.ExportLandscapePhotographers();

            Xml.ExportPhotographersWithSameCameraMake();
            Xml.ExportWorkshopsByLocation();
        }
    }
}
