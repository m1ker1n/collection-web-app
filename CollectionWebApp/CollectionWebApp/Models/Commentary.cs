using System.ComponentModel.DataAnnotations.Schema;

namespace CollectionWebApp.Models
{
    public class Commentary
    {
        public string Text { get; set; } = null!;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; } = null!;

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
