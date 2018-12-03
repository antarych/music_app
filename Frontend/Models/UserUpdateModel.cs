using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class UserUpdateModel
    {
        public string Avatar { get; set; }

        [MaxLength(60)]
        public string Username { get; set; }
    }
}