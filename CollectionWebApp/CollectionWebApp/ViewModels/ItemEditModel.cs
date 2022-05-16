using CollectionWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CollectionWebApp.ViewModels
{
    public class ItemEditModel
    {
        public int ItemId { get; set; }

        public int UserCollectionId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<FieldItemModel> FieldItems { get; set; } = null!;
    }
}
