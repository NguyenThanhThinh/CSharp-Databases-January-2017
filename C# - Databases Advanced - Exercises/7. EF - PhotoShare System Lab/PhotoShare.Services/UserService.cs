namespace PhotoShare.Services
{
    using Data;
    using Models;
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;

    public class UserService
    {
        public void Add(string username, string password, string email)
        {
            User user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false,
                RegisteredOn = DateTime.Now,
                LastTimeLoggedIn = DateTime.Now
            };

            using (PhotoShareContext context = new PhotoShareContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public bool IsUserExisting(string username)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Users.Any(u => u.Username == username);
            }
        }

        public bool IsUserExisting(string username, string password)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Users.Any(u => u.Username == username && u.Password == password);

            }
        }

        public User GetUserByUsername(string username)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Users.SingleOrDefault(u => u.Username == username);   
            }
        }

        public void MakeFriends(string username, string username2)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                User user = context.Users.SingleOrDefault(u => u.Username == username);
                User user2 = context.Users.SingleOrDefault(u => u.Username == username2);

                if (user != null && user2 != null)
                {
                    user.Friends.Add(user2);
                    user2.Friends.Add(user);

                    context.SaveChanges();
                }
            }
        }

        public bool AreFriends(string username, string username2)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                return context.Users.SingleOrDefault(u => u.Username == username).Friends.Any(u => u.Username == username2);
            }
        }

        public List<string> GetUserFriends(string username)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                User user = context.Users.SingleOrDefault(u => u.Username == username);

                if (user == null)
                {
                    return new List<string>();
                }

                List<string> friends = user.Friends.Select(f => f.Username).OrderBy(f => f).ToList();

                return friends;
            }
        }

        public void Delete(string username)
        {
            using (PhotoShareContext context = new PhotoShareContext())
            {
                User user = context.Users.SingleOrDefault(u => u.Username == username);

                if (user.IsDeleted != null && user.IsDeleted.Value)
                {
                    throw new InvalidOperationException($"User {username} was already deleated!");
                }

                user.IsDeleted = true;
                context.SaveChanges();
            }
        }

        public void UpdateUser(User updatedUser)
        {
            // Solution with specifically FK properties in the User model for the virtual Town properties;

            using (PhotoShareContext context = new PhotoShareContext())
            {
                context.Users.Attach(updatedUser);
                context.Entry(updatedUser).State = EntityState.Modified;
                context.SaveChanges();
            }
            
            // Solution with disabled lazy loading and no FK properties in the User model for the virtual Town properties;

            //using (PhotoShareContext context = new PhotoShareContext())
            //{
            //    User user = context.Users
            //        .Include(u => u.BornTown)
            //        .Include(u => u.CurrentTown)
            //        .SingleOrDefault(u => u.Id == updatedUser.Id);

            //    if (user != null)
            //    {
            //        if (user.Password != updatedUser.Password)
            //        {
            //            user.Password = updatedUser.Password;
            //        }

            //        if (updatedUser.BornTown != null && 
            //           (user.BornTown == null || user.BornTown.Id != updatedUser.BornTown.Id))
            //        {
            //            user.BornTown = context.Towns.Find(updatedUser.BornTown.Id);
            //        }

            //        if (updatedUser.CurrentTown != null &&
            //           (user.CurrentTown == null || user.CurrentTown.Id != updatedUser.CurrentTown.Id))
            //        {
            //            user.CurrentTown = context.Towns.Find(updatedUser.CurrentTown.Id);
            //        }

            //        context.SaveChanges();
            //    }
            //}
        }
    }
}
