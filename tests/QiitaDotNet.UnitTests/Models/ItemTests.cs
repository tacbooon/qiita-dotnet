using System.Text.Json;
using QiitaDotNet.Models;
using QiitaDotNet.Serialization;
using QiitaDotNet.UnitTests.TestData;

namespace QiitaDotNet.UnitTests.Models;

/// <summary>
/// <see cref="Item"/> のテストです。
/// </summary>
public class ItemTests
{
    [Test]
    public async Task Deserialize_SimpleValues_MapsSnakeCaseProperties()
    {
        var item = JsonSerializer.Deserialize(
            ItemJson.Typical, QiitaJsonSerializerContext.Default.Item);

        await Assert.That(item).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(item!.RenderedBody).IsEqualTo("<h1>Rendered Typical</h1>");
            await Assert.That(item.Body).IsEqualTo("# Markdown Typical");
            await Assert.That(item.Coediting).IsTrue();
            await Assert.That(item.CommentsCount).IsEqualTo(101);
            await Assert.That(item.CreatedAt).IsEqualTo(new DateTimeOffset(2001, 2, 3, 4, 5, 6, TimeSpan.FromHours(9)));
            await Assert.That(item.Id).IsEqualTo("c686397e4a0f4f11683d");
            await Assert.That(item.LikesCount).IsEqualTo(102);
            await Assert.That(item.Private).IsFalse();
            await Assert.That(item.ReactionsCount).IsEqualTo(103);
            await Assert.That(item.StocksCount).IsEqualTo(104);
            await Assert.That(item.Title).IsEqualTo("Typical item title");
            await Assert.That(item.UpdatedAt).IsEqualTo(new DateTimeOffset(2004, 5, 6, 7, 8, 9, TimeSpan.FromHours(9)));
            await Assert.That(item.Url).IsEqualTo("https://qiita.com/typical_user/items/c686397e4a0f4f11683d");
            await Assert.That(item.PageViewsCount).IsEqualTo(105);
            await Assert.That(item.OrganizationUrlName).IsEqualTo("typical-organization");
            await Assert.That(item.Slide).IsTrue();
            await Assert.That(item.PostingCampaignUuid).IsEqualTo("a486397e4a0f4f11683d");
            await Assert.That(item.Group).IsNotNull();
            await Assert.That(item.Group!.CreatedAt).IsEqualTo(new DateTimeOffset(2002, 3, 4, 5, 6, 7, TimeSpan.Zero));
            await Assert.That(item.Group.Description).IsEqualTo("Typical group description.");
            await Assert.That(item.Group.Name).IsEqualTo("Typical Group");
            await Assert.That(item.Group.Private).IsTrue();
            await Assert.That(item.Group.UpdatedAt).IsEqualTo(new DateTimeOffset(2003, 4, 5, 6, 7, 8, TimeSpan.Zero));
            await Assert.That(item.Group.UrlName).IsEqualTo("typical-group");
            await Assert.That(item.Tags).Count().IsEqualTo(2);
            await Assert.That(item.Tags[0].Name).IsEqualTo("Ruby");
            await Assert.That(item.Tags[0].Versions).IsEquivalentTo(["0.0.1", "0.0.2"]);
            await Assert.That(item.Tags[1].Name).IsEqualTo("Python");
            await Assert.That(item.Tags[1].Versions).IsEmpty();
            await Assert.That(item.User.Description).IsEqualTo("Typical user description.");
            await Assert.That(item.User.FacebookId).IsEqualTo("typical_facebook_id");
            await Assert.That(item.User.FolloweesCount).IsEqualTo(201);
            await Assert.That(item.User.FollowersCount).IsEqualTo(202);
            await Assert.That(item.User.GithubLoginName).IsEqualTo("typical_github_name");
            await Assert.That(item.User.Id).IsEqualTo("typical_user");
            await Assert.That(item.User.ItemsCount).IsEqualTo(203);
            await Assert.That(item.User.LinkedinId).IsEqualTo("typical_linkedin_id");
            await Assert.That(item.User.Location).IsEqualTo("Osaka, Japan");
            await Assert.That(item.User.Name).IsEqualTo("Typical User");
            await Assert.That(item.User.Organization).IsEqualTo("Typical Organization");
            await Assert.That(item.User.PermanentId).IsEqualTo(204);
            await Assert.That(item.User.ProfileImageUrl).IsEqualTo("https://example.com/typical_user.png");
            await Assert.That(item.User.TeamOnly).IsFalse();
            await Assert.That(item.User.TwitterScreenName).IsEqualTo("typical_twitter");
            await Assert.That(item.User.WebsiteUrl).IsEqualTo("https://example.com/typical_user");
            await Assert.That(item.TeamMembership).IsNotNull();
            await Assert.That(item.TeamMembership!.Name).IsEqualTo("Typical Team Member");
        }
    }

    [Test]
    public async Task Deserialize_NullValues_MapsToNull()
    {
        var item = JsonSerializer.Deserialize(
            ItemJson.WithNulls, QiitaJsonSerializerContext.Default.Item);

        await Assert.That(item).IsNotNull();
        using (Assert.Multiple())
        {
            await Assert.That(item!.RenderedBody).IsEqualTo("<h1>Rendered Nulls</h1>");
            await Assert.That(item.Body).IsEqualTo("# Markdown Nulls");
            await Assert.That(item.Coediting).IsFalse();
            await Assert.That(item.CommentsCount).IsEqualTo(1);
            await Assert.That(item.CreatedAt).IsEqualTo(new DateTimeOffset(2011, 12, 13, 14, 15, 16, TimeSpan.FromHours(9)));
            await Assert.That(item.Group).IsNull();
            await Assert.That(item.Id).IsEqualTo("b486397e4a0f4f11683e");
            await Assert.That(item.LikesCount).IsEqualTo(2);
            await Assert.That(item.Private).IsTrue();
            await Assert.That(item.ReactionsCount).IsEqualTo(3);
            await Assert.That(item.StocksCount).IsEqualTo(4);
            await Assert.That(item.Tags).IsEmpty();
            await Assert.That(item.Title).IsEqualTo("Nulls item title");
            await Assert.That(item.UpdatedAt).IsEqualTo(new DateTimeOffset(2014, 5, 16, 17, 18, 19, TimeSpan.Zero));
            await Assert.That(item.Url).IsEqualTo("https://qiita.com/nulls_user/items/b486397e4a0f4f11683e");
            await Assert.That(item.PageViewsCount).IsNull();
            await Assert.That(item.TeamMembership).IsNull();
            await Assert.That(item.OrganizationUrlName).IsNull();
            await Assert.That(item.Slide).IsFalse();
            await Assert.That(item.PostingCampaignUuid).IsNull();
            await Assert.That(item.User.Description).IsNull();
            await Assert.That(item.User.FacebookId).IsNull();
            await Assert.That(item.User.FolloweesCount).IsEqualTo(5);
            await Assert.That(item.User.FollowersCount).IsEqualTo(6);
            await Assert.That(item.User.GithubLoginName).IsNull();
            await Assert.That(item.User.Id).IsEqualTo("nulls_user");
            await Assert.That(item.User.ItemsCount).IsEqualTo(7);
            await Assert.That(item.User.LinkedinId).IsNull();
            await Assert.That(item.User.Location).IsNull();
            await Assert.That(item.User.Name).IsNull();
            await Assert.That(item.User.Organization).IsNull();
            await Assert.That(item.User.PermanentId).IsEqualTo(8);
            await Assert.That(item.User.ProfileImageUrl).IsEqualTo("https://example.com/nulls_user.png");
            await Assert.That(item.User.TeamOnly).IsTrue();
            await Assert.That(item.User.TwitterScreenName).IsNull();
            await Assert.That(item.User.WebsiteUrl).IsNull();
        }
    }
}
