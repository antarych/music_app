using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Common;
using Journalist;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class Authorizer : IAuthorizer
    {        
        public Authorizer(TimeSpan tokenLifeTime, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            TokenLifeTime = tokenLifeTime;
        }

        private readonly IUserRepository _userRepository;


        public TimeSpan TokenLifeTime { get; }

        private readonly ConcurrentDictionary<string, AuthorizationTokenInfo> _tokensWithGenerationTime
            = new ConcurrentDictionary<string, AuthorizationTokenInfo>();

        public AuthorizationTokenInfo GetTokenInfo(string authorizationToken)
        {
            Require.NotEmpty(authorizationToken, nameof(authorizationToken));

            if (!_tokensWithGenerationTime.ContainsKey(authorizationToken))
            {
                return null;
            }

            var token = _tokensWithGenerationTime[authorizationToken];

            if (token.CreationTime + TokenLifeTime < DateTime.Now)
            {
                _tokensWithGenerationTime.TryRemove(token.Token, out token);
                return null;
            }

            token.CreationTime = DateTime.Now;
            return token;
        }

        public AuthorizationTokenInfo Authorize(string mail, Password password)
        {
            Require.NotNull(password, nameof(password));

            Require.NotEmpty(mail, nameof(mail));

            var userAccount = _userRepository.GetAccounts(account => account.Email.Address == mail).SingleOrDefault();

            if (userAccount == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (userAccount.Password.Value != password.Value)
            {
                throw new IncorrectPasswordException("Incorrect password");
            }

            var userToken = GetTokenByUserId(userAccount.UserId);
            if (userToken == null)
            {
                userToken = GenerateNewToken(userAccount);
                _tokensWithGenerationTime.AddOrUpdate(userToken.Token, userToken, (oldToken, info) => userToken);
            }

            return userToken;
        }

        public Account GetUserByMail(string mail)
        {
            var userAccount = _userRepository.GetAccounts(account => account.Email.Address == mail).SingleOrDefault();
            if (userAccount == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            return userAccount;
        }

        public void LogOut(string authorizationToken)
        {
            AuthorizationTokenInfo token;

            token = _tokensWithGenerationTime[authorizationToken];
            
            _tokensWithGenerationTime.TryRemove(token.Token, out token);
        }

        private AuthorizationTokenInfo GetTokenByUserId(int userId)
        {
            var tokenById = _tokensWithGenerationTime.SingleOrDefault(token => token.Value.UserId == userId);
            if (!tokenById.Equals(default(KeyValuePair<string, AuthorizationTokenInfo>)))
            {
                return tokenById.Value;
            }

            return null;
        }

        private static AuthorizationTokenInfo GenerateNewToken(Account account)
        {
            var guid = Guid.NewGuid();
            var token = BitConverter.ToString(guid.ToByteArray());
            token = token.Replace("-", "");
            return new AuthorizationTokenInfo(account.UserId, token, DateTime.Now);
        }       
    }
}
