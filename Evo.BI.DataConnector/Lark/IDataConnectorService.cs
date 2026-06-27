namespace Evo.BI.Lark;

public interface IDataConnectorService
{
    Task<object> GetMetaAsync();

    Task<object> PostTableMetaAsync();

    Task<object> PostRecordsAsync();
}
