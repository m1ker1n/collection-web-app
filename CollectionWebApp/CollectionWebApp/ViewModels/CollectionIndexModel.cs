using CollectionWebApp.Models;

namespace CollectionWebApp.ViewModels
{
    public class CollectionIndexModel
    {
        public UserCollection Collection { get; set; } = null!;

        public bool ChangeAllowed { get; set; }
    }
}
