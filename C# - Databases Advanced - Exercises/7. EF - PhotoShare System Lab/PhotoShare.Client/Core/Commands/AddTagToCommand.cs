namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Services;
    using Utilities;

    public class AddTagToCommand
    {
        private TagService tagService;
        private AlbumService albumService;

        public AddTagToCommand(AlbumService albumService, TagService tagService)
        {
            this.albumService = albumService;
            this.tagService = tagService;
        }

        // AddTagTo <albumName> <tag>
        public string Execute(string[] data)
        {
            string albumName = data[0];
            string tagName = data[1].ValidateOrTransform();
            //string tagName = TagUtilities.ValidateOrTransform(data[1]);

            if (!SecurityService.IsAuthenticated())
            {
                throw new InvalidOperationException("Login in order to add tag!");
            }

            if (!this.albumService.IsAlbumExisting(albumName) || !this.tagService.IsTagExisting(tagName))
            {
                throw new ArgumentException($"Either tag or album do not exist!");
            }

            if (this.albumService.HasAlbumTag(tagName))
            {
                throw new ArgumentException("Album already has this tag!");
            }

            if (!this.albumService.IsUserOwnerOfAlbum(SecurityService.GetCurrentUser().Username, albumName))
            {
                throw new InvalidOperationException("You can add tag only to album of which you are 'owner'!");
            }

            this.tagService.AddTagTo(albumName, tagName);

            return $"Tag {tagName} added to {albumName}!";
        }
    }
}
