using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic.Models;
using BusinessLogic.Services;

namespace ServiceOfElectronicDevices.Controllers.API
{
    public class OrdersController : ApiController
    {
        private readonly OrderService orderService;
        private readonly TokenService tokenService;

        public OrdersController(OrderService orderService, TokenService tokenService)
        {
            this.orderService = orderService;
            this.tokenService = tokenService;
        }

        public OrdersController() : this(new OrderService(), new TokenService())
        {
        }

        public IHttpActionResult Get(string token)
        {
            var user = tokenService.CheckToken(token);
            if (user == null)
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized));

            var orders = orderService.GetUserOrders(user.Id);
            return Json(orders);
        }

        public IHttpActionResult Get(string token, int orderId)
        {
            var user = tokenService.CheckToken(token);
            if (user == null)
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized));

            var order = orderService.GetOrderDetails(orderId);
            return Json(order);
        }

        public IHttpActionResult Post(ChooseTaskModel model)
        {
            var user = tokenService.CheckToken(model.Token);
            if (user == null)
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized));

            orderService.ChooseComponent(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
        }
    }
}
