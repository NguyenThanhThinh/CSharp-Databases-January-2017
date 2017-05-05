namespace Photography
{
    using System.Data.Entity;
    using Models;
    using Migrations;

    public class PhotographyContext : DbContext
    {
        public PhotographyContext()
            : base("name=PhotographyContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotographyContext, Configuration>());
        }

        public virtual DbSet<Photographer> Photographers { get; set; }

        public virtual DbSet<Album> Albums { get; set; }

        public virtual DbSet<Picture> Pictures { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<PhotographerAlbum> PhotographerAlbums { get; set; }
    }
}