using MusicManagement.Application;

namespace UserManagement.Application
{
    public interface IMusicManager
    {
        int CreateArtist(CreateArtistRequest reauest);

        //int CreateUser(CreateAccountRequest request);

        //void UpdateUser(Account account);

        //IEnumerable<Account> GetAccounts(Func<Account, bool> predicate = null);
    }
}
