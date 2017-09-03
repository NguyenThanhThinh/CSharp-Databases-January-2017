namespace HardwareShop.Web.Controllers.Admin
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Models.EntityModels;
    using Models.ViewModels.Role;
    using Models.ViewModels.Search;
    using Models.ViewModels.Users;
    using PagedList;
    using Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private IUserService userService;
        private IRoleService roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }

        [HttpGet]
        public ActionResult List(ListUsersViewModel model, string searchString, string searchType, int? page)
        {
            var searchVm = new SearchViewModel();

            if (searchString != null)
            {
                searchVm.SearchString = searchString;
                searchVm.SearchType = searchType;
            }

            model.SearchViewModel = model.SearchViewModel ?? searchVm;
            model.Users = (IPagedList<UserViewModel>)this.userService.GetUsersForList(page, model.SearchViewModel);
            var roles = this.roleService.GetRoles();

            foreach (var user in model.Users)
            {
                user.RoleName = GetUserRoles(user.Id, roles)
                    .Where(r => r.IsSelected == true)
                    .OrderBy(r => r.Name)
                    .FirstOrDefault().Name;
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Edit(string userId, int? page)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = userService.GetUserForEdit(userId);

            if (model == null)
            {
                return this.HttpNotFound();
            }

            var roles = this.roleService.GetRoles();
            model.Roles = this.GetUserRoles(userId, roles);

            this.ViewBag.Page = page;

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel model, int? page)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewBag.Page = page;
                return this.View(model);
            }

            var user = this.userService.GetUser(model.Id);

            this.SetUserRoles(model.Roles, user);
            this.userService.EditUser(user, model);

            return this.RedirectToAction("List", new { @page = page });
        }

        [HttpGet]
        public ActionResult Delete(string userId, int? page)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.userService.GetUserForDelete(userId);

            if (model == null)
            {
                return this.HttpNotFound();
            }

            this.ViewBag.Page = page;

            return this.View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(string Id, int? page)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            userManager.SetLockoutEnabled(Id, true);
            userManager.SetLockoutEndDate(Id, DateTime.Now.AddYears(30));

            return this.RedirectToAction("List", new { @page = page });
        }

        [HttpPost]
        public ActionResult Restore(string userId, int? page)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            userManager.SetLockoutEnabled(userId, false);

            return this.RedirectToAction("List", new { @page = page });
        }

        private void SetUserRoles(IList<RoleViewModel> roles, ApplicationUser user)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (var role in roles)
            {
                if (role.IsSelected && !userManager.IsInRole(user.Id, role.Name))
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if (!role.IsSelected && userManager.IsInRole(user.Id, role.Name))
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }

        private IList<RoleViewModel> GetUserRoles(string userId, IList<string> roles)
        {
            var userManager = this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userRoles = new List<RoleViewModel>();

            foreach (var role in roles)
            {
                var userRole = new RoleViewModel { Name = role };

                if (userManager.IsInRole(userId, role))
                {
                    userRole.IsSelected = true;
                }

                userRoles.Add(userRole);
            }

            return userRoles;
        }
    }
}