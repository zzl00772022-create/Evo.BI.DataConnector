namespace Evo.BI.Lark;

public sealed class LarkDataConnectorOptions
{
    public const string SectionName = "LarkDataConnector";

    public SignatureOptions Signature { get; set; } = new();

    public MetaOptions Meta { get; set; } = new();
}

public sealed class SignatureOptions
{
    public bool Enabled { get; set; }

    public string SecretKey { get; set; } = "gCmRo2oVVxMs5GloZHctSCE4CBVM5p";

    public int ToleranceMinutes { get; set; } = 5;
}

public sealed class MetaOptions
{
    public string DataSourceConfigUiUri { get; set; } =
        "https://ext.baseopendev.com/ext/data-sync-fe-demo/c70fa2864a002386423f26411f21a3c674bc2f9c/index.html";

    public int InitHeight { get; set; } = 300;

    public int InitWidth { get; set; } = 500;

    public bool DisabledPeriodicSync { get; set; }
}
