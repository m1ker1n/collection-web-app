using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ValueType Type { get; set; }

        public int UserCollectionId { get; set; }
        [JsonIgnore]
        public virtual UserCollection UserCollection { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Item> Items { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<FieldItem> FieldItems { get; set; } = null!;
    }
}
