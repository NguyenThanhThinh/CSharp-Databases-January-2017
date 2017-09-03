namespace PhotographyWorkshop.ImportJson
{
    internal class Import
    {
        private static void Main(string[] args)
        {
            Json.ImportLenses();
            Json.ImportCameras();
            Json.ImportPhotographers();

            XML.ImportAccessories();
            XML.ImportWorkshops();
        }
    }
}