namespace QiitaDotNet.UnitTests.TestData;

/// <summary>
/// AuthenticatedUser 系テストで共有する JSON データです。
/// </summary>
internal static class AuthenticatedUserJson
{
    /// <summary>通常の値を持つ認証中ユーザー 1 件分の JSON です。</summary>
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
            "website_url": "https://example.com/",
            "image_monthly_upload_limit": 1048576,
            "image_monthly_upload_remaining": 524288
        }
        """;
}
