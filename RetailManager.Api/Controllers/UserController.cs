using RetailManager.DataManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using RetailManager.DataManager.Models;

namespace RetailManager.Api.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public List<UserModel> DetailsGetById()
        {
            string id = RequestContext.Principal.Identity.GetUserId();
            UserData userData = new UserData();
            var list = userData.GetUserById(id);
            return list;
        }
    }
}