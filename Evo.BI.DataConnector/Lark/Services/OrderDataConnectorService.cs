using System.Reflection;
using System.Text.Json;
using Evo.BI.Lark.Bitable;
using Microsoft.Extensions.Options;

namespace Evo.BI.Lark;

public sealed class OrderDataConnectorService : DataConnectorService
{
    private static readonly IReadOnlyList<OrderRecord> Orders = new[]
    {
        new OrderRecord
        {
            OrderNo = "SO202606270001",
            CustomerName = "上海云织服饰有限公司",
            ProductName = "夏季短袖衬衫1",
            Quantity = 12,
            Amount = 3588.00m,
            OrderDate = new DateTime(2026, 6, 21),
            Status = "待发货"
        },
        new OrderRecord
        {
            OrderNo = "SO202606270002",
            CustomerName = "杭州青禾贸易有限公司",
            ProductName = "轻薄防晒外套",
            Quantity = 8,
            Amount = 3192.00m,
            OrderDate = new DateTime(2026, 6, 22),
            Status = "已发货"
        },
        new OrderRecord
        {
            OrderNo = "SO202606270003",
            CustomerName = "南京拾光买手店",
            ProductName = "高腰阔腿裤",
            Quantity = 5,
            Amount = 1495.00m,
            OrderDate = new DateTime(2026, 6, 23),
            Status = "已完成"
        },
        new OrderRecord
        {
            OrderNo = "SO202606270004",
            CustomerName = "成都栖木集合店",
            ProductName = "针织开衫",
            Quantity = 10,
            Amount = 2690.00m,
            OrderDate = new DateTime(2026, 6, 24),
            Status = "待付款"
        },
        new OrderRecord
        {
            OrderNo = "SO202606270005",
            CustomerName = "深圳蓝湾百货",
            ProductName = "通勤半身裙",
            Quantity = 16,
            Amount = 4784.00m,
            OrderDate = new DateTime(2026, 6, 25),
            Status = "已发货"
        },
        new OrderRecord
        {
            OrderNo = "SO202606270006",
            CustomerName = "北京鹿鸣商贸有限公司",
            ProductName = "真丝印花连衣裙",
            Quantity = 3,
            Amount = 2397.00m,
            OrderDate = new DateTime(2026, 6, 26),
            Status = "已取消"
        }
    };

    private record PageRequest(int Offset, int PageSize);

    public OrderDataConnectorService(
        IHttpContextAccessor httpContextAccessor,
        IOptions<LarkDataConnectorOptions> options)
        : base(httpContextAccessor, options)
    {
    }

    public override Task<object> PostTableMetaAsync()
    {
        var fields = BitableFieldGenerator.Parse<OrderRecord>();

        return Task.FromResult<object>(new
        {
            code = 0,
            msg = "成功",
            data = new
            {
                tableName = "OrderData",
                fields
            }
        });
    }

    public override async Task<object> PostRecordsAsync()
    {
        var pageRequest = await GetPageRequestAsync();
        var pageRecords = Orders
            .Skip(pageRequest.Offset)
            .Take(pageRequest.PageSize)
            .ToList();
        var nextOffset = pageRequest.Offset + pageRecords.Count;
        var hasMore = nextOffset < Orders.Count;

        return new
        {
            code = 0,
            msg = "成功",
            data = new
            {
                nextPageToken = hasMore ? nextOffset.ToString() : string.Empty,
                hasMore,
                records = pageRecords.Select(record => new
                {
                    primaryID = record.OrderNo,
                    data = ToRecordData(record)
                })
            }
        };
    }

    private async Task<PageRequest> GetPageRequestAsync()
    {
        var request = HttpContextAccessor.HttpContext?.Request;
        if (request?.Body is null)
        {
            return new PageRequest(0, Orders.Count);
        }

        request.EnableBuffering();
        request.Body.Position = 0;
        var body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;

        if (string.IsNullOrWhiteSpace(body))
        {
            return new PageRequest(0, Orders.Count);
        }

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;
        if (root.TryGetProperty("params", out var paramsElement))
        {
            var paramsJson = paramsElement.ValueKind == JsonValueKind.String
                ? paramsElement.GetString()
                : paramsElement.GetRawText();

            if (!string.IsNullOrWhiteSpace(paramsJson))
            {
                using var paramsDocument = JsonDocument.Parse(paramsJson);
                return ParsePageRequest(paramsDocument.RootElement);
            }
        }

        return ParsePageRequest(root);
    }

    private static PageRequest ParsePageRequest(JsonElement element)
    {
        var pageToken = GetStringProperty(element, "pageToken")
                        ?? GetStringProperty(element, "nextPageToken")
                        ?? "0";
        var offset = int.TryParse(pageToken, out var parsedOffset) ? parsedOffset : 0;
        var maxPageSize = GetIntProperty(element, "maxPageSize") ?? Orders.Count;
        var pageSize = Math.Clamp(maxPageSize, 1, Orders.Count);

        return new PageRequest(Math.Clamp(offset, 0, Orders.Count), pageSize);
    }

    private static string GetStringProperty(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;
    }

    private static int? GetIntProperty(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property))
        {
            return null;
        }

        return property.ValueKind switch
        {
            JsonValueKind.Number when property.TryGetInt32(out var value) => value,
            JsonValueKind.String when int.TryParse(property.GetString(), out var value) => value,
            _ => null
        };
    }

    private static Dictionary<string, object> ToRecordData(OrderRecord record)
    {
        return typeof(OrderRecord)
            .GetProperties()
            .ToDictionary(GetFieldKey, property => NormalizeValue(property.GetValue(record)));
    }

    private static string GetFieldKey(PropertyInfo property)
    {
        return property.GetCustomAttribute<BitableFieldAttribute>()?.FieldID ?? property.Name;
    }

    private static object NormalizeValue(object value)
    {
        return value switch
        {
            DateTime dateTime => new DateTimeOffset(dateTime).ToUnixTimeMilliseconds(),
            DateTimeOffset dateTimeOffset => dateTimeOffset.ToUnixTimeMilliseconds(),
            _ => value
        };
    }
}
