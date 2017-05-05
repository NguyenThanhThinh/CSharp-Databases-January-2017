namespace Users
{
    using System.Data.Entity;

    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("name=UsersContext")
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}