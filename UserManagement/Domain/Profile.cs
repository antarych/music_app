using System.Collections.Generic;
using Common;

namespace MusicManagement.Domain
{
    public class Profile
    {
        public virtual string Avatar { get; set; }

        public virtual string Username { get; set; }

        public virtual int FavArtists { get; set; }

        public virtual int FavSongs { get; set; }
    }
}
