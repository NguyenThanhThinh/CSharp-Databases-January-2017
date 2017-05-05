namespace HardwareShop.Services.Services
{
    using System.Linq;
    using Models.ViewModels.Manage;
    using Models.EntityModels;
    using Data;
    using AutoMapper;
    using System.Data.Entity;
    using Contracts;

    public class ManageService : IManageService
    {
        public ManageUserViewModel GetUser(string id)
        {
            using (var context = new HardwareShopContext())
            {
                var user = this.GetUserByUsername(id, context);
                var model = Mapper.Map<ManageUserViewModel>(user);

                return model;
            }
        }

        public void EditUser(ManageUserViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var user = this.GetUserByUsername(model.Id, context);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.Email = model.Email;
                
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private ApplicationUser GetUserByUsername(string id, HardwareShopContext context)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
