namespace PhotoShare.Services
{
    using Data;
    using Models;
    using System.Linq;

    public class TownService
    {
        public void Add(string townName, string countryName)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                Town town = new Town();
                town.Name = townName;
                town.Country = countryName;

                context.Towns.Add(town);
                context.SaveChanges();
            }
        }

        public bool IsTownExisting(string townName)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Towns.Any(t => t.Name == townName);
            }
        }

        public Town GetTownByTownName(string townName)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Towns.SingleOrDefault(t => t.Name == townName);
            }
        }
    }
}