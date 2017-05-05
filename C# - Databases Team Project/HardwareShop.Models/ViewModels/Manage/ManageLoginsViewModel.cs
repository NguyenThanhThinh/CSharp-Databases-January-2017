﻿namespace HardwareShop.Models.ViewModels.Manage
{
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using System.Collections.Generic;

    public class ManageLoginsViewModel : ViewModelBase
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
}