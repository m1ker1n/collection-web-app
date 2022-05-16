using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; } = null!;
    }
}
