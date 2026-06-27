using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Evo.BI.Lark.Authorization;

public sealed class LarkDataConnectorSignatureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly LarkDataConnectorOptions _options;

    public LarkDataConnectorSignatureMiddleware(
        RequestDelegate next,
        IOptions<LarkDataConnectorOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_options.Signature.Enabled ||
            !context.Request.Path.StartsWithSegments("/lark", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var timestamp = context.Request.Headers["X-Base-Request-Timestamp"].ToString();
        var nonce = context.Request.Headers["X-Base-Request-Nonce"].ToString();
        var signature = context.Request.Headers["X-Base-Signature"].ToString();

        if (string.IsNullOrWhiteSpace(signature))
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "签名缺失 (Signature missing)");
            return;
        }

        if (!IsTimestampValid(timestamp, _options.Signature.ToleranceMinutes))
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "请求已过期，请重新发起请求");
            return;
        }

        var body = await ReadRequestBodyAsync(context.Request);
        var isValid = ValidateSignature(timestamp, nonce, _options.Signature.SecretKey, body, signature);

        if (!isValid)
        {
            await WriteErrorAsync(context, StatusCodes.Status401Unauthorized, "签名校验失败 (Invalid signature)");
            return;
        }

        await _next(context);
    }

    private static bool ValidateSignature(string timestamp, string nonce, string secretKey, string body, string signature)
    {
        var combinedString = timestamp + nonce + secretKey + body;
        var inputBytes = Encoding.UTF8.GetBytes(combinedString);
        var hashBytes = SHA1.HashData(inputBytes);
        var computedSignature = Convert.ToHexString(hashBytes).ToLowerInvariant();

        return signature.Equals(computedSignature, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsTimestampValid(string timestamp, int toleranceMinutes)
    {
        if (string.IsNullOrWhiteSpace(timestamp))
        {
            return false;
        }

        const string format = "dddd, dd-MMM-yy HH:mm:ss 'CST'";
        if (!DateTime.TryParseExact(
                timestamp,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var requestTime))
        {
            return false;
        }

        var diff = DateTime.Now - requestTime;
        return Math.Abs(diff.TotalMinutes) <= toleranceMinutes;
    }

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        return body;
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync(message);
    }
}
