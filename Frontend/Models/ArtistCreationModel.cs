using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application
{
    public class ArtistCreationModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
