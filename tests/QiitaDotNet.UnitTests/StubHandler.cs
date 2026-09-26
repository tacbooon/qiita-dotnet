using System.Net;
using System.Text;

namespace QiitaDotNet.UnitTests;

/// <summary>
/// テスト用のスタブハンドラーです。サーバーにアクセスしません。
/// </summary>
internal sealed class StubHandler : HttpMessageHandler
{
    internal string ResponseBody { get; init; } = "null";
    internal HttpStatusCode Status { get; init; } = HttpStatusCode.OK;
    internal HttpRequestMessage? LastRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        LastRequest = request;
        return Task.FromResult(new HttpResponseMessage(Status)
        {
            Content = new StringContent(ResponseBody, Encoding.UTF8, "application/json"),
            RequestMessage = request,
        });
    }
}
