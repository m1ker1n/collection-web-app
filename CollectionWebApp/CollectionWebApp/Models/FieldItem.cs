namespace CollectionWebApp.Models
{
    public class FieldItem
    {
        public enum ValueType
        {
            NumberType,
            StringType,
            TextType,
            DateType,
            BoolType
        }

        public double? NumberValue { get; set; }
        public string? StringValue { get; set; }
        public string? TextValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BoolValue { get; set; }

        public ValueType Type { get; set; }

        public object? Value 
        {
            get
            {
                switch(Type)
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
                switch (Type)
                {
                    case ValueType.NumberType:
                        NumberValue = (double?) value;
                        break;
                    case ValueType.StringType:
                        StringValue = (string?) value;
                        break;
                    case ValueType.TextType:
                        TextValue = (string?) value;
                        break;
                    case ValueType.DateType:
                        DateValue = (DateTime?) value;
                        break;
                    case ValueType.BoolType:
                        BoolValue = (bool?) value;
                        break;
                }
            }
        }

        public int FieldId { get; set; }
        public virtual Field Field { get; set; } = null!;

        public int ItemId { get; set; }
        public virtual Item Item { get; set; } = null!;
    }
}
