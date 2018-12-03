
using System.Security.Principal;
using Journalist;
using UserManagement.Domain;

namespace Frontend.Authorization
{
    public class MusicAppPrincipal : IPrincipal
    {
        public MusicAppPrincipal(IIdentity identity)
        {
            Require.NotNull(identity, nameof(identity));

            Identity = identity;
        }

        public static IPrincipal EmptyPrincipal
            => new MusicAppPrincipal(MusicAppIdentity.EmptyIdentity) { IsEmpty = true };

        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity { get; }

        public bool IsEmpty { get; private set; }
    }
}