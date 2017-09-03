namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using Services;
    using System;
    using System.Linq;
    using Utilities;

    public class CreateAlbumCommand
    {
        private AlbumService albumService;
        private UserService userService;
        private TagService tagService;

        public CreateAlbumCommand(AlbumService albumService, UserService userService, TagService tagService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.tagService = tagService;
        }

        // CreateAlbum <username> <albumName> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(string[] data)
        {
            if (!SecurityService.IsAuthenticated())
            {
                throw new InvalidOperationException("Login in order to create albums!");
            }

            string username = data[0];
            string albumName = data[1];
            string backgroundColor = data[2];
            string[] tags = data.Skip(3).Select(t => t.ValidateOrTransform()).ToArray();
            //string[] tags = data.Skip(3).Select(t => TagUtilities.ValidateOrTransform(t)).ToArray();

            if (!this.userService.IsUserExisting(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }

            Color color;
            bool isColorValid = Enum.TryParse(backgroundColor, out color);

            if (!isColorValid)
            {
                throw new ArgumentException($"Color {backgroundColor} not found!");
            }

            if (tags.Any(t => !this.tagService.IsTagExisting(t)))
            {
                throw new ArgumentException($"Invaid tags!");
            }

            if (this.albumService.IsAlbumExisting(albumName))
            {
                throw new ArgumentException($"Album {albumName} already exists!");
            }

            this.albumService.AddAlbum(albumName, username, color, tags);

            return $"Album {albumName} successfully created!";
        }
    }
}