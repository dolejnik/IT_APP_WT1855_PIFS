using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic.Services;

namespace ServiceOfElectronicDevices.Controllers.API
{
    public class OrdersController : ApiController
    {
        private readonly OrderService orderService;
        public OrdersController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        public OrdersController() : this(new OrderService())
        {
        }

        public IHttpActionResult Get()
        {
            var orders = orderService.GetOrderList();
            return Json(orders);
        }
    }
}
