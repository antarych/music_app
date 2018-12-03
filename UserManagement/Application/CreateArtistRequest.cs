using System.Net.Mail;
using Journalist;


namespace MusicManagement.Application
{
    public class CreateArtistRequest
    {
        public CreateArtistRequest(            
            string name,
            string description)
        {
            Require.NotNull(name, nameof(name));
            Require.NotEmpty(description, nameof(description));

            Name = name;
            Description = description;
        }
        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
