
using System.Security.Principal;

namespace Frontend.Authorization
{
    public class MusicAppIdentity : IIdentity
    {
        public MusicAppIdentity(int userId, bool isAuthenticated)
        {
            UserId = userId;
            IsAuthenticated = isAuthenticated;
        }

        public int UserId { get; }

        public static MusicAppIdentity EmptyIdentity => new MusicAppIdentity(0, false);

        public string Name => UserId.ToString();

        public string AuthenticationType => "Token";

        public bool IsAuthenticated { get; }
    }
}
