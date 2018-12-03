using MusicManagement;
using System;
using System.Collections.Generic;

namespace UserManagement.Infrastructure
{
    public interface IMusicRepository
    {
        int CreateArtist(Artist artist);

        List<Artist> GetArtists(Func<Artist, bool> predicate = null);
    }
}
