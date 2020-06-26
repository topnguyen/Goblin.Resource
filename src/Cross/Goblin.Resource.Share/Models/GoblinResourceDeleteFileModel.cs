using Goblin.Core.Models;

namespace Goblin.Resource.Share.Models
{
    public class GoblinResourceDeleteFileModel : GoblinApiRequestModel
    {
        public string Slug { get; set; }
    }
}