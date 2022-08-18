using RetailManager.DataManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using RetailManager.DataManager.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using RetailManager.Api.Models;

namespace RetailManager.Api.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel DetailsGetById()
        {
            string id = RequestContext.Principal.Identity.GetUserId();
            UserData userData = new UserData();
            var list = userData.GetUserById(id);
            return list.First();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            var output = new List<ApplicationUserModel>();
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    ApplicationUserModel um = new ApplicationUserModel()
                    {
                        Id = user.Id,
                        Email = user.Email,
                    };
                    foreach (var role in user.Roles)
                    {
                        um.Roles.Add(role.RoleId, roles.Where(r => r.Id == role.RoleId).Select(r => r.Name).FirstOrDefault());
                    }
                    output.Add(um);
                }
                return output;
            }
        }
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var roles = ctx.Roles.ToDictionary(x => x.Id, x => x.Name);
                return roles;
            }
        }
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public void AddRole(UserRolePairModel model)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var res = userManager.AddToRole(model.UserId, model.RoleName);
                if (res.Succeeded)
                {
                    //
                }
            }
        }
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public void RemoveRole(UserRolePairModel model)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var res = userManager.RemoveFromRole(model.UserId, model.RoleName);
                if (res.Succeeded)
                {
                    //
                }
            }
        }
    }
}