using System.Text.Json;
using QiitaDotNet.Models;
using QiitaDotNet.Serialization;
using QiitaDotNet.UnitTests.TestData;

namespace QiitaDotNet.UnitTests.Models;

/// <summary>
/// <see cref="Tag"/> のテストです。
/// </summary>
public class TagTests
{
    [Test]
    public async Task Deserialize_SimpleValues_MapsSnakeCaseProperties()
    {
        var tag = JsonSerializer.Deserialize(
            TagJson.Typical, QiitaJsonSerializerContext.Default.Tag);

        await Assert.That(tag).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(tag!.Id).IsEqualTo("qiita");
            await Assert.That(tag.FollowersCount).IsEqualTo(100);
            await Assert.That(tag.IconUrl).IsEqualTo("https://example.com/tag.png");
            await Assert.That(tag.ItemsCount).IsEqualTo(200);
        }
    }

    [Test]
    public async Task Deserialize_NullIconUrl_MapsToNull()
    {
        var tag = JsonSerializer.Deserialize(
            TagJson.WithNull, QiitaJsonSerializerContext.Default.Tag);

        await Assert.That(tag).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(tag!.Id).IsEqualTo("null_icon");
            await Assert.That(tag.FollowersCount).IsEqualTo(0);
            await Assert.That(tag.IconUrl).IsNull();
            await Assert.That(tag.ItemsCount).IsEqualTo(1);
        }
    }
}
