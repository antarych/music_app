using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Http;
using System.Web.WebPages;
using FileManagement;
using MusicManagement.Application;
using Frontend.Models;
using MusicManagement.Domain;

namespace Frontend.Controllers
{
    public class UsersController : ApiController
    {
        protected UsersController() { }

        public UsersController(IUserManager musicManager, 
            IAuthorizer authorizer, 
            IFileManager fileManager)
        {
            _musicManager = musicManager;
            _authorizer = authorizer;
            _fileManager = fileManager;
        }
        private readonly IUserManager _musicManager;
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
                createdUserId = _musicManager.CreateUser(accountRequest);
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
                account = _musicManager.GetUser(userId);
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
                account = _musicManager.GetUser(tokenInfo.UserId);
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
                if (_authorizer.GetTokenInfo(tokenString) == null && _musicManager.GetUser(_authorizer.GetTokenInfo(tokenString).UserId) == null)
                {
                    return Content(HttpStatusCode.Unauthorized, "Invalid token");
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Account accountToChange;
            accountToChange = _musicManager.GetUser(_authorizer.GetTokenInfo(tokenString).UserId);
            

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
                _musicManager.UpdateUser(accountToChange);
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