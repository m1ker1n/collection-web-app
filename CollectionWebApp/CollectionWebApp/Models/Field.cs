namespace CollectionWebApp.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int UserCollectionId { get; set; }
        public virtual UserCollection UserCollection { get; set; } = null!;

        public virtual ICollection<Item> Items { get; set; } = null!;
        public virtual ICollection<FieldItem> FieldItems { get; set; } = null!;
    }
}
