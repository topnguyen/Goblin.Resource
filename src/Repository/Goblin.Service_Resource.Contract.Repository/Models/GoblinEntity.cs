using Elect.Data.EF.Models;
using Goblin.Core.DateTimeUtils;

namespace Goblin.Service_Resource.Contract.Repository.Models
{
    public abstract class GoblinEntity : Entity<long>
    {
        protected GoblinEntity()
        {
            CreatedTime = LastUpdatedTime = GoblinDateTimeHelper.SystemTimeNow;
        }
    }
}