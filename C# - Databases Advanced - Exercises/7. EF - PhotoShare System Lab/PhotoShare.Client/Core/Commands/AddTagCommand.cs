namespace PhotoShare.Client.Core.Commands
{
    using Services;
    using System;
    using Utilities;

    public class AddTagCommand
    {
        private TagService tagService;

        public AddTagCommand(TagService tagService)
        {
            this.tagService = tagService;
        }

        // AddTag <tag>
        public string Execute(string[] data)
        {
            // With using Extention:
            string tagName = data[0].ValidateOrTransform();

            // Without using Extention:
            //string tag = TagUtilities.ValidateOrTransform(data[0]);

            if (!SecurityService.IsAuthenticated())
            {
                throw new InvalidOperationException("Login in order to add tag!");
            }

            if (this.tagService.IsTagExisting(tagName))
            {
                throw new ArgumentException($"Tag {tagName} exists!");
            }

            this.tagService.AddTag(tagName);

            return $"Tag {tagName} was added successfully!";
        }
    }
}