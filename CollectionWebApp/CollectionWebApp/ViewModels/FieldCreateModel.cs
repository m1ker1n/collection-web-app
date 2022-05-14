using CollectionWebApp.Models;

namespace CollectionWebApp.ViewModels
{
    public class FieldCreateModel
    {
        public string Name { get; set; } = null!;
        public Models.ValueType Type { get; set; }

        public int UserCollectionId { get; set; }
    }
}
