using System.Text.Json.Serialization;

namespace Evo.BI.Lark.Bitable;

/// <summary>
/// Property 基类，用于多态指代
/// </summary>
[JsonPolymorphic] // 启用多态支持
[JsonDerivedType(typeof(NumberProperty))]
[JsonDerivedType(typeof(DateTimeProperty))]
[JsonDerivedType(typeof(CurrencyProperty))]
[JsonDerivedType(typeof(ProgressProperty))]
[JsonDerivedType(typeof(RatingProperty))]
public abstract class BitablePropertyBase { }

/// <summary>
/// 数字 Property 结构
/// </summary>
public class NumberProperty : BitablePropertyBase
{
    
    [JsonPropertyName("formatter")]
    public string Formatter { get; set; }
}

/// <summary>
/// 日期 Property 结构
/// </summary>
public class DateTimeProperty : BitablePropertyBase
{
    [JsonPropertyName("formatter")]
    public DateTimeFormatter Formatter { get; set; }
}

/// <summary>
/// 货币 Property 结构
/// </summary>
public class CurrencyProperty : BitablePropertyBase
{

    [JsonPropertyName("formatter")]
    public string Formatter { get; set; }
    
    
    [JsonPropertyName("currencyCode")]
    public CurrencyCode CurrencyCode { get; set; }
}

/// <summary>
/// 进度 Property 结构
/// </summary>
public class ProgressProperty :  BitablePropertyBase
{
    [JsonPropertyName("formatter")]
    public string Formatter { get; set; }
    
    
    [JsonPropertyName("min")]
    public double Min
    {
        get;
        set;
    }


    [JsonPropertyName("max")]
    public double Max
    {
        get;
        set;
    }
    
    [JsonPropertyName("color")]
    public Color Color { get; set; }
}


/// <summary>
/// 评分 Property 结构
/// </summary>
public class RatingProperty : BitablePropertyBase
{
    [JsonPropertyName("symbol")]
    public Symbol Symbol { get; set; }
    
    [JsonPropertyName("max")]
    public double Max { get; set; }
}