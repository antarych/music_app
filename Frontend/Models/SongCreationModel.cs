using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application
{
    public class SongCreationModel
    {
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Text { get; set; }

        public string Artist { get; set; }
    }
}
