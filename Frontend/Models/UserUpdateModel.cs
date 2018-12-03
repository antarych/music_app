using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UserManagement.Domain;

namespace Frontend.Models
{
    public class UserUpdateModel
    {
        public string Avatar { get; set; }

        [MaxLength(60)]
        public string Username { get; set; }
    }
}