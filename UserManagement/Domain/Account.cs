using System;
using System.Collections.Generic;
using Common;
using Journalist;
using System.Net.Mail;

namespace UserManagement.Domain
{
    public class Account
    {
        public Account(
            MailAddress email,
            Password password,
            DateTime registrationTime)
        {
            Require.NotNull(email, nameof(email));
            Require.NotNull(password, nameof(password));
            
            Email = email;
            Password = password;
            RegistrationTime = registrationTime;
            Profile = new Profile();
        }

        protected Account()
        {
        }

        public virtual int UserId { get; protected set; }

        public virtual MailAddress Email { get; protected set; }

        public virtual Password Password { get; set; }

        public virtual DateTime RegistrationTime { get; protected set; }

        public virtual Profile Profile { get; set; }

    }
}
