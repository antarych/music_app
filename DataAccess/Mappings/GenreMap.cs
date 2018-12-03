using UserManagement;
using DataAccess.Mappings.Application;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DataAccess.Mappings
{
    class GenreMap : ClassMapping<Genre>
    {
        public GenreMap()
        {
            Table("Genre");
            Id(genre => genre.GenreId, mapper => mapper.Generator(Generators.Identity));
            Set(genre => genre.Songs,
                        c =>
                        {
                            c.Cascade(Cascade.Persist);
                            c.Key(k => k.Column("GenreId"));
                            c.Table("Song_Genre");
                        },
                        r => r.ManyToMany(m => m.Column("SongId")));
        }
    }
}
