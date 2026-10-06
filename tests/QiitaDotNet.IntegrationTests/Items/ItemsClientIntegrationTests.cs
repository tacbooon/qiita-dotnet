using TUnit.Core;

namespace QiitaDotNet.IntegrationTests.Items;

/// <summary>
/// <see cref="QiitaDotNet.Items.IItemsClient"/> の統合テストです。
/// </summary>
[ClassDataSource<QiitaHttpClientFixture>(Shared = SharedType.PerTestSession)]
public class ItemsClientIntegrationTests(QiitaHttpClientFixture fixture)
{
    [Test]
    public async Task ListAuthenticatedUserItemsAsync_ReturnsItems(CancellationToken ct)
    {
        Skip.When(
            string.IsNullOrWhiteSpace(
                Environment.GetEnvironmentVariable(QiitaHttpClientFixture.AccessTokenEnvironmentVariableName)),
            $"Environment variable '{QiitaHttpClientFixture.AccessTokenEnvironmentVariableName}' is not set.");

        var result = await fixture.Client.Items.ListAuthenticatedUserItemsAsync(perPage: 2, page: 1, cancellationToken: ct);
        using (Assert.Multiple())
        {
            // テスト実行ユーザーが 2 件以上の記事を投稿していることが前提です。
            await Assert.That(result.Count).IsEqualTo(2);
            await Assert.That(result[0].Id).IsNotEmpty();
            await Assert.That(result[0].Title).IsNotEmpty();
            await Assert.That(result[1].Id).IsNotEmpty();
            await Assert.That(result[1].Title).IsNotEmpty();
            await Assert.That(result[0].CreatedAt).IsGreaterThanOrEqualTo(result[1].CreatedAt);
        }
    }
}
