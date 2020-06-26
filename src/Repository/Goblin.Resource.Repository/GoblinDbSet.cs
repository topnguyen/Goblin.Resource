using Goblin.Resource.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Resource.Repository
{
    public sealed partial class GoblinDbContext
    {
        public DbSet<FileEntity> Files { get; set; }
    }
}