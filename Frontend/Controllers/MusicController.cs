using FileManagement;
using Journalist;
using MusicManagement;
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

        [HttpGet]
        [Route("artist/{artistId}")]
        public IHttpActionResult GetArtist(int artistId)
        {
            Artist artist;
            try
            {
                artist = _musicManager.GetArtist(artistId);
            }
            catch (AccountNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }

            return Ok(artist);
        }

        [HttpPut]
        [Route("artist/{artistId}")]
        public IHttpActionResult UpdateUser([FromBody] ArtistCreationModel updateArtistRequest, int artistId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Artist artistToChange;
            artistToChange = _musicManager.GetArtist(artistId);

            if (updateArtistRequest != null)
            {
                if (updateArtistRequest.Name != null)
                    artistToChange.Name = updateArtistRequest.Name;
                if (updateArtistRequest.Description != null)
                    artistToChange.Description = updateArtistRequest.Description;
            }

            try
            {
                _musicManager.UpdateArtist(artistToChange);
            }
            catch (System.ArgumentException)
            {
                return Content(HttpStatusCode.Unauthorized, "Artist not found");
            }
            return Ok(_musicManager.GetArtist(artistToChange.ArtistId));
        }

        [HttpPost]
        [Route("artist/{artistId}/delete")]
        public IHttpActionResult DeleteArtist(int artistId)
        {
            Require.Positive(artistId, nameof(artistId));

            _musicManager.DeleteArtist(artistId);

            return Ok();
        }


        [HttpPost]
        [Route("song")]
        public IHttpActionResult AddNewSong([FromBody] SongCreationModel songCreationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CreateSongRequest songRequest;
            try
            {
                songRequest = new CreateSongRequest(
                    songCreationModel.Name,
                    songCreationModel.Text,
                    songCreationModel.Artist);
            }
            catch (ArgumentException)
            {
                return BadRequest("Fields must not be empty");
            }
            int createdSongId;
            try
            {
                createdSongId = _musicManager.CreateSong(songRequest);
            }
            catch (AccountAlreadyExistsException ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
            return Ok(createdSongId);
        }

        [HttpGet]
        [Route("song/{songId}")]
        public IHttpActionResult GetSong(int songId)
        {
            Song song;
            try
            {
                song = _musicManager.GetSong(songId);
            }
            catch (AccountNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }

            return Ok(song);
        }

        [HttpPut]
        [Route("song/{songId}")]
        public IHttpActionResult UpdateSong([FromBody] SongCreationModel updateSongRequest, int songId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Song songToChange;
            songToChange = _musicManager.GetSong(songId);

            if (updateSongRequest != null)
            {
                if (updateSongRequest.Name != null)
                    songToChange.Name = songToChange.Name;
                if (updateSongRequest.Text != null)
                    songToChange.Text = songToChange.Text;
            }

            try
            {
                _musicManager.UpdateSong(songToChange, updateSongRequest.Artist);
            }
            catch (System.ArgumentException)
            {
                return Content(HttpStatusCode.Unauthorized, "Song not found");
            }
            
            return Ok(_musicManager.GetSong(songId));
        }

        [HttpPost]
        [Route("song/{songId}/delete")]
        public IHttpActionResult DeleteSong(int songId)
        {
            Require.Positive(songId, nameof(songId));

            _musicManager.DeleteSong(songId);

            return Ok();
        }

    }
}
