using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class UserCollection
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Created { get; set; }

        public int ThemeId { get; set; }
        [JsonIgnore] 
        public virtual Theme Theme { get; set; } = null!;

        [JsonIgnore] 
        public virtual ICollection<Item> Items { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int UserId { get; set; }
        [JsonIgnore] 
        public virtual User User { get; set; } = null!;

        [JsonIgnore] 
        public virtual ICollection<Field> Fields { get; set; } = null!;
    }
}
