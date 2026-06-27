using Microsoft.AspNetCore.Mvc;

namespace Evo.BI.Lark;

[ApiController]
[Route("lark/order")]
public sealed class DataConnectorController : ControllerBase
{
    private readonly OrderDataConnectorService _orderDataConnectorService;

    public DataConnectorController(OrderDataConnectorService orderDataConnectorService)
    {
        _orderDataConnectorService = orderDataConnectorService;
    }

    [HttpGet("meta.json")]
    public Task<object> GetMetaAsync()
    {
        return _orderDataConnectorService.GetMetaAsync();
    }

    [HttpPost("table_meta")]
    public Task<object> PostTableMetaAsync()
    {
        return _orderDataConnectorService.PostTableMetaAsync();
    }

    [HttpPost("records")]
    public Task<object> PostRecordsAsync()
    {
        return _orderDataConnectorService.PostRecordsAsync();
    }
}
