using DataAccess.NHibernate;
using Journalist;
using MusicManagement;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class MusicRepository : IMusicRepository
    {
        private readonly ISessionProvider _sessionProvider;

        public MusicRepository(ISessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));
            _sessionProvider = sessionProvider;
        }

        public int CreateArtist(Artist artist)
        {
            Require.NotNull(artist, nameof(artist));

            var session = _sessionProvider.GetCurrentSession();
            var savedArtistId = (int)session.Save(artist);
            return savedArtistId;
        }

        public Artist GetArtist(int artistId)
        {
            Require.Positive(artistId, nameof(artistId));

            var session = _sessionProvider.GetCurrentSession();
            var artist = session.Get<Artist>(artistId);
            return artist;
        }

        public List<Artist> GetArtists(Func<Artist, bool> predicate = null)
        {
            var session = _sessionProvider.GetCurrentSession();
            return predicate == null
                ? session.Query<Artist>().ToList()
                : session.Query<Artist>().Where(predicate).ToList();
        }

        public void UpdateArtist(Artist artist)
        {
            Require.NotNull(artist, nameof(artist));

            var session = _sessionProvider.GetCurrentSession();
            session.Update(artist);
        }

        public void DeleteArtist(int artistId)
        {
            Require.Positive(artistId, nameof(artistId));

            var session = _sessionProvider.GetCurrentSession();
            var artist = GetArtist(artistId);
            session.Delete(artist);
        }

        public int CreateSong(Song song)
        {
            Require.NotNull(song, nameof(song));

            var session = _sessionProvider.GetCurrentSession();
            var savedSongId= (int)session.Save(song);
            return savedSongId;
        }

        public Song GetSong(int songId)
        {
            Require.Positive(songId, nameof(songId));

            var session = _sessionProvider.GetCurrentSession();
            var song = session.Get<Song>(songId);
            return song;
        }

        public List<Song> GetSongs(Func<Song, bool> predicate = null)
        {
            var session = _sessionProvider.GetCurrentSession();
            return predicate == null
                ? session.Query<Song>().ToList()
                : session.Query<Song>().Where(predicate).ToList();
        }

        public void UpdateSong(Song song)
        {
            Require.NotNull(song, nameof(song));

            var session = _sessionProvider.GetCurrentSession();
            session.Update(song);
        }

        public void DeleteSong(int songId)
        {
            Require.Positive(songId, nameof(songId));

            var session = _sessionProvider.GetCurrentSession();
            var song = GetSong(songId);
            session.Delete(song);
        }
    }
}
