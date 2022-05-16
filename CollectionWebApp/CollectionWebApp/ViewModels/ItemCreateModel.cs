using System.ComponentModel.DataAnnotations;

namespace CollectionWebApp.ViewModels
{
    public class ItemCreateModel
    {
        public int UserCollectionId { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
