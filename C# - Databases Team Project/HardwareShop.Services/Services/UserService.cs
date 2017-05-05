namespace HardwareShop.Services.Services
{
    using AutoMapper;
    using Contracts;
    using Data;
    using Models.EntityModels;
    using Models.ViewModels.Users;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using Microsoft.AspNet.Identity;
    using PagedList;
    using Models.ViewModels.Search;

    public class UserService : IUserService
    {
        public IEnumerable<UserViewModel> GetUsersForList(int? page, SearchViewModel searchModel)
        {
            using (var context = new HardwareShopContext())
            {
                var users = new List<ApplicationUser>();

                if (searchModel != null && searchModel.SearchString != null)
                {
                    searchModel.SearchString = searchModel.SearchString.ToLower();
                    switch (searchModel.SearchType)
                    {
                        case "username":
                            users = context.Users
                                .Where(u => u.UserName.Contains(searchModel.SearchString)).ToList();
                            break;
                        case "lastname":
                            users = context.Users
                                .Where(u => u.LastName.Contains(searchModel.SearchString))
                                .ToList();
                            break;
                        case "email":
                            users = context.Users
                                .Where(u => u.Email.Contains(searchModel.SearchString))
                                .ToList();
                            break;
                    }
                }
                else
                {
                    users = context.Users.ToList();
                }

                var usersVm = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(users);
                usersVm = usersVm.ToPagedList(page ?? 1, 3);

                return usersVm;
            }
        }

        public ApplicationUser GetUser(string userId)
        {
            using (var context = new HardwareShopContext())
            {
                return this.GetUserById(userId, context);
            }
        }

        public EditUserViewModel GetUserForEdit(string userId)
        {
            using (var context = new HardwareShopContext())
            {
                var user = this.GetUserById(userId, context);
                var model = Mapper.Map<EditUserViewModel>(user);
                return model;
            }
        }

        public DeleteUserViewModel GetUserForDelete(string userId)
        {
            using (var context = new HardwareShopContext())
            {
                var user = this.GetUserById(userId, context);

                if (user.LockoutEnabled)
                {
                    return null;
                }

                var model = Mapper.Map<DeleteUserViewModel>(user);
                return model;
            }
        }

        private ApplicationUser GetAllUserInfo(string userId, HardwareShopContext context)
        {
            return context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Reviews)
                .Include(u => u.Comments)
                .FirstOrDefault();
        }

        public void EditUser(ApplicationUser user, EditUserViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                context.Users.Attach(user);

                if (!string.IsNullOrEmpty(model.Password))
                {
                    var hasher = new PasswordHasher();
                    var passwordHash = hasher.HashPassword(model.Password);
                    user.PasswordHash = passwordHash;
                }

                user.UserName = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.Email = model.Email;

                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private ApplicationUser GetUserByUsername(string username, HardwareShopContext context)
        {
            return context.Users.FirstOrDefault(u => u.UserName == username);
        }

        private ApplicationUser GetUserById(string userId, HardwareShopContext context)
        {
            return context.Users.FirstOrDefault(u => u.Id == userId);
        }
    }
}
