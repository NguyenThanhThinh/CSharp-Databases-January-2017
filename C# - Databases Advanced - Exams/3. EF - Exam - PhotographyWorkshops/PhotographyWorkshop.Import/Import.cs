namespace PhotographyWorkshop.ImportJson
{
    class Import
    {
        static void Main(string[] args)
        {
            Json.ImportLenses();
            Json.ImportCameras();
            Json.ImportPhotographers();

            XML.ImportAccessories();
            XML.ImportWorkshops();
        }
    }
}
