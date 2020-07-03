using Goblin.Core.Models;

namespace Goblin.Resource.Share.Models
{
    public class GoblinResourceDeleteFileModel : GoblinApiSubmitRequestModel
    {
        public string Slug { get; set; }
    }
}