using System.Net;
using System.Web.Http;
using Common;
using Frontend.Models;
using MusicManagement.Application;
using MusicManagement.Domain;

namespace Frontend.Controllers
{

    public class AuthorizationController : ApiController
    {
        private readonly IAuthorizer _authorizer;
        private readonly IUserManager _userManager;

        public AuthorizationController(IAuthorizer authorizer, IUserManager userManager)
        {
            _authorizer = authorizer;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Authorize([FromBody] LoginInformation loginInformation)
        {
            AuthorizationTokenInfo token;
            try
            {
                token = _authorizer.Authorize(loginInformation.Mail,
                    new Password(loginInformation.Password));
            }
            catch (AccountNotFoundException ex)
            {
                return Content(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (IncorrectPasswordException ex)
            {
                return Content(HttpStatusCode.Unauthorized, ex.Message);
            }
            var account = _authorizer.GetUserByMail(loginInformation.Mail);
            return Ok(new CurrentUserPresentation(account, token));                      
        }

        [HttpPost]
        [Route("logOut")]
        public IHttpActionResult LogOut()
        {
            if (Request.Headers.Authorization == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Invalid token");
            }
            var token = Request.Headers.Authorization.ToString();
            var tokenString = token.Substring("Basic ".Length).Trim();


            if (_authorizer.GetTokenInfo(tokenString) == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Invalid token");
            }
            _authorizer.LogOut(tokenString);
            return Ok();
        }
    }
}
