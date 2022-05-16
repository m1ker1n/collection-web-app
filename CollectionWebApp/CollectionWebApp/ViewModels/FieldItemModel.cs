using CollectionWebApp.Models;

namespace CollectionWebApp.ViewModels
{
    public class FieldItemModel
    {
        public int FieldId { get; set; }

        public string FieldName { get; set; } = null!;
        
        public int ItemId { get; set; }

        public int NumberValue { get; set; }
        public string StringValue { get; set; } = String.Empty;
        public string TextValue { get; set; } = String.Empty;
        public DateTime DateValue { get; set; }
        public bool BoolValue { get; set; }

        public CollectionWebApp.Models.ValueType Type { get; set; }
    }
}
