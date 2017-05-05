namespace WeddingsPlanner.Import
{
    class Import
    {
        static void Main(string[] args)
        {
            JsonMethods.ImportAgencies();
            JsonMethods.ImportPeople();
            JsonMethods.ImportWeddings();

            XmlMethods.ImportVenues();
            XmlMethods.ImportPresents();
        }
    }
}
