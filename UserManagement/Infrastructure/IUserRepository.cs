using System;
using System.Collections.Generic;
using MusicManagement.Domain;

namespace MusicManagement.Infrastructure
{
    public interface IUserRepository
    {
        int CreateAccount(Account account);

        void UpdateAccount(Account account);

        Account GetAccount(int accountId);

        List<Account> GetAccounts(Func<Account, bool> predicate = null);
    }
}
