namespace CollectionWebApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Created { get; set; }

        public int UserCollectionId { get; set; }
        public virtual UserCollection UserCollection { get; set; } = null!;

        public virtual ICollection<Tag> Tags { get; set; } = null!; 
        public virtual ICollection<TagItem> TagItems { get; set; } = null!;

        public virtual ICollection<Field> Fields { get; set; } = null!;
        public virtual ICollection<FieldItem> FieldItems { get; set; } = null!;

        public virtual ICollection<Like> Likes { get; set; } = null!;

        public virtual ICollection<Commentary> Commentaries { get; set; } = null!;
    }
}
