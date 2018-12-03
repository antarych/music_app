
using System.Collections.Generic;
using MusicManagement.Domain;

namespace MusicManagement
{
    public class Song
    {
        public Song(
            string name,
            string text,
            Artist artist)
        {
            Name = name;
            Text = text;
            Artist = artist;
        }

        protected Song()
        {
        }

        public virtual int SongId { get; protected set; }

        public virtual string Name { get; set; }

        public virtual string Text { get; set; }

        public virtual Artist Artist { get; set; }

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
