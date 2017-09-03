namespace PhotoShare.Services
{
    using Data;
    using Models;
    using System.Data.Entity;
    using System.Linq;

    public class AlbumService
    {
        public void AddAlbum(string albumName, string ownerUsername, Color color, string[] tagNames)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                Album album = new Album();
                album.Name = albumName;
                album.BackgroundColor = color;
                album.Tags = context.Tags.Where(t => tagNames.Contains(t.Name)).ToArray();

                User owner = context.Users.SingleOrDefault(u => u.Username == ownerUsername);

                AlbumRole albumRole = new AlbumRole
                {
                    Album = album,
                    User = owner,
                    Role = Role.Owner
                };

                album.AlbumRoles.Add(albumRole);

                context.Albums.Add(album);
                context.SaveChanges();
            }
        }

        public bool IsAlbumExisting(string albumName)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Albums.Any(a => a.Name == albumName);
            }
        }

        public bool IsAlbumExisting(int albumId)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Albums.Any(a => a.Id == albumId);
            }
        }

        public bool HasAlbumTag(string tag)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Albums.Any(a => a.Tags.Any(t => t.Name == tag));
            }
        }

        public Album GetAlbumById(int albumId)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Albums.SingleOrDefault(alb => alb.Id == albumId);
            }
        }

        public bool IsUserOwnerOfAlbum(string username, string albumName)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                Album album = context.Albums
                      .Include(a => a.AlbumRoles)
                      .Include(a => a.AlbumRoles.Select(ab => ab.User))
                      .SingleOrDefault(a => a.Name == albumName);

                if (album == null)
                {
                    return false;
                }

                return album.AlbumRoles.Any(ar => ar.User.Username == username && ar.Role == Role.Owner);
            }
        }

        public bool IsRoleExistingForUser(string username, int albumId)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.AlbumRoles.Any(ar => ar.User.Username == username && ar.Album.Id == albumId);
            }
        }

        public void ShareAlbum(int albumId, string username, Role role)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                Album album = context.Albums.SingleOrDefault(a => a.Id == albumId);
                User user = context.Users.SingleOrDefault(u => u.Username == username);

                if (user != null && album != null)
                {
                    AlbumRole albumRole = new AlbumRole
                    {
                        Album = album,
                        User = user,
                        Role = role
                    };

                    album.AlbumRoles.Add(albumRole);
                    context.SaveChanges();
                }
            }
        }
    }
}