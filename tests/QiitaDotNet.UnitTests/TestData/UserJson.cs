namespace QiitaDotNet.UnitTests.TestData;

/// <summary>
/// User 系テストで共有する JSON データです。
/// </summary>
internal static class UserJson
{
    /// <summary>通常の値を持つユーザー 1 件分の JSON です。</summary>
    internal const string Typical = """
        {
            "description": "Hello, world.",
            "facebook_id": "typical_fb",
            "followees_count": 100,
            "followers_count": 200,
            "github_login_name": "typical_gh",
            "id": "typical",
            "items_count": 300,
            "linkedin_id": "typical_li",
            "location": "Tokyo, Japan",
            "name": "Typical User",
            "organization": "Typical Inc.",
            "permanent_id": 1,
            "profile_image_url": "https://example.com/typical.png",
            "team_only": false,
            "twitter_screen_name": "typical_tw",
            "website_url": "https://example.com/"
        }
        """;

    /// <summary>null 許容の文字列がすべて null のユーザー 1 件分の JSON です。</summary>
    internal const string WithNulls = """
        {
            "description": null,
            "facebook_id": null,
            "followees_count": 100,
            "followers_count": 200,
            "github_login_name": null,
            "id": "with_nulls",
            "items_count": 300,
            "linkedin_id": null,
            "location": null,
            "name": null,
            "organization": null,
            "permanent_id": 2,
            "profile_image_url": "https://example.com/null.png",
            "team_only": false,
            "twitter_screen_name": null,
            "website_url": null
        }
        """;

    /// <summary>
    /// <see cref="Typical"/> と <see cref="WithNulls"/> を連結した、JSON 配列リテラルです。
    /// </summary>
    internal const string UserList = $"[{Typical},{WithNulls}]";
}
