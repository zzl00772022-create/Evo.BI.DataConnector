# Evo.BI.DataConnector

独立的飞书多维表格 Data Connector 服务，目前只保留 `order` 连接器，不再依赖 ClickHouse 或其他数据库。

## 运行

```powershell
dotnet run --project E:\feishu\Evo.BI.DataConnector\Evo.BI.DataConnector.csproj
```

默认地址：

```text
http://localhost:5035
```

## 配置

在 `appsettings.json` 中配置飞书连接器参数：

- `LarkDataConnector:Signature:Enabled`：是否启用飞书签名校验。
- `LarkDataConnector:Signature:SecretKey`：飞书 Data Connector 的签名密钥。
- `LarkDataConnector:Meta`：飞书连接器元数据配置。

## 接口

- `GET /`
- `GET /lark/order/meta.json`
- `POST /lark/order/table_meta`
- `POST /lark/order/records`
