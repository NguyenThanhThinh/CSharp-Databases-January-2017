namespace PhotoShare.Services
{
    using Data;
    using Models;
    using System;
    using System.Linq;

    public class TagService
    {
        public void AddTag(string tagName)
        {
            Tag tag = new Tag();
            tag.Name = tagName;

            using (PhotoShareContext context = new PhotoShareContext())
            {
                context.Tags.Add(tag);
                context.SaveChanges();
            }
        }

        public void AddTagTo(string albumName, string tagName)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                Album album = context.Albums.SingleOrDefault(a => a.Name == albumName);
                Tag tag = context.Tags.SingleOrDefault(t => t.Name == tagName);

                if (album.Tags.Any(t => t.Name == tag.Name))
                {
                    throw new ArgumentException($"Album already has this tag!");
                }

                album.Tags.Add(tag);
                context.SaveChanges();
            }
        }

        public bool IsTagExisting(string tagName)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Tags.Any(t => t.Name == tagName);
            }
        }
    }
}