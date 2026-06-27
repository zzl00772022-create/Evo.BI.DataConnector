using Microsoft.Extensions.Options;

namespace Evo.BI.Lark;

public abstract class DataConnectorService : IDataConnectorService
{
    private readonly LarkDataConnectorOptions _options;

    protected DataConnectorService(
        IHttpContextAccessor httpContextAccessor,
        IOptions<LarkDataConnectorOptions> options)
    {
        HttpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }

    protected IHttpContextAccessor HttpContextAccessor { get; }

    public virtual Task<object> GetMetaAsync()
    {
        var meta = _options.Meta;

        return Task.FromResult<object>(new
        {
            schemaVersion = 1,
            version = "1.2.0",
            type = "data_connector",
            extraData = new
            {
                disabledPeriodicSync = meta.DisabledPeriodicSync,
                dataSourceConfigUiUri = meta.DataSourceConfigUiUri,
                initHeight = meta.InitHeight,
                initWidth = meta.InitWidth
            },
            protocol = new
            {
                type = "http",
                httpProtocol = new
                {
                    uris = new[]
                    {
                        new
                        {
                            type = "tableMeta",
                            uri = "/table_meta"
                        },
                        new
                        {
                            type = "records",
                            uri = "/records"
                        }
                    }
                }
            }
        });
    }

    public abstract Task<object> PostTableMetaAsync();

    public abstract Task<object> PostRecordsAsync();
}
