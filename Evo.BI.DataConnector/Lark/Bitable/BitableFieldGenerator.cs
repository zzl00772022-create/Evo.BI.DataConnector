using System.Reflection;

namespace Evo.BI.Lark.Bitable;


public static class BitableFieldGenerator
{
    public static List<BitableFieldInfo> Parse<T>()
    {
        var fields = new List<BitableFieldInfo>();
            
        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            // 获取基类特性即可，反射会自动抓取到特化的子类特性
            var attr = prop.GetCustomAttribute<BitableFieldAttribute>();
            if (attr == null) continue;

            fields.Add(new BitableFieldInfo
            {
                FieldID = attr.FieldID,
                FieldName = attr.FieldName,
                FieldType = attr.FieldType,
                IsPrimary = attr.IsPrimary ? true : (bool?)null,
                Description = string.IsNullOrEmpty(attr.Description) ? null : attr.Description,
                    
                // 核心多态调用：让特性自己决定生成什么结构的 Property
                Property = attr.GetPropertyObject() 
            });
        }

        return fields;
    }
}