namespace PhotographyWorkshop.Export
{
    using Data;
    using Utilities;
    using System.IO;
    using ExportDTOs;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using PhotographyWorkshops.Models;

    public class Json
    {
        private static void ExportJsonToFolder<TEntity>(string path, TEntity entityType)
        {
            string json = JsonConvert.SerializeObject(entityType, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static void ExportOrderedPhotograpers()
        {
            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                List<OrderedPhotographerDto> photographers = context.Photographers
                    .OrderBy(photographer => photographer.FirstName)
                    .ThenByDescending(photographer => photographer.LastName)
                    .Select(photographer => new OrderedPhotographerDto
                    {
                        FirstName = photographer.FirstName,
                        LastName = photographer.LastName,
                        Phone = (photographer.Phone == "" ? "null" : photographer.Phone)
                    })
                    .ToList();

                ExportJsonToFolder(Constants.ExportOrderedPhotographers, photographers);
            }
        }

        public static void ExportLandscapePhotographers()
        {
            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                List<LandscapePhotographerDto> photographers = context.Photographers
                    .Where(photographer => photographer.PrimaryCamera is CameraDslr &&
                                           photographer.Lenses.Count > 0 &&
                                           photographer.Lenses.All(lens => lens.FocalLength <= 30))
                    .OrderBy(photographer => photographer.FirstName)
                    .Select(photographer => new LandscapePhotographerDto
                    {
                        FirstName = photographer.FirstName,
                        LastName = photographer.LastName,
                        CameraMake = photographer.PrimaryCamera.Make,
                        LensesCount = photographer.Lenses.Count
                    })
                    .ToList();

                ExportJsonToFolder(Constants.ExportLandscapePhotographers, photographers);
            }
        }
    }
}
