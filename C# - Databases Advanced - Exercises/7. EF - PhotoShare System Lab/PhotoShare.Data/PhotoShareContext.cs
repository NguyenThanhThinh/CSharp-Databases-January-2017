namespace PhotoShare.Data
{
    using Migrations;
    using Models;
    using System.Data.Entity;

    public class PhotoShareContext : DbContext
    {
        public PhotoShareContext() : base("name=PhotoShareContext")
        {
            // If we want a solution with changing BornTown and CurrentTown of User
            // without specific FK properties for the virtual Town properties in the User model -
            // we have specifically to disable the lazy loading because otherwise the EF is giving error when trying to update;

            //this.Configuration.LazyLoadingEnabled = false;

            //Database.SetInitializer(new DropCreateDatabaseAlways<PhotoShareContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoShareContext, Configuration>());
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Album> Albums { get; set; }

        public virtual DbSet<Picture> Pictures { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<AlbumRole> AlbumRoles { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("FriendId");
                    m.ToTable("UsersFriends");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}