using MusicManagement;
using DataAccess.Mappings.Application;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DataAccess.Mappings
{
    class SongMap : ClassMapping<Song>
    {
        public SongMap()
        {
            Table("Song");
            Id(song => song.SongId, mapper => mapper.Generator(Generators.Identity));          
            Property(song => song.Name, mapper => mapper.Column("Name"));
            Property(song => song.Text, mapper => mapper.Column("Text"));
            ManyToOne(x => x.Artist,
                                c => {
                                    c.Cascade(Cascade.Persist);
                                    c.Column("ArtistId");});
            Set(song => song.Accounts,
                                c =>
                                {
                                    c.Cascade(Cascade.Persist);
                                    c.Key(k => k.Column("SongId"));
                                    c.Table("Song_Account");
                                },
                                r => r.ManyToMany(m => m.Column("UserId")));
        }
    }
}
