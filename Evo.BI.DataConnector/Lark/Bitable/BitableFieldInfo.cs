using System.Text.Json.Serialization;

namespace Evo.BI.Lark.Bitable;

public class BitableFieldInfo
{
    [JsonPropertyName("fieldID")]
    public string FieldID { get; set; }

    [JsonPropertyName("fieldName")]
    public string FieldName { get; set; }

    [JsonPropertyName("fieldType")]
    public BitableFieldType FieldType { get; set; } // 强类型枚举

    [JsonPropertyName("isPrimary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsPrimary { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Description { get; set; }

    [JsonPropertyName("property")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BitablePropertyBase Property { get; set; } // 强类型 Property
}