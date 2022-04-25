namespace CollectionWebApp.Models
{
    public class TagItem
    {
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; } = null!;

        public int ItemId { get; set; }
        public virtual Item Item { get; set; } = null!;
    }
}
