using Goblin.Core.Models;

namespace Goblin.Resource.Share.Models
{
    public class GoblinResourceGetFileModel : GoblinApiRequestModel
    {
        public string Slug { get; set; }
    }
}