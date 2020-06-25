using Goblin.Service_Resource.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Service_Resource.Repository
{
    public sealed partial class GoblinDbContext
    {
        public DbSet<FileEntity> Files { get; set; }
    }
}