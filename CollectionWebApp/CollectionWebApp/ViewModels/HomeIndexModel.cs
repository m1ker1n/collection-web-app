using CollectionWebApp.Models;

namespace CollectionWebApp.ViewModels
{
    public class HomeIndexModel
    {
        public ICollection<Item> Items { get; set; } = null!;

        public ICollection<UserCollection> Collections { get; set; } = null!;

        public ICollection<Tag> Tags { get; set; } = null!;
    }
}
