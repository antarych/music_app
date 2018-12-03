using MusicManagement;
using MusicManagement.Application;

namespace UserManagement.Application
{
    public interface IMusicManager
    {
        int CreateArtist(CreateArtistRequest request);

        Artist GetArtist(int artistId);

        void UpdateArtist(Artist artist);

        void DeleteArtist(int artistId);

        int CreateSong(CreateSongRequest request);

        Song GetSong(int songId);

        void UpdateSong(Song song, string artistName);

        void DeleteSong(int songId);
    }
}
