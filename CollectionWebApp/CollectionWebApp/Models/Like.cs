using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class Like
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }

        public int ItemId { get; set; }
        [JsonIgnore]
        public virtual Item Item { get; set; } = null!;

        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
    }
}
