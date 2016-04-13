using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Services;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
      private readonly OrderService orderService;

        public HomeController()
        {
            this.orderService = new OrderService();
        }
        public ActionResult Index()
        {
            var VMtest = new DevicesDto { Id = 2, Brand = "Motorola", Model = "Fajny" };
            var VMtest2 = orderService.GetOrderList();


            return View(VMtest);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}