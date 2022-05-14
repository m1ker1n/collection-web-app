using CollectionWebApp.Models;

namespace CollectionWebApp.ViewModels
{
    public class FieldEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Models.ValueType Type { get; set; }

        public int UserCollectionId { get; set; }
    }
}
