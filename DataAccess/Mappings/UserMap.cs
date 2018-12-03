using DataAccess.Mappings.Application;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using MusicManagement.Domain;

namespace DataAccess.Mappings
{
    class UserMap : ClassMapping<Account>
    {
        public UserMap()
        {
            Table("Account");
            Id(user => user.UserId, mapper => mapper.Generator(Generators.Identity));          
            Property(user => user.Email, mapper =>
            {
                mapper.Column("Email");
                mapper.Unique(true);
                mapper.Type<MailAddressType>();

            });
            Property(user => user.Password, mapper =>
            {
                mapper.Column("Password");
                mapper.Type<PasswordType>();
            });
            Component(x => x.Profile, m =>
            {
                m.Property(profile => profile.Avatar, mapper => mapper.Column("Avatar"));
                m.Property(profile => profile.Username, mapper => mapper.Column("Username"));
                m.Property(profile => profile.FavSongs, mapper => mapper.Column("NumberOfFavSongs"));
                m.Property(profile => profile.FavArtists, mapper => mapper.Column("NumberOfFavArtists"));
            });
            Property(user => user.RegistrationTime, mapper => mapper.Column("RegistrationDate"));
            Set(user => user.Artists,
                                c =>
                                {
                                    c.Cascade(Cascade.Persist);
                                    c.Key(k => k.Column("UserId"));
                                    c.Table("Artist_account");
                                },
                                r => r.ManyToMany(m => m.Column("ArtistId")));
        }
    }
}
