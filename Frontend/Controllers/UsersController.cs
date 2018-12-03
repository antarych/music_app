using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using FileManagement;
using UserManagement.Application;
using Frontend.Models;
using Newtonsoft.Json;
using UserManagement.Domain;

namespace Frontend.Controllers
{
    public class UsersController : ApiController
    {
        protected UsersController() { }

        public UsersController(IUserManager userManager, 
            IAuthorizer authorizer, 
            IFileManager fileManager)
        {
            _userManager = userManager;
            _authorizer = authorizer;
            _fileManager = fileManager;
        }
        private readonly IUserManager _userManager;
        private readonly IAuthorizer _authorizer;
        private readonly IFileManager _fileManager;

        [HttpPost]
        [Route("users")]
        public IHttpActionResult RegisterNewUser([FromBody] UserRegistrationModel userRegistrationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CreateAccountRequest accountRequest;
            try
            {
                accountRequest = new CreateAccountRequest(
                    userRegistrationModel.Password,
                    new MailAddress(userRegistrationModel.Mail));
            }
            catch (ArgumentException)
            {
                return BadRequest("Fields must not be empty");
            }
            int createdUserId;
            try
            {
                createdUserId = _userManager.CreateUser(accountRequest);
            }
            catch (AccountAlreadyExistsException ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
            return Ok(createdUserId);
        }

        [HttpGet]
        [Route("users/{userId}")]
        public IHttpActionResult GetUser(int userId)
        {
            Account account;
            try
            {
                account = _userManager.GetUser(userId);
            }
            catch (AccountNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }

            return Ok(new UserPresentation(account.UserId, account.Email.ToString(), account.Profile));
        }

        [HttpGet]
        [Route("current")]
        public IHttpActionResult GetCurrentUser()
        {
            if (Request.Headers.Authorization == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Invalid token");
            }
            var token = Request.Headers.Authorization.ToString();
            var tokenString = token.Substring("Basic ".Length).Trim();
            if (!token.IsEmpty() && token.StartsWith("Basic"))
            {
                if (_authorizer.GetTokenInfo(tokenString) == null)
                {
                    return Content(HttpStatusCode.Unauthorized, "Invalid token");
                }
            }
            var tokenInfo = _authorizer.GetTokenInfo(tokenString);
            if (tokenInfo == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Invalid token");
            }
            Account account;
            try
            {
                account = _userManager.GetUser(tokenInfo.UserId);
            }
            catch (AccountNotFoundException ex)
            {
                return Content(HttpStatusCode.Unauthorized, ex.Message);
            }
            return Ok(new UserPresentation(account.UserId, account.Email.ToString(), account.Profile));
        }

        [HttpPut]
        [Route("users")]
        public IHttpActionResult UpdateUser([FromBody] UserUpdateModel updateProfileRequest)
        {
            if (Request.Headers.Authorization == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Invalid token");
            }
            var token = Request.Headers.Authorization.ToString();
            var tokenString = token.Substring("Basic ".Length).Trim();
            if (!token.IsEmpty() && token.StartsWith("Basic"))
            {
                if (_authorizer.GetTokenInfo(tokenString) == null && _userManager.GetUser(_authorizer.GetTokenInfo(tokenString).UserId) == null)
                {
                    return Content(HttpStatusCode.Unauthorized, "Invalid token");
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Account accountToChange;
            accountToChange = _userManager.GetUser(_authorizer.GetTokenInfo(tokenString).UserId);
            

            if (updateProfileRequest != null)
            {
                if (updateProfileRequest.Avatar != null)
                {
                    try
                    {
                        _fileManager.GetImage(updateProfileRequest.Avatar);
                        accountToChange.Profile.Avatar = updateProfileRequest.Avatar;
                    }
                    catch (FileNotFoundException ex)
                    {
                        return Content(HttpStatusCode.BadRequest, ex.Message);
                    }
                }
                if (updateProfileRequest.Username != null)
                    accountToChange.Profile.Username = updateProfileRequest.Username;
            }

            try
            {
                _userManager.UpdateUser(accountToChange);
            }
            catch (System.ArgumentException)
            {
                return Content(HttpStatusCode.Unauthorized, "Account not found");
            }
            return Ok(new UserPresentation(accountToChange));
        }

        private AuthorizationTokenInfo CheckToken()
        {
            if (Request.Headers.Authorization == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            var token = Request.Headers.Authorization.ToString();
            var tokenString = token.Substring("Basic ".Length).Trim();
            if (!token.IsEmpty() && token.StartsWith("Basic"))
            {
                if (_authorizer.GetTokenInfo(tokenString) == null)
                {
                    throw new UnauthorizedAccessException("Invalid token");
                }
            }
            var tokenInfo = _authorizer.GetTokenInfo(tokenString);
            if (tokenInfo == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            return tokenInfo;
        }
    }
}