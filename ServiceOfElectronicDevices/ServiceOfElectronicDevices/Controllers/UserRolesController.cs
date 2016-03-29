using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using ServiceOfElectronicDevices.Models;
using Web;
using Web.Models;

namespace ServiceOfElectronicDevices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly OrderService orderService;
        private readonly UserRolesService userRolesService;
        private ApplicationUserManager _userManager;


        public UserRolesController(OrderService orderService, UserRolesService userRolesService)
        {
            this.orderService = orderService;
            this.userRolesService = userRolesService;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public UserRolesController() : this(new OrderService(), new UserRolesService())
        {
        }

        public ActionResult ManageUserRoles()
        {
            var model = new UserRolesViewModel
            {
                Users = orderService.GetClientsList().Select(user => new SelectListItem { Text = user.UserName, Value = user.Id }),
                Roles = userRolesService.GetRoleList().Select(role => new SelectListItem {Text = role.Name, Value = role.Name})
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddRoleToUser(UserRolesViewModel model)
        {
            if (model.Role == "SuperAdmin")
                return RedirectToAction("AddSuperAdmin", "UserRoles", model);

            await UserManager.AddToRoleAsync(model.UserId, model.Role);
            return RedirectToAction("ManageUserRoles", "UserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult AddRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            roleManager.Create(new IdentityRole(roleName));
            return RedirectToAction("ManageUserRoles");
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> AddSuperAdmin(UserRolesViewModel model)
        {
            await UserManager.AddToRoleAsync(model.UserId, model.Role);
            return RedirectToAction("ManageUserRoles", "UserRoles");
        }
    }
}