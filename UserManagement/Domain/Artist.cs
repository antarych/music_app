using System.Collections.Generic;
using MusicManagement.Domain;

namespace MusicManagement
{
    public class Artist
    {
        public Artist(
            string name,
            string description)
        {
            Name = name;
            Description = description;
        }

        protected Artist()
        {
        }

        public virtual int ArtistId { get; protected set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Picture { get; set; }

        private IList<Song> _songs;
        public virtual IList<Song> Songs
        {
            get
            {
                return _songs ?? (_songs = new List<Song>());
            }
            set { _songs = value; }
        }

        private ISet<Genre> _genres;
        public virtual ISet<Genre> Genres
        {
            get
            {
                return _genres ?? (_genres = new HashSet<Genre>());
            }
            set { _genres = value; }
        }

        private ISet<Account> _accounts;
        public virtual ISet<Account> Accounts
        {
            get
            {
                return _accounts ?? (_accounts = new HashSet<Account>());
            }
            set { _accounts = value; }
        }
    }
}
