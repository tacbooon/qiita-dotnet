using System.Text.Json;
using QiitaDotNet.Models;
using QiitaDotNet.Serialization;
using QiitaDotNet.UnitTests.TestData;

namespace QiitaDotNet.UnitTests.Models;

/// <summary>
/// <see cref="AuthenticatedUser"/> のテストです。
/// </summary>
public class AuthenticatedUserTests
{
    [Test]
    public async Task Deserialize_SimpleValues_MapsSnakeCaseProperties()
    {
        var user = JsonSerializer.Deserialize(
            AuthenticatedUserJson.Typical, QiitaJsonSerializerContext.Default.AuthenticatedUser);

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
            await Assert.That(user.ImageMonthlyUploadLimit).IsEqualTo(1048576);
            await Assert.That(user.ImageMonthlyUploadRemaining).IsEqualTo(524288);
        }
    }

    [Test]
    public async Task AuthenticatedUser_IsAssignableToUser()
    {
        var user = JsonSerializer.Deserialize(
            AuthenticatedUserJson.Typical, QiitaJsonSerializerContext.Default.AuthenticatedUser);

        await Assert.That(user).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(user is User).IsTrue();
            await Assert.That(user!.GetType()).IsEqualTo(typeof(AuthenticatedUser));
            User upcast = user;
            await Assert.That(upcast.Id).IsEqualTo("typical");
        }
    }

    [Test]
    public async Task ToUser_ReturnsSlicedUserCopy()
    {
        var authenticatedUser = JsonSerializer.Deserialize(
            AuthenticatedUserJson.Typical, QiitaJsonSerializerContext.Default.AuthenticatedUser);

        await Assert.That(authenticatedUser).IsNotNull();
        var user = authenticatedUser!.ToUser();
        using (Assert.Multiple())
        {
            await Assert.That(user.GetType()).IsEqualTo(typeof(User));
            await Assert.That(user.Id).IsEqualTo(authenticatedUser.Id);
        }
    }
}
