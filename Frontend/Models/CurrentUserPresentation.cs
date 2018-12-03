using System.Collections.Generic;
using Frontend.Models;

namespace MusicManagement.Domain
{
    public class CurrentUserPresentation
    {
        public CurrentUserPresentation(Account account, AuthorizationTokenInfo userToken)
        {            
            token = userToken.Token;
            user = new UserPresentation(account);
        }

        protected CurrentUserPresentation()
        {
            
        }

        public virtual string token { get; set; }

        public UserPresentation user { get; set; }
    }
}