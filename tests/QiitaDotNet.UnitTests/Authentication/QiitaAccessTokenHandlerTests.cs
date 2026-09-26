using QiitaDotNet.Authentication;

namespace QiitaDotNet.UnitTests.Authentication;

/// <summary>
/// <see cref="QiitaAccessTokenHandler"/> のテストです。
/// </summary>
public class QiitaAccessTokenHandlerTests
{
    [Test]
    [Arguments("https://qiita.com/", DisplayName = "qiita.com")]
    [Arguments("https://team.qiita.com/", DisplayName = "team.qiita.com")]
    public async Task SendAsync_SingleArgConstructor_AddsBearerAuthorizationHeader(string baseAddress)
    {
        var stub = new StubHandler();
        var handler = new QiitaAccessTokenHandler("tok123") { InnerHandler = stub };
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseAddress, UriKind.Absolute),
        };
        await httpClient.GetAsync("api/v2/users");
        await Assert.That(stub.LastRequest?.Headers.Authorization?.ToString())
            .IsEqualTo("Bearer tok123");
    }

    [Test]
    [Arguments("https://qiita.com/", DisplayName = "qiita.com")]
    [Arguments("https://team.qiita.com/", DisplayName = "team.qiita.com")]
    public async Task SendAsync_TwoArgConstructor_AddsBearerAuthorizationHeader(string baseAddress)
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(new QiitaAccessTokenHandler("tok123", stub))
        {
            BaseAddress = new Uri(baseAddress, UriKind.Absolute),
        };
        await httpClient.GetAsync("api/v2/users");
        await Assert.That(stub.LastRequest?.Headers.Authorization?.ToString())
            .IsEqualTo("Bearer tok123");
    }

    [Test]
    public async Task SendAsync_PreservesExistingAuthorizationHeader()
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(new PreAuthHandler(new QiitaAccessTokenHandler("tok123", stub)));
        await httpClient.GetAsync("https://qiita.com/api/v2/users");
        await Assert.That(stub.LastRequest?.Headers.Authorization?.ToString())
            .IsEqualTo("Bearer existing");
    }

    [Test]
    [Arguments("https://example.com/api/v2/users", DisplayName = "unrelated host")]
    [Arguments("https://evil-qiita.com/api/v2/users", DisplayName = "similar host")]
    [Arguments("https://qiita.com.evil.com/api/v2/users", DisplayName = "suffix spoofing")]
    public async Task SendAsync_UntrustedHost_ThrowsInvalidOperationException(string url)
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(new QiitaAccessTokenHandler("tok123", stub));
        await Assert.That(async () => await httpClient.GetAsync(url))
            .ThrowsExactly<InvalidOperationException>();
        await Assert.That(stub.LastRequest).IsNull();
    }

    [Test]
    public async Task SendAsync_AllowCustomHosts_SendsToUntrustedHost()
    {
        var stub = new StubHandler();
        var handler = new QiitaAccessTokenHandler("tok123", stub) { AllowCustomHosts = true };
        using var httpClient = new HttpClient(handler);
        await httpClient.GetAsync("https://example.com/api/v2/users");
        await Assert.That(stub.LastRequest?.Headers.Authorization?.ToString())
            .IsEqualTo("Bearer tok123");
    }

    [Test]
    public async Task SendAsync_InsecureScheme_ThrowsInvalidOperationException()
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(new QiitaAccessTokenHandler("tok123", stub));
        await Assert.That(async () => await httpClient.GetAsync("http://qiita.com/api/v2/users"))
            .ThrowsExactly<InvalidOperationException>();
        await Assert.That(stub.LastRequest).IsNull();
    }

    [Test]
    public async Task SendAsync_AllowInsecureScheme_SendsOverHttp()
    {
        var stub = new StubHandler();
        var handler = new QiitaAccessTokenHandler("tok123", stub) { AllowInsecureScheme = true };
        using var httpClient = new HttpClient(handler);
        await httpClient.GetAsync("http://qiita.com/api/v2/users");
        await Assert.That(stub.LastRequest?.Headers.Authorization?.ToString())
            .IsEqualTo("Bearer tok123");
    }

    [Test]
    public async Task SendAsync_NullRequestUri_ThrowsInvalidOperationException()
    {
        var stub = new StubHandler();
        var handler = new QiitaAccessTokenHandler("tok123", stub);
        using var invoker = new HttpMessageInvoker(handler);
        await Assert.That(async () => await invoker.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, (Uri?)null),
                CancellationToken.None))
            .ThrowsExactly<InvalidOperationException>();
        await Assert.That(stub.LastRequest).IsNull();
    }

    [Test]
    public async Task Constructor_NullAccessToken_ThrowsArgumentNullException()
    {
        await Assert.That(() => new QiitaAccessTokenHandler(null!))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName("accessToken");
    }

    [Test]
    [Arguments("", DisplayName = "empty")]
    [Arguments("  ", DisplayName = "whitespace")]
    public async Task Constructor_EmptyOrWhiteSpaceAccessToken_ThrowsArgumentException(string accessToken)
    {
        await Assert.That(() => new QiitaAccessTokenHandler(accessToken))
            .ThrowsExactly<ArgumentException>()
            .WithParameterName("accessToken");
    }

    [Test]
    public async Task Constructor_NullInnerHandler_ThrowsArgumentNullException()
    {
        await Assert.That(() => new QiitaAccessTokenHandler("tok123", null!))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName("innerHandler");
    }

    private sealed class PreAuthHandler : DelegatingHandler
    {
        public PreAuthHandler(HttpMessageHandler inner)
            : base(inner)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "existing");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
