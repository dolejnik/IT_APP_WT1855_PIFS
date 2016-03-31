using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BusinessLogic.Services;
using Microsoft.AspNet.Identity;
using ServiceOfElectronicDevices.Models;
using Web;
using Microsoft.AspNet.Identity.Owin;


namespace ServiceOfElectronicDevices.Controllers.API
{
    public class LoginController : ApiController
    {
        private ApplicationUserManager userManager;
        private readonly TokenService tokenService;

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        public LoginController() : this(new TokenService())
        {
        }

        public LoginController(TokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpPost]
        public IHttpActionResult Post(LoginModel model)
        {
            try
            {
                var loginInfo = UserManager.Find(model.Email, model.Password);
                if (loginInfo == null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized));

                var token = tokenService.SetToken(loginInfo.Id);
                return Json(new {Token=token, UserId=loginInfo.Id});
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
        }
    }
}
