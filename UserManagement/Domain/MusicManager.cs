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

        public Artist GetArtist(int artistId)
        {
            Require.Positive(artistId, nameof(artistId));

            var artist = _musicRepository.GetArtist(artistId);
            if (artist == null)
            {
                throw new AccountNotFoundException("Artist not found");
            }
            return artist;
        }

        public void UpdateArtist(Artist artist)
        {
            Require.NotNull(artist, nameof(artist));

            if (_musicRepository.GetArtist(artist.ArtistId) == null)
            {
                throw new AccountNotFoundException();
            }
            _musicRepository.UpdateArtist(artist);
        }

        public void DeleteArtist(int artistId)
        {
            _musicRepository.DeleteArtist(artistId);
        }

        public int CreateSong(CreateSongRequest request)
        {
            Require.NotNull(request, nameof(request));
            Artist artist = _musicRepository.GetArtists(a => a.Name.Equals(request.Artist)).SingleOrDefault();
            if (artist == null)
            {
                throw new AccountAlreadyExistsException("Artist not found");
            }
            var newSong = new Song(
                request.Name,
                request.Text,
                artist);
            var songId = _musicRepository.CreateSong(newSong);
            return songId;
        }

        public Song GetSong(int songId)
        {
            Require.Positive(songId, nameof(songId));

            var song = _musicRepository.GetSong(songId);
            if (song == null)
            {
                throw new AccountNotFoundException("Song not found");
            }
            return song;
        }

        public void UpdateSong(Song song, string artistName)
        {
            Require.NotNull(song, nameof(song));

            if (_musicRepository.GetSong(song.SongId) == null)
            {
                throw new AccountNotFoundException();
            }
            Artist artist = _musicRepository.GetArtists(a => a.Name.Equals(artistName)).SingleOrDefault();
            if (artist == null)
            {
                throw new AccountAlreadyExistsException("Artist not found");
            }
            song.Artist = artist;
            _musicRepository.UpdateSong(song);
        }

        public void DeleteSong(int songId)
        {
            _musicRepository.DeleteSong(songId);
        }

    }
}
