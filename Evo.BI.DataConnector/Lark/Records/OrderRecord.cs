using Evo.BI.Lark.Bitable;

namespace Evo.BI.Lark;

public sealed class OrderRecord
{
    [BitableField("OrderNo", "订单编号", BitableFieldType.Text, IsPrimary = true)]
    public string OrderNo { get; set; }

    [BitableField("CustomerName", "客户名称", BitableFieldType.Text)]
    public string CustomerName { get; set; }

    [BitableField("ProductName", "商品名称", BitableFieldType.Text)]
    public string ProductName { get; set; }

    [BitableNumberField("Quantity", "数量", Formatter = "0")]
    public int Quantity { get; set; }

    [BitableCurrencyField("Amount", "订单金额", Formatter = "#,##0.00", CurrencyCode = CurrencyCode.CNY)]
    public decimal Amount { get; set; }

    [BitableDateField("OrderDate", "下单日期", Formatter = DateTimeFormatter.yyyyMMdd_Slash)]
    public DateTime OrderDate { get; set; }

    [BitableField("Status", "订单状态", BitableFieldType.Text)]
    public string Status { get; set; }
}
