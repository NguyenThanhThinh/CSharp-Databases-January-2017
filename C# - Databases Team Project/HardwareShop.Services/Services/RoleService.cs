namespace HardwareShop.Services.Services
{
    using Contracts;
    using Data;
    using System.Collections.Generic;
    using System.Linq;

    public class RoleService : IRoleService
    {
        public IList<string> GetRoles()
        {
            using (var context = new HardwareShopContext())
            {
                return context.Roles.Select(r => r.Name).OrderBy(r => r).ToList();
            }
        }
    }
}