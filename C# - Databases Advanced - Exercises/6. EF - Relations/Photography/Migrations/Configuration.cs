namespace Photography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Collections.Generic;
    using PhotographersDB;
    using Models;
 
    internal sealed class Configuration : DbMigrationsConfiguration<PhotographyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Photography.PhotographyContext";
        }

        protected override void Seed(PhotographyContext context)
        {
            Photographer dragan = new Photographer()
            {
                Username = "Dragan",
                Password = "123456",
                Email = "dragan@abv.bg",
                BirthDate = new DateTime(1992, 01, 08),
                RegisterDate = new DateTime(2010, 10, 10)
            };

            Photographer stamen = new Photographer()
            {
                Username = "Stamen",
                Password = "123456",
                Email = "stamen@abv.bg",
                BirthDate = new DateTime(1992, 01, 08),
                RegisterDate = new DateTime(2010, 10, 10)
            };

            context.Photographers.AddOrUpdate(p => p.Username, dragan, stamen);
            context.SaveChanges();



            Picture picture = new Picture()
            {
                Title = "Mountain",
                Caption = "Peak",
                PathOnFileSystem = "../pictures/mountains"
            };

            Picture picture2 = new Picture()
            {
                Title = "River",
                Caption = "Water",
                PathOnFileSystem = "../pictures/rivers"
            };

            context.Pictures.AddOrUpdate(pic => pic.Title, picture, picture2);
            context.SaveChanges();



            Tag tag = new Tag()
            {
                Name = "rocks"
                // Tag name doesn't start with '#' but it's transformed to valid tag with the 'TagTansformer' class;
            };

            Tag tag2 = new Tag()
            {
                Name = "#   fishes"
                //Tag name contains white spaces but it's transformed to valid tag with the 'TagTansformer' class;
            };

            List<Tag> tags = new List<Tag>();
            tags.Add(tag);
            tags.Add(tag2);

            foreach (var currentTag in tags)
            {
                currentTag.Name = TagTansformer.Transform(currentTag.Name);
            }

            foreach (var currentTag in tags)
            {
                context.Tags.AddOrUpdate(t => t.Name, currentTag);
            }

            context.SaveChanges();



            Album album = new Album()
            {
                Name = "Mountains",
                BackgroundColor = "Green and Brown",
                IsPublic = true
            };

            Album album2 = new Album()
            {
                Name = "Rivers",
                BackgroundColor = "Green and Blue",
                IsPublic = true
            };

            context.Albums.AddOrUpdate(a => a.Name, album, album2);       

            context.SaveChanges();



            PhotographerAlbum photographerAlbumDragan = new PhotographerAlbum()
            {
                Album_Id = album.Id,
                Photographer_Id = dragan.Id,
                Role = Role.Owner
            };

            PhotographerAlbum photographerAlbumStamen = new PhotographerAlbum()
            {
                Album_Id = album2.Id,
                Photographer_Id = stamen.Id,
                Role = Role.Owner
            };

            PhotographerAlbum photographerAlbumDragan2 = new PhotographerAlbum()
            {
                Album_Id = album2.Id,
                Photographer_Id = dragan.Id,
                Role = Role.Viewer
            };

            PhotographerAlbum photographerAlbumStamen2 = new PhotographerAlbum()
            {
                Album_Id = album.Id,
                Photographer_Id = stamen.Id,
                Role = Role.Viewer
            };

            // After executing the Seed method:
            // In the first album Dragan is an owner and Stamen is a viewer;
            // In the second album Stamen is an owner and Dragan is a viewer;
            context.PhotographerAlbums.AddOrUpdate(pa => new { pa.Photographer_Id, pa.Album_Id}, 
                                                                photographerAlbumDragan, photographerAlbumStamen, 
                                                                photographerAlbumDragan2, photographerAlbumStamen2);


            // Album-Pictures many-to-many relationship it's working - table [dbo].[PictureAlbums] it's filling with data;
            album.Pictures.Add(picture);
            album2.Pictures.Add(picture2);

            // Album-Tags many-to-many relationship it's working - table [dbo].[TagAlbums] it's filling with data;
            album.Tags.Add(tag);
            album2.Tags.Add(tag2);

            context.SaveChanges();
        }
    }
}
