using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Item> Items { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<TagItem> TagItems { get; set; } = null!;
    }
}
