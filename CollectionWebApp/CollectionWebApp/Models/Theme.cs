using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<UserCollection> UserCollections { get; set; } = null!;
    }
}
