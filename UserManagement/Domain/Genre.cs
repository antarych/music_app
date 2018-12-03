
using System.Collections.Generic;

namespace UserManagement
{
    public class Genre
    {
        public Genre(string genre)
        {
            Genre_name = genre;
        }

        protected Genre()
        {
        }

        public virtual int GenreId { get; protected set; }

        public virtual string Genre_name { get; protected set; }

        private ISet<Song> _songs;
        public virtual ISet<Song> Songs
        {
            get
            {
                return _songs ?? (_songs = new HashSet<Song>());
            }
            set { _songs = value; }
        }

        private ISet<Artist> _artists;
        public virtual ISet<Artist> Artists
        {
            get
            {
                return _artists ?? (_artists = new HashSet<Artist>());
            }
            set { _artists = value; }
        }
    }
}
