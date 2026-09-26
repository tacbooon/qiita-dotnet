using System.Net;
using QiitaDotNet.UnitTests.TestData;

namespace QiitaDotNet.UnitTests;

/// <summary>
/// <see cref="ApiTransport"/> 相当の横断関心事のテストです。
/// <see cref="ApiTransport"/> は internal のため、代表として <c>ListUsersAsync</c> 経由で検証します。
/// 新しいサブクライアントやメソッドを追加しても、ここにある項目は重複して検証しません。
/// </summary>
public class ApiTransportTests
{
    [Test]
    public async Task WithoutBaseAddress_FallsBackWithoutMutatingHttpClient()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Users.ListUsersAsync();
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo("https://qiita.com/api/v2/users");
            await Assert.That(httpClient.BaseAddress).IsNull();
        }
    }

    [Test]
    public async Task WithBaseAddress_UsesItForRequests()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        var baseAddress = new Uri("https://team.qiita.com/", UriKind.Absolute);
        using var httpClient = new HttpClient(stub) { BaseAddress = baseAddress, };
        var client = new QiitaClient(httpClient);
        await client.Users.ListUsersAsync();
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo("https://team.qiita.com/api/v2/users");
            await Assert.That(httpClient.BaseAddress).IsEqualTo(baseAddress);
        }
    }

    [Test]
    public async Task SendsAcceptJsonHeader()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Users.ListUsersAsync();
        await Assert.That(stub.LastRequest?.Headers.Accept.ToString())
            .IsEqualTo("application/json");
    }

    [Test]
    public async Task ErrorResponseWithJsonBody_ThrowsQiitaApiException()
    {
        var stub = new StubHandler
        {
            Status = HttpStatusCode.NotFound,
            ResponseBody = ErrorJson.NotFound,
        };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var ex = (await Assert.That(async () => await client.Users.ListUsersAsync())
            .ThrowsExactly<QiitaApiException>())!;
        using (Assert.Multiple())
        {
            await Assert.That(ex.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
            await Assert.That(ex.ErrorType).IsEqualTo("not_found");
            await Assert.That(ex.Message).Contains("Not found");
        }
    }

    [Test]
    public async Task ErrorResponseWithJsonMissingType_ThrowsQiitaApiExceptionWithoutType()
    {
        var stub = new StubHandler
        {
            Status = HttpStatusCode.NotFound,
            ResponseBody = ErrorJson.MissingType,
        };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var ex = (await Assert.That(async () => await client.Users.ListUsersAsync())
            .ThrowsExactly<QiitaApiException>())!;
        using (Assert.Multiple())
        {
            await Assert.That(ex.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
            await Assert.That(ex.ErrorType).IsNull();
            await Assert.That(ex.Message).Contains("Invalid response body");
        }
    }

    [Test]
    public async Task ErrorResponseWithJsonMissingMessage_ThrowsQiitaApiExceptionWithType()
    {
        var stub = new StubHandler
        {
            Status = HttpStatusCode.NotFound,
            ResponseBody = ErrorJson.MissingMessage,
        };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var ex = (await Assert.That(async () => await client.Users.ListUsersAsync())
            .ThrowsExactly<QiitaApiException>())!;
        using (Assert.Multiple())
        {
            await Assert.That(ex.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
            await Assert.That(ex.ErrorType).IsEqualTo("not_found");
            await Assert.That(ex.Message).Contains("Invalid response body");
        }
    }

    [Test]
    public async Task ErrorResponseWithEmptyBody_ThrowsQiitaApiExceptionWithoutType()
    {
        var stub = new StubHandler
        {
            Status = HttpStatusCode.NotFound,
            ResponseBody = string.Empty,
        };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var ex = (await Assert.That(async () => await client.Users.ListUsersAsync())
            .ThrowsExactly<QiitaApiException>())!;
        using (Assert.Multiple())
        {
            await Assert.That(ex.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
            await Assert.That(ex.ErrorType).IsNull();
            await Assert.That(ex.Message).Contains("Empty response body");
        }
    }

    [Test]
    public async Task ErrorResponseWithNullBody_ThrowsQiitaApiExceptionWithoutType()
    {
        var stub = new StubHandler
        {
            Status = HttpStatusCode.NotFound,
            ResponseBody = "null",
        };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var ex = (await Assert.That(async () => await client.Users.ListUsersAsync())
            .ThrowsExactly<QiitaApiException>())!;
        using (Assert.Multiple())
        {
            await Assert.That(ex.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
            await Assert.That(ex.ErrorType).IsNull();
            await Assert.That(ex.Message).Contains("Invalid response body");
        }
    }

    [Test]
    public async Task ErrorResponseWithHtmlBody_ThrowsQiitaApiExceptionWithoutType()
    {
        var stub = new StubHandler
        {
            Status = HttpStatusCode.InternalServerError,
            ResponseBody = "<html>unexpected</html>",
        };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var ex = (await Assert.That(async () => await client.Users.ListUsersAsync())
            .ThrowsExactly<QiitaApiException>())!;
        using (Assert.Multiple())
        {
            await Assert.That(ex.StatusCode).IsEqualTo(HttpStatusCode.InternalServerError);
            await Assert.That(ex.ErrorType).IsNull();
            await Assert.That(ex.Message).Contains("unexpected");
        }
    }

    [Test]
    public async Task ErrorResponseWithHugeHtmlBody_ThrowsQiitaApiExceptionWithTruncatedMessage()
    {
        var stub = new StubHandler
        {
            Status = HttpStatusCode.BadGateway,
            ResponseBody = "<html>" + new string('x', 5000) + "</html>",
        };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var ex = (await Assert.That(async () => await client.Users.ListUsersAsync())
            .ThrowsExactly<QiitaApiException>())!;
        using (Assert.Multiple())
        {
            await Assert.That(ex.Message).EndsWith("...");
            await Assert.That(ex.Message.Length).IsLessThan(256);
        }
    }

    [Test]
    [Timeout(5000)]
    public async Task ResponseBodyReadCancellation_ThrowsTaskCanceledException(CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient(new HangingErrorHandler());
        var client = new QiitaClient(httpClient);
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMilliseconds(200));
        await Assert.That(async () => await client.Users.ListUsersAsync(cancellationToken: cts.Token))
            .ThrowsExactly<TaskCanceledException>();
    }

    private sealed class HangingErrorHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new SlowContent(),
                RequestMessage = request,
            });
        }
    }

    private sealed class SlowContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            return Task.Delay(Timeout.InfiniteTimeSpan);
        }
        protected override async Task SerializeToStreamAsync(
            Stream stream,
            TransportContext? context,
            CancellationToken cancellationToken)
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
