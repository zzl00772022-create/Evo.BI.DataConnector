namespace Evo.BI.Lark.Bitable;

/// <summary>
/// 飞书多维表格字段类型枚举
/// </summary>
public enum BitableFieldType
{
    Text = 1,           // 多行文本
    Number = 2,         // 数字
    SingleSelect = 3,   // 单选
    MultiSelect = 4,    // 多选
    DateTime = 5,       // 日期
    Barcode = 6,        // 条码
    Checkbox = 7,       // 复选框
    Currency = 8,       // 货币
    Phone = 9,          // 电话号码
    Url = 10,           // 超链接
    Progress = 11,      // 进度
    Rating = 12,        // 评分
    Location = 13,      // 地理位置
    User = 14,          // 人员
    Attachment = 15,    // 附件
    Group = 16          // 群组
}