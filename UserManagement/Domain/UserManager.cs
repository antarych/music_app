using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using UserManagement.Application;
using UserManagement.Infrastructure;
using Journalist;

namespace UserManagement.Domain
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository
            )
        {
            _userRepository = userRepository;
        }

        public int CreateUser(CreateAccountRequest request)
        {
            Require.NotNull(request, nameof(request));
            if (_userRepository.GetAccounts(user => user.Email.Equals(request.Email)).SingleOrDefault() != null)
            {
                throw new AccountAlreadyExistsException("Account with such mail already exists");
            }
            var newAccount = new Account(
                request.Email,
                new Password(request.Password),
                DateTime.Now);
            var userId = _userRepository.CreateAccount(newAccount);
            return userId;
        }

        public void UpdateUser(Account request)
        {
            Require.NotNull(request, nameof(request));

            var account = _userRepository.GetAccount(request.UserId);

            if (account == null)
            {
                throw new AccountNotFoundException();
            }
            _userRepository.UpdateAccount(account);
        }

        public Account GetUser(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var account = _userRepository.GetAccount(userId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            return account;
        }

        public IEnumerable<Account> GetAccounts(Func<Account, bool> predicate = null)
        {
            return _userRepository.GetAccounts(predicate);
        }
    }
}
