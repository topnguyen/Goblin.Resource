using System.Reflection;
using Elect.Data.EF.Utils.ModelBuilderUtils;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Resource.Repository
{
    public sealed partial class GoblinDbContext : Elect.Data.EF.Services.DbContext.DbContext
    {
        public GoblinDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // [Important] Keep Under Base For Override And Make End Result

            // Scan and apply Config/Mapping for Tables/Entities (from folder "Maps")
            builder.AddConfigFromAssembly<GoblinDbContext>(typeof(GoblinDbContext).GetTypeInfo().Assembly);

            // Set Delete Behavior as Restrict in Relationship
            builder.DisableCascadingDelete();

            // Convention for Table name
            builder.RemovePluralizingTableNameConvention();

            builder.ReplaceTableNameConvention("Entity", string.Empty);
        }
    }
}