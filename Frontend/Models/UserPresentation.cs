using System.Collections.Generic;
using System.Linq;
using Frontend.Models;

namespace MusicManagement.Domain
{
    public class UserPresentation
    {
        public UserPresentation(int userId, string email, Profile profile)
        {
            id = userId;
            name = profile.Username;
            mail = email;
            if (profile.Avatar != null)
            {
                avatar = profile.Avatar;
            }            
        }

        public UserPresentation(Account account)
        {
            id = account.UserId;
            name = account.Profile.Username;
            mail = account.Email.ToString();
            if (account.Profile.Avatar != null)
            {
                avatar = account.Profile.Avatar;
            }         
        }

        protected UserPresentation()
        {
            
        }

        public virtual int id { get; set; }

        public virtual string name { get; set; }

        public virtual string mail { get; set; }

        public virtual string avatar { get; set; }

    }
}