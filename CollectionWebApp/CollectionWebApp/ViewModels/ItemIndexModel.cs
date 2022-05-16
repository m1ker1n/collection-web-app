using CollectionWebApp.Models;

namespace CollectionWebApp.ViewModels
{
    public class ItemIndexModel
    {
        public Item Item { get; set; } = null!;

        public CollectionWebApp.Models.User? User { get; set; }

        public bool ChangeAllowed { get; set; }
    }
}
