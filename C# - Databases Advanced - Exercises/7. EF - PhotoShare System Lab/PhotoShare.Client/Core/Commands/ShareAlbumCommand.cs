namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using System;
    using Services;
 
    public class ShareAlbumCommand
    {
        private AlbumService albumService;
        private UserService userService;

        public ShareAlbumCommand(AlbumService albumService, UserService userService)
        {
            this.albumService = albumService;
            this.userService = userService;
        }

        // ShareAlbum <albumId> <username> <permission>
        public string Execute(string[] data)
        {
            int albumId = int.Parse(data[0]);
            string username = data[1];
            string permission = data[2];

            if (!SecurityService.IsAuthenticated())
            {
                throw new InvalidOperationException("Login in order to share albums!");
            }

            if (!this.albumService.IsAlbumExisting(albumId))
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }

            Album album = this.albumService.GetAlbumById(albumId);
            if (!this.userService.IsUserExisting(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (!this.albumService.IsUserOwnerOfAlbum(SecurityService.GetCurrentUser().Username, album.Name))
            {
                throw new ArgumentException($"You can share album only if you are 'owner' of that album!");
            }

            Role role;
            bool isRoleValid = Enum.TryParse(permission, out role);

            if (!isRoleValid)
            {
                throw new ArgumentException($"Permission must be either \"Owner\" or \"Viewer\"!");
            }

            if (this.albumService.IsRoleExistingForUser(username, albumId))
            {
                throw new ArgumentException($"User {username} already has role in album {album.Name}!");
            }

            this.albumService.ShareAlbum(albumId, username, role);

            return $"Username {username} added to album {album.Name} ({permission})";
        }
    }
}

