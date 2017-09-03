namespace PhotoShare.Services
{
    using Data;
    using Models;
    using System.Linq;

    public class PictureService
    {
        public void AddPicture(string albumName, string pictureTitle, string pictureFilePath)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                Album album = context.Albums.SingleOrDefault(alb => alb.Name == albumName);

                if (album != null)
                {
                    Picture picture = new Picture();
                    picture.Title = pictureTitle;
                    picture.Path = pictureFilePath;

                    bool isPictureExisting = context.Pictures.Any(p => p.Title == pictureTitle && p.Path == pictureFilePath);
                    if (!isPictureExisting)
                    {
                        context.Pictures.Add(picture);
                    }

                    album.Pictures.Add(picture);
                    context.SaveChanges();
                }
            }
        }

        public bool IsPictureExisting(string albumName, string pictureTitle, string pictureFilePath)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Albums.SingleOrDefault(a => a.Name == albumName).Pictures.Any(p => p.Title == pictureTitle && p.Path == pictureFilePath);
            }
        }
    }
}