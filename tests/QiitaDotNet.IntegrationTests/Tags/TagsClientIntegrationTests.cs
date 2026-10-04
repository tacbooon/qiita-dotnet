using QiitaDotNet.Tags;

namespace QiitaDotNet.IntegrationTests.Tags;

/// <summary>
/// <see cref="QiitaDotNet.Tags.ITagsClient"/> の統合テストです。
/// </summary>
[ClassDataSource<QiitaHttpClientFixture>(Shared = SharedType.PerTestSession)]
public class TagsClientIntegrationTests(QiitaHttpClientFixture fixture)
{
    [Test]
    public async Task ListTagsAsync_ReturnsTags(CancellationToken ct)
    {
        var result = await fixture.Client.Tags.ListTagsAsync(perPage: 1, page: 2, cancellationToken: ct);
        using (Assert.Multiple())
        {
            await Assert.That(result).Count().IsEqualTo(1);
            await Assert.That(result[0].Id).IsNotEmpty();
        }
    }

    [Test]
    public async Task ListTagsAsync_WithSortCount_ReturnsTags(CancellationToken ct)
    {
        var result = await fixture.Client.Tags.ListTagsAsync(
            perPage: 2, sort: TagSort.Count, cancellationToken: ct);
        using (Assert.Multiple())
        {
            await Assert.That(result).Count().IsEqualTo(2);
            await Assert.That(result[0].Id).IsNotEmpty();
            await Assert.That(result[1].Id).IsNotEmpty();
            await Assert.That(result[0].ItemsCount).IsGreaterThanOrEqualTo(result[1].ItemsCount);
        }
    }

    [Test]
    public async Task ListTagsAsync_WithSortName_ReturnsTags(CancellationToken ct)
    {
        var result = await fixture.Client.Tags.ListTagsAsync(
            perPage: 2, sort: TagSort.Name, cancellationToken: ct);
        using (Assert.Multiple())
        {
            await Assert.That(result).Count().IsEqualTo(2);
            await Assert.That(result[0].Id).IsNotEmpty();
            await Assert.That(result[1].Id).IsNotEmpty();
            await Assert.That(result[0].Id).IsLessThan(result[1].Id);
        }
    }

    [Test]
    public async Task GetTagAsync_ReturnsTag(CancellationToken ct)
    {
        var result = await fixture.Client.Tags.GetTagAsync("qiita", cancellationToken: ct);
        using (Assert.Multiple())
        {
            await Assert.That(result.Id.Equals("qiita", StringComparison.OrdinalIgnoreCase)).IsTrue();
            await Assert.That(result.FollowersCount).IsGreaterThanOrEqualTo(0);
            await Assert.That(result.ItemsCount).IsGreaterThanOrEqualTo(0);
        }
    }
}
