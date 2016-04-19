using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Models;
using BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using ServiceOfElectronicDevices.Models;

namespace ServiceOfElectronicDevices.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly OrderService orderService;

        public OrderController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        public OrderController() : this( new OrderService())
        {
        }

        // GET: Order
        public ActionResult Index()
        {
            if (User.IsInRole("Employee"))
                return RedirectToAction("AdminIndex");

            var model = orderService.GetUserOrders(User.Identity.GetUserId());
            return View(model);
        }

        [Authorize(Roles = "Employee")]
        public ActionResult AdminIndex()
        {
            var model = orderService.GetOrderList();
            return View("Index", model);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public ActionResult AddOrder()
        {
            var model = new AddOrderViewModel
            {
                Devices = orderService.GetDevicesList().Select(device => new SelectListItem {Text = $"{device.Brand} {device.Model}", Value = device.Id.ToString()}),
                Users = orderService.GetClientsList().Select(user => new SelectListItem {Text = user.UserName, Value = user.Id})
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public ActionResult AddOrder(AddOrderViewModel model)
        {
            orderService.AddOrder(model.UserId, model.DeviceId, model.Description);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public ActionResult AddDevice()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public ActionResult AddDevice(DevicesDto deviceDto)
        {
            if (!ModelState.IsValid)
            {
                return View(deviceDto);
            }
            orderService.AddDevice(deviceDto);
            return RedirectToAction("AddOrder");
        }

        public ActionResult OrderDetails(int id)
        {
            if (!User.IsInRole("Admin") && !orderService.AuthorizeOrderOwner(User.Identity.GetUserId(), id))
                return RedirectToAction("Index");

            var model = orderService.GetOrderDetails(id);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public ActionResult NewTask(int id)
        {
            var model = orderService.GetLastTask(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public ActionResult NewTask(TaskProgressDto task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }
            orderService.AddTask(task);
            return RedirectToAction("OrderDetails", new {id = task.OrderId});
        }
    }
}