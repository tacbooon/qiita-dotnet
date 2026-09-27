using System.Net;
using QiitaDotNet.Authentication;

namespace QiitaDotNet.IntegrationTests.Authentication;

/// <summary>
/// 認証に関する結合テストです。
/// </summary>
public class AuthenticationIntegrationTests
{
    [Test]
    public async Task ListUsersAsync_WithInvalidToken_ThrowsQiitaApiException(CancellationToken ct)
    {
        using var httpClient = new HttpClient(
            new QiitaAccessTokenHandler("invalid-access-token", new HttpClientHandler()));
        var client = new QiitaClient(httpClient);
        var ex = (await Assert.That(async () => await client.Users.ListUsersAsync(cancellationToken: ct))
            .ThrowsExactly<QiitaApiException>())!;
        using (Assert.Multiple())
        {
            await Assert.That(ex.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
            await Assert.That(ex.ErrorType).IsEqualTo("unauthorized");
        }
    }
}
