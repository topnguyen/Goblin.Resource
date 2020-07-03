using Goblin.Core.Models;

namespace Goblin.Resource.Share.Models
{
    public class GoblinResourceGetFileModel : GoblinApiSubmitRequestModel
    {
        public string Slug { get; set; }
    }
}