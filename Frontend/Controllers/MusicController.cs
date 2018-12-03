using FileManagement;
using MusicManagement.Application;
using MusicManagement.Domain;
using System;
using System.Net;
using System.Web.Http;
using UserManagement.Application;
using UserManagement.Domain;

namespace Frontend.Controllers
{
    public class MusicController : ApiController
    {
        protected MusicController() { }

        public MusicController(MusicManager musicManager,
            IAuthorizer authorizer,
            IFileManager fileManager)
        {
            _musicManager = musicManager;
            _authorizer = authorizer;
            _fileManager = fileManager;
        }
        private readonly IMusicManager _musicManager;
        private readonly IAuthorizer _authorizer;
        private readonly IFileManager _fileManager;


        [HttpPost]
        [Route("artist")]
        public IHttpActionResult AddNewArtist([FromBody] ArtistCreationModel artistCreationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CreateArtistRequest artistRequest;
            try
            {
                artistRequest = new CreateArtistRequest(
                    artistCreationModel.Name,
                    artistCreationModel.Description);
            }
            catch (ArgumentException)
            {
                return BadRequest("Fields must not be empty");
            }
            int createdArtistId;
            try
            {
                createdArtistId = _musicManager.CreateArtist(artistRequest);
            }
            catch (AccountAlreadyExistsException ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
            return Ok(createdArtistId);
        }
    }
}
