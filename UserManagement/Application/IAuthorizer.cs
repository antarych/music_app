using System;
using Common;
using MusicManagement.Domain;

namespace MusicManagement.Application
{
    public interface IAuthorizer
    {
        TimeSpan TokenLifeTime { get; }

        AuthorizationTokenInfo GetTokenInfo(string authorizationToken);

        AuthorizationTokenInfo Authorize(string mail, Password password);

        Account GetUserByMail(string userMail);

        void LogOut(string authorizationToken);
    }
}
