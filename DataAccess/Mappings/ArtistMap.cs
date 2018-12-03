using DataAccess.Mappings.Application;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using MusicManagement;

namespace DataAccess.Mappings
{
    class ArtistMap : ClassMapping<Artist>
    {
        public ArtistMap()
        {
            Table("Artist");
            Id(artist => artist.ArtistId, mapper => mapper.Generator(Generators.Identity));
            Property(artist => artist.Name, mapper => mapper.Column("Name"));
            Property(artist => artist.Description, mapper => mapper.Column("Description"));
            Property(artist => artist.Picture, mapper => mapper.Column("Picture"));
            Bag(x => x.Songs,
                            c => { c.Key(k => k.Column("Artist_Id"));
                            c.Inverse(true); },
                            r => r.OneToMany());
            Set(a => a.Genres,
                            c =>
                            {
                                c.Cascade(Cascade.All);
                                c.Key(k => k.Column("ArtistId"));
                                c.Table("Artist_Genre");
                                c.Inverse(true);
                            },
                            r => r.ManyToMany(m => m.Column("GenreId")));
        }
    }
}
