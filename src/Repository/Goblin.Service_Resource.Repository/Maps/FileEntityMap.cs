using Goblin.Service_Resource.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goblin.Service_Resource.Repository.Maps
{
    public class FileGoblinEntityMap : GoblinEntityMap<FileEntity>
    {
        public override void Map(EntityTypeBuilder<FileEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(FileEntity));

            builder.HasIndex(x => x.Hash);
        }
    }
}