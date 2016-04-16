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
            var model = orderService.GetUserOrders(User.Identity.GetUserId());
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddOrder()
        {
            var model = new AddOrderViewModel
            {
                Devices = orderService.GetDevicesList().Select(device => new SelectListItem {Text = $"{device.Brand} {device.Model}", Value = device.Brand}),
                Users = orderService.GetClientsList().Select(user => new SelectListItem {Text = user.UserName, Value = user.Id})
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddOrder(AddOrderViewModel model)
        {
            orderService.AddOrder(model.UserId, model.DeviceId, model.Description);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddDevice()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddDevice(DevicesDto deviceDto)
        {
            orderService.AddDevice(deviceDto);
            return RedirectToAction("AddOrder");
        }

        public ActionResult OrderDetails(int id)
        {
            var model = orderService.GetOrderDetails(id);

            return View(model);
        }
    }
}