using Evo.BI.Lark;
using Evo.BI.Lark.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<LarkDataConnectorOptions>(
    builder.Configuration.GetSection(LarkDataConnectorOptions.SectionName));

builder.Services.AddScoped<OrderDataConnectorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<LarkDataConnectorSignatureMiddleware>();
app.MapGet("/", () => Results.Ok(new
{
    name = "Evo.BI.DataConnector",
    status = "Running",
    endpoints = new[]
    {
        "/lark/order/meta.json",
        "/lark/order/table_meta",
        "/lark/order/records",
        "/openapi/v1.json"
    }
}));
app.MapControllers();

app.Run();
