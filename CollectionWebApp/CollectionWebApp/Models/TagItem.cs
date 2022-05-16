using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class TagItem
    {
        public int TagId { get; set; }
        [JsonIgnore]
        public virtual Tag Tag { get; set; } = null!;

        public int ItemId { get; set; }
        [JsonIgnore]
        public virtual Item Item { get; set; } = null!;
    }
}
