using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CollectionWebApp.Models
{
    public class FieldItem
    {
        public int NumberValue { get; set; } = 0;
        public string StringValue { get; set; } = String.Empty;
        public string TextValue { get; set; } = String.Empty;
        public DateTime DateValue { get; set; }
        public bool BoolValue { get; set; } = false;

        public object? Value 
        {
            get
            {
                if (Field == null) return null;
                switch(Field.Type)
                {
                    case ValueType.NumberType:
                        return NumberValue;
                    case ValueType.StringType:
                        return StringValue;
                    case ValueType.TextType:
                        return TextValue;
                    case ValueType.DateType:
                        return DateValue;
                    case ValueType.BoolType:
                        return BoolValue;
                }
                return null;
            }
            set
            {
                if (Field == null || value == null) return;
                switch (Field.Type)
                {
                    case ValueType.NumberType:
                        NumberValue = (Convert.ToInt32(value));
                        break;
                    case ValueType.StringType:
                        StringValue = (string) value;
                        break;
                    case ValueType.TextType:
                        TextValue = (string) value;
                        break;
                    case ValueType.DateType:
                        DateValue = (DateTime) value;
                        break;
                    case ValueType.BoolType:
                        BoolValue = (bool) value;
                        break;
                }
            }
        }

        public int FieldId { get; set; }
        [JsonIgnore]
        public virtual Field Field { get; set; } = null!;

        public int ItemId { get; set; }
        [JsonIgnore]
        public virtual Item Item { get; set; } = null!;
    }
}
