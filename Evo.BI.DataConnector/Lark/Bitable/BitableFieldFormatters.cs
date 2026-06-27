using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evo.BI.Lark.Bitable;

public class EnumDescriptionConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        foreach (var field in typeToConvert.GetFields())
        {
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null && attribute.Description == value)
                return (T)field.GetValue(null);
            if (field.Name == value)
                return (T)field.GetValue(null);
        }
        return default;
    }
  
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        
     
        writer.WriteStringValue(attribute != null ? attribute.Description : value.ToString());
    }
}

[JsonConverter(typeof(EnumDescriptionConverter<DateTimeFormatter>))]
public enum DateTimeFormatter
{
    /// <summary>
    /// yyyy/MM/dd
    /// </summary>
    [Description("yyyy/MM/dd")] yyyyMMdd_Slash,

    /// <summary>
    /// yyyy-MM-dd
    /// </summary>
    [Description("yyyy-MM-dd")] yyyyMMdd_Dash,

    /// <summary>
    /// MM-dd
    /// </summary>
    [Description("MM-dd")] MMdd_Dash,

    /// <summary>
    /// MM/dd/yyyy
    /// </summary>
    [Description("MM/dd/yyyy")] MMddyyy_Slash,

    /// <summary>
    /// dd/MM/yyyy
    /// </summary>
    [Description("dd/MM/yyyy")] ddMMyyy_Slash,

    /// <summary>
    /// yyyy/MM/dd HH:mm
    /// </summary>
    [Description("yyyy/MM/dd HH:mm")] yyyyMMddHHmm_Slash,

    /// <summary>
    /// yyyy-MM-dd HH:mm
    /// </summary>
    [Description("yyyy-MM-dd HH:mm")] yyyyMMddHHmm_Dash
}

[JsonConverter(typeof(EnumDescriptionConverter<CurrencyCode>))]
public enum CurrencyCode
{
    /// <summary>
    /// 人民币
    /// </summary>
    [Description("CNY")] CNY,
    /// <summary>
    /// 美元
    /// </summary>
    [Description("USD")] USD,
    /// <summary>
    /// 欧元
    /// </summary>
    [Description("EUR")] EUR,
    /// <summary>
    /// 英镑
    /// </summary>
    [Description("GBP")] GBP,
    /// <summary>
    /// 阿联酋迪拉姆
    /// </summary>
    [Description("AED")] AED,
    /// <summary>
    /// 澳大利亚元
    /// </summary>
    [Description("AUD")] AUD,
    /// <summary>
    /// 巴西雷亚尔
    /// </summary>
    [Description("BRL")] BRL,
    /// <summary>
    /// 加拿大元
    /// </summary>
    [Description("CAD")] CAD,
    /// <summary>
    /// 瑞士法郎
    /// </summary>
    [Description("CHF")] CHF,
    /// <summary>
    /// 港元
    /// </summary>
    [Description("HKD")] HKD,
    /// <summary>
    /// 印度卢比
    /// </summary>
    [Description("INR")] INR,
    /// <summary>
    /// 印尼盾
    /// </summary>
    [Description("IDR")] IDR,
    /// <summary>
    /// 日元
    /// </summary>
    [Description("JPY")] JPY,
    /// <summary>
    /// 韩元
    /// </summary>
    [Description("KRW")] KRW,
    /// <summary>
    /// 澳门元
    /// </summary>
    [Description("MOP")] MOP,
    /// <summary>
    /// 墨西哥比索
    /// </summary>
    [Description("MXN")] MXN,
    /// <summary>
    /// 马来西亚令吉
    /// </summary>
    [Description("MYR")] MYR,
    /// <summary>
    /// 菲律宾比索
    /// </summary>
    [Description("PHP")] PHP,
    /// <summary>
    /// 波兰兹罗提
    /// </summary>
    [Description("PLN")] PLN,
    /// <summary>
    /// 俄罗斯卢布
    /// </summary>
    [Description("RUB")] RUB,
    /// <summary>
    /// 新加坡元
    /// </summary>
    [Description("SGD")] SGD,
    /// <summary>
    /// 泰国铢
    /// </summary>
    [Description("THB")] THB,
    /// <summary>
    /// 土耳其里拉
    /// </summary>
    [Description("TRY")] TRY,
    /// <summary>
    /// 新台币
    /// </summary>
    [Description("TWD")] TWD,
    /// <summary>
    /// 越南盾
    /// </summary>
    [Description("VND")] VND,
}

/// <summary>
/// symbol 图表
/// </summary>
[JsonConverter(typeof(EnumDescriptionConverter<Symbol>))]
public enum Symbol
{
    /// <summary>
    /// ⭐️ 
    /// </summary>
    [Description("⭐️")]
    Star,
    /// <summary>
    /// ❤️
    /// </summary>
    [Description("❤️")]
    Heart,
    /// <summary>
    /// 👍
    /// </summary>
    [Description("👍")]
    Thumbsup,
    /// <summary>
    /// 🔥
    /// </summary>
    [Description("🔥")]
    Fire,
    /// <summary>
    /// 😊
    /// </summary>
    [Description("😊")]
    Smile,
    /// <summary>
    /// ⚡️
    /// </summary>
    [Description("⚡️")]
    Lightning,
    /// <summary>
    /// 🌷
    /// </summary>
    [Description("🌷")]
    Flower,
    /// <summary>
    /// 🔢
    /// </summary>
    [Description("🔢")]
    Number
}


public enum Color
{
    [Description("蓝色")]
    蓝色 = 0,
    [Description("紫色")]
    紫色 = 1,
    [Description("深绿")]
    深绿 = 2,
    [Description("绿色")]
    绿色 = 3,
    [Description("青色")]
    青色 = 4,
    [Description("橙色")]
    橙色 = 5,
    [Description("红色")]
    红色 = 6,
    [Description("墨黑")]
    墨黑 = 7,
    [Description("蓝色渐变")]
    蓝色渐变 = 8,
    [Description("紫色渐变")]
    紫色渐变 = 9,
    [Description("橙色渐变")]
    橙色渐变 = 10,
    [Description("绿红")]
    绿红 = 11,
    [Description("红绿")]
    红绿 = 12,
    [Description("蓝粉")]
    蓝粉 = 13,
    [Description("粉蓝")]
    粉蓝 = 14,
    [Description("彩虹色")]
    彩虹色 = 15
}