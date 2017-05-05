namespace HardwareShop.Services.Contracts
{
    using System.Collections.Generic;

    public interface IRoleService
    {
        IList<string> GetRoles();
    }
}