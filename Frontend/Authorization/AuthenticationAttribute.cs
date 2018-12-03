﻿using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Journalist;
using UserManagement.Application;
using UserManagement.Domain;

namespace Frontend.Authorization
{
    public class AuthenticationAttribute : IAuthenticationFilter
    {
        private readonly IAuthorizer _authorizer;

        public AuthenticationAttribute(IAuthorizer authorizer)
        {
            Require.NotNull(authorizer, nameof(authorizer));

            _authorizer = authorizer;
        }

        public bool AllowMultiple => true;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var tokenString = context?.Request?.Headers?.Authorization?.Parameter;
            if (string.IsNullOrEmpty(tokenString))
            {
                SetupUnauthenticated();
                return;
            }

            var tokenInfo = _authorizer.GetTokenInfo(tokenString);
            if (tokenInfo == null)
            {
                context.ErrorResult = new UnauthorizedResult(
                    new AuthenticationHeaderValue[] { },
                    context.Request);
                await context.ErrorResult.ExecuteAsync(cancellationToken);
                SetupUnauthenticated();
                return;
            }

            var identity = new MusicAppIdentity(tokenInfo.UserId, true);
            var principal = new MusicAppPrincipal(identity);

            Thread.CurrentPrincipal = principal;
            context.Principal = principal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void SetupUnauthenticated()
        {
            Thread.CurrentPrincipal = MusicAppPrincipal.EmptyPrincipal;
        }

    }
}