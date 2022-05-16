using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Created { get; set; }

        public int UserCollectionId { get; set; }
        [JsonIgnore]
        public virtual UserCollection UserCollection { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Tag> Tags { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<TagItem> TagItems { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Field> Fields { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<FieldItem> FieldItems { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Like> Likes { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Commentary> Commentaries { get; set; } = null!;
    }
}
