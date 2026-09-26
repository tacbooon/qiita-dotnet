using System.Text.Json;
using QiitaDotNet.Models;
using QiitaDotNet.Serialization;
using QiitaDotNet.UnitTests.TestData;

namespace QiitaDotNet.UnitTests.Models;

/// <summary>
/// <see cref="User"/> のテストです。
/// </summary>
public class UserTests
{
    [Test]
    public async Task Deserialize_SimpleValues_MapsSnakeCaseProperties()
    {
        var user = JsonSerializer.Deserialize(
            UserJson.Typical, QiitaJsonSerializerContext.Default.User);

        await Assert.That(user).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(user!.Id).IsEqualTo("typical");
            await Assert.That(user.Description).IsEqualTo("Hello, world.");
            await Assert.That(user.FacebookId).IsEqualTo("typical_fb");
            await Assert.That(user.FolloweesCount).IsEqualTo(100);
            await Assert.That(user.FollowersCount).IsEqualTo(200);
            await Assert.That(user.GithubLoginName).IsEqualTo("typical_gh");
            await Assert.That(user.ItemsCount).IsEqualTo(300);
            await Assert.That(user.LinkedinId).IsEqualTo("typical_li");
            await Assert.That(user.Location).IsEqualTo("Tokyo, Japan");
            await Assert.That(user.Name).IsEqualTo("Typical User");
            await Assert.That(user.Organization).IsEqualTo("Typical Inc.");
            await Assert.That(user.PermanentId).IsEqualTo(1);
            await Assert.That(user.ProfileImageUrl).IsEqualTo("https://example.com/typical.png");
            await Assert.That(user.TeamOnly).IsFalse();
            await Assert.That(user.TwitterScreenName).IsEqualTo("typical_tw");
            await Assert.That(user.WebsiteUrl).IsEqualTo("https://example.com/");
        }
    }

    [Test]
    public async Task Deserialize_NullValues_MapsToNull()
    {
        var user = JsonSerializer.Deserialize(
            UserJson.WithNulls, QiitaJsonSerializerContext.Default.User);

        await Assert.That(user).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(user!.Id).IsEqualTo("with_nulls");
            await Assert.That(user.Description).IsNull();
            await Assert.That(user.FacebookId).IsNull();
            await Assert.That(user.FolloweesCount).IsEqualTo(100);
            await Assert.That(user.FollowersCount).IsEqualTo(200);
            await Assert.That(user.GithubLoginName).IsNull();
            await Assert.That(user.ItemsCount).IsEqualTo(300);
            await Assert.That(user.LinkedinId).IsNull();
            await Assert.That(user.Location).IsNull();
            await Assert.That(user.Name).IsNull();
            await Assert.That(user.Organization).IsNull();
            await Assert.That(user.PermanentId).IsEqualTo(2);
            await Assert.That(user.ProfileImageUrl).IsEqualTo("https://example.com/null.png");
            await Assert.That(user.TeamOnly).IsFalse();
            await Assert.That(user.TwitterScreenName).IsNull();
            await Assert.That(user.WebsiteUrl).IsNull();
        }
    }
}
