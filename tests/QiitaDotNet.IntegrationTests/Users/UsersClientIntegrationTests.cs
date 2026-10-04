using TUnit.Core;

namespace QiitaDotNet.IntegrationTests.Users;

/// <summary>
/// <see cref="QiitaDotNet.Users.IUsersClient"/> の統合テストです。
/// </summary>
[ClassDataSource<QiitaHttpClientFixture>(Shared = SharedType.PerTestSession)]
public class UsersClientIntegrationTests(QiitaHttpClientFixture fixture)
{
    [Test]
    public async Task ListUsersAsync_ReturnsUsers(CancellationToken ct)
    {
        var result = await fixture.Client.Users.ListUsersAsync(perPage: 1, page: 2, cancellationToken: ct);
        using (Assert.Multiple())
        {
            await Assert.That(result).Count().IsEqualTo(1);
            await Assert.That(result[0].Id).IsNotEmpty();
            await Assert.That(result[0].PermanentId).IsGreaterThan(0);
        }
    }

    [Test]
    public async Task GetUserAsync_ReturnsUser(CancellationToken ct)
    {
        var result = await fixture.Client.Users.GetUserAsync("tacbooon", cancellationToken: ct);
        using (Assert.Multiple())
        {
            await Assert.That(result.Id).IsEqualTo("tacbooon");
            await Assert.That(result.PermanentId).IsEqualTo(277357);
        }
    }

    [Test]
    public async Task GetAuthenticatedUserAsync_ReturnsAuthenticatedUser(CancellationToken ct)
    {
        Skip.When(
            string.IsNullOrWhiteSpace(
                Environment.GetEnvironmentVariable(QiitaHttpClientFixture.AccessTokenEnvironmentVariableName)),
            $"Environment variable '{QiitaHttpClientFixture.AccessTokenEnvironmentVariableName}' is not set.");

        var result = await fixture.Client.Users.GetAuthenticatedUserAsync(cancellationToken: ct);
        using (Assert.Multiple())
        {
            await Assert.That(result.Id).IsNotEmpty();
            await Assert.That(result.PermanentId).IsGreaterThan(0);
            await Assert.That(result.ImageMonthlyUploadLimit).IsGreaterThanOrEqualTo(0);
            await Assert.That(result.ImageMonthlyUploadRemaining).IsGreaterThanOrEqualTo(0);
        }
    }
}
