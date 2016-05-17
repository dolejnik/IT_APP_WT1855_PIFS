using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ComponentsService componentsService;

        public OrderController(OrderService orderService, ComponentsService componentsService)
        {
            this.orderService = orderService;
            this.componentsService = componentsService;
        }

        public OrderController() : this( new OrderService(), new ComponentsService())
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
            if (!User.IsInRole("Employee") && !orderService.AuthorizeOrderOwner(User.Identity.GetUserId(), id))
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewTask(TaskProgressDto task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            await orderService.AddTask(task);
            return RedirectToAction("OrderDetails", new { id = task.OrderId });
        }

        public ActionResult CommponentsList(TaskProgressDto model)
        {
            var model2 = new AddTaskViewModel
            {
                Id = model.Id,
                OrderId= model.OrderId,
                State = model.State,
                Description = model.Description,
                DateFrom = model.DateFrom,
                Price = model.Price,
                ComponentsTypes =
                    new List<SelectListItem>(
                        componentsService.GetComponentsTypes()
                            .Select(c => new SelectListItem {Text = c.PartName, Value = c.Id.ToString()})),
            };
            return View("AddCommponents",model2);
        }

        [HttpGet]
        public ActionResult AddCommponents(AddTaskViewModel model)
        {
            model.ComponentsTypes = new List<SelectListItem>(componentsService.GetComponentsTypes()
                .Select(c => new SelectListItem {Text = c.PartName, Value = c.Id.ToString()}));
            model.Components = componentsService.GetComponents(model.ComponentType);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> AddCommponentsList(AddTaskViewModel model)
        {
            await orderService.AddTaskWithComponentsList((TaskProgressDto)model, model.Posted.ComponentsIds);
            return RedirectToAction("OrderDetails", new {id = model.OrderId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChooseComponent(int orderId, int taskId, int componentId)
        {
            orderService.ChooseComponent(new ChooseTaskModel {OrderId = orderId, TaskId = taskId, ComponentId = componentId});
            return RedirectToAction("OrderDetails", new {id = orderId});
        }
    }
}