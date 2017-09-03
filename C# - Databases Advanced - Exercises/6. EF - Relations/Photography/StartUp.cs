namespace Photography
{
    using System;
    using System.Linq;

    internal class StartUp
    {
        private static void Main(string[] args)
        {
            // All the sample data passes without errors through the Seed method in Migration.Configuration:
            // You can drop the database and create it again by executing the StartUp method to check;

            // All the many-to-many tables also are working - the data in them it's filling automatically;

            // Task 5.	Photographers model added with migration: InitiaCreate;
            // Task 6.	Albums and Pictures models added with migration: AddedAlbumAndPictureModels;
            // Task 7.	Tags model added with migration: AddedTagModel;
            // Task 8.	TagAttribute validation added to Tag model - created in folder Attributes;
            //          TagTransformer class added to check and transform invalid tags in valid ones;
            // Task 9.	Created many-to-many relationship between Photographer and Album modls with migration: AddOwnersToAlbums;
            // Task 10. Created 'owner' or 'viewer' role for each photographer on given album with migration: AddRolesToPhotographers;

            PhotographyContext context = new PhotographyContext();
            var photographersCount = context.Photographers.Count();
            Console.WriteLine(photographersCount);
        }
    }
}