﻿
using System;
using Journalist;

namespace UserManagement.Domain
{
    public class AuthorizationTokenInfo
    {
        public AuthorizationTokenInfo(int userId, string token, DateTime creationTime)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotNull(token, nameof(token));

            UserId = userId;
            Token = token;
            CreationTime = creationTime;
        }

        public int UserId { get; private set; }

        public string Token { get; private set; }

        public DateTime CreationTime { get; set; }
    }
}
