namespace CollectionWebApp.Models
{
    public class UserCollection
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Created { get; set; }

        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; } = null!;

        public virtual ICollection<Item> Items { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Field> Fields { get; set; } = null!;
    }
}
