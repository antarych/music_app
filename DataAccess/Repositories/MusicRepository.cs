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

        public List<Artist> GetArtists(Func<Artist, bool> predicate = null)
        {
            var session = _sessionProvider.GetCurrentSession();
            return predicate == null
                ? session.Query<Artist>().ToList()
                : session.Query<Artist>().Where(predicate).ToList();
        }
    }
}
