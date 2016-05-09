using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Models;
using BusinessLogic.Services;
using Microsoft.AspNet.Identity.Owin;
using ServiceOfElectronicDevices.Models;
using Web;

namespace ServiceOfElectronicDevices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly OrderService orderService;
        private readonly UserRolesService userRolesService;
        private readonly ComponentsService componentsService;
        private ApplicationUserManager _userManager;


        public UserRolesController(OrderService orderService, UserRolesService userRolesService, ComponentsService componentsService)
        {
            this.orderService = orderService;
            this.userRolesService = userRolesService;
            this.componentsService = componentsService;
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


        public UserRolesController() : this(new OrderService(), new UserRolesService(), new ComponentsService())
        {
        }

        public ActionResult ManageUserRoles()
        {
            var model = new UserRolesViewModel
            {
                Users = orderService.GetClientsList().Select(user => new SelectListItem { Text = user.UserName, Value = user.Id }),
                Roles = userRolesService.GetRoleList().Select(role => new SelectListItem {Text = role.Name, Value = role.Name}),
                ComponentsTypes = componentsService.GetComponentsTypes().Select(type => new SelectListItem { Text = type.PartName, Value = type.Id.ToString()})
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRoleToUser(UserRolesViewModel model)
        {
            await UserManager.AddToRoleAsync(model.UserId, model.Role);
            return RedirectToAction("ManageUserRoles", "UserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveRoleFromUser(UserRolesViewModel model)
        {
            await UserManager.RemoveFromRoleAsync(model.UserId, model.Role);
            return RedirectToAction("ManageUserRoles", "UserRoles");
        }

        public ActionResult AddComponentType(string componentType)
        {
            componentsService.AddComponentType(componentType);
            return RedirectToAction("ManageUserRoles");
        }

        public ActionResult AddComponent(PartDto partDto)
        {
            componentsService.AddComponent(partDto);
            return RedirectToAction("ManageUserRoles");
        }
    }
}