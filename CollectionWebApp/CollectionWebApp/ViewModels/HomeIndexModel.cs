using CollectionWebApp.Models;

namespace CollectionWebApp.ViewModels
{
    public class HomeIndexModel
    {
        public ICollection<Item> Items { get; set; } = null!;
    }
}
