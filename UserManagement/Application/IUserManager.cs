using System;
using System.Collections.Generic;
using MusicManagement.Domain;

namespace MusicManagement.Application
{
    public interface IUserManager
    {
        Account GetUser(int userId);

        int CreateUser(CreateAccountRequest request);

        void UpdateUser(Account account);
        
        IEnumerable<Account> GetAccounts(Func<Account, bool> predicate = null);
    }
}
