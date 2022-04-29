using CollectionWebApp.Models;

namespace CollectionWebApp.ViewModels
{
    public class AccountModel
    {
        public User User { get; set; } = null!;

        public bool ChangeAllowed { get; set; }
    }
}
