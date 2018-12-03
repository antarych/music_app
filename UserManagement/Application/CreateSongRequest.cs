using System.Net.Mail;
using Journalist;


namespace MusicManagement.Application
{
    public class CreateSongRequest
    {
        public CreateSongRequest(
            string name,
            string text,
            string artist)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotEmpty(text, nameof(text));
            Require.NotEmpty(artist, nameof(artist));

            Name = name;
            Text = text;
            Artist = artist;
        }
        public string Name { get; private set; }

        public string Text { get; private set; }

        public string Artist { get; private set; }
    }
}
