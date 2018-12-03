using Journalist;
using MusicManagement;
using MusicManagement.Application;
using MusicManagement.Domain;
using System;
using System.Linq;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class MusicManager : IMusicManager
    {
        private readonly IMusicRepository _musicRepository;

        public MusicManager(IMusicRepository musicRepository)
        {
            _musicRepository = musicRepository;
        }

        public int CreateArtist(CreateArtistRequest request)
        {
            Require.NotNull(request, nameof(request));
            if (_musicRepository.GetArtists(artist => artist.Name.Equals(request.Name)).SingleOrDefault() != null)
            {
                throw new AccountAlreadyExistsException("Artist with such name already exists");
            }
            var newArtist = new Artist(
                request.Name,
                request.Description);
            var artistId = _musicRepository.CreateArtist(newArtist);
            return artistId;
        }
    }
}
