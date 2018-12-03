using MusicManagement;
using System;
using System.Collections.Generic;

namespace UserManagement.Infrastructure
{
    public interface IMusicRepository
    {
        int CreateArtist(Artist artist);

        Artist GetArtist(int artistId);

        void UpdateArtist(Artist artist);

        void DeleteArtist(int artistId);

        List<Artist> GetArtists(Func<Artist, bool> predicate = null);

        int CreateSong(Song song);

        Song GetSong(int songId);

        void UpdateSong(Song song);

        void DeleteSong(int songId);

        List<Song> GetSongs(Func<Song, bool> predicate = null);
    }
}
