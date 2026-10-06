namespace QiitaDotNet.UnitTests.TestData;

/// <summary>
/// Item 系テストで共有する JSON データです。
/// </summary>
/// <remarks>
/// <para>フィールドの取り違えを検出できるよう、同じ型の値は1つずつ異なる値にしています。</para>
/// </remarks>
internal static class ItemJson
{
    /// <summary>通常の値を持つ記事 1 件分の JSON です。</summary>
    internal const string Typical = """
        {
            "rendered_body": "<h1>Rendered Typical</h1>",
            "body": "# Markdown Typical",
            "coediting": true,
            "comments_count": 101,
            "created_at": "2001-02-03T04:05:06+09:00",
            "group": {
                "created_at": "2002-03-04T05:06:07+00:00",
                "description": "Typical group description.",
                "name": "Typical Group",
                "private": true,
                "updated_at": "2003-04-05T06:07:08+00:00",
                "url_name": "typical-group"
            },
            "id": "c686397e4a0f4f11683d",
            "likes_count": 102,
            "private": false,
            "reactions_count": 103,
            "stocks_count": 104,
            "tags": [
                {
                    "name": "Ruby",
                    "versions": ["0.0.1", "0.0.2"]
                },
                {
                    "name": "Python",
                    "versions": []
                }
            ],
            "title": "Typical item title",
            "updated_at": "2004-05-06T07:08:09+09:00",
            "url": "https://qiita.com/typical_user/items/c686397e4a0f4f11683d",
            "user": {
                "description": "Typical user description.",
                "facebook_id": "typical_facebook_id",
                "followees_count": 201,
                "followers_count": 202,
                "github_login_name": "typical_github_name",
                "id": "typical_user",
                "items_count": 203,
                "linkedin_id": "typical_linkedin_id",
                "location": "Osaka, Japan",
                "name": "Typical User",
                "organization": "Typical Organization",
                "permanent_id": 204,
                "profile_image_url": "https://example.com/typical_user.png",
                "team_only": false,
                "twitter_screen_name": "typical_twitter",
                "website_url": "https://example.com/typical_user"
            },
            "page_views_count": 105,
            "team_membership": {
                "name": "Typical Team Member"
            },
            "organization_url_name": "typical-organization",
            "slide": true,
            "posting_campaign_uuid": "a486397e4a0f4f11683d"
        }
        """;

    /// <summary>null 許容のプロパティが null の記事 1 件分の JSON です。</summary>
    internal const string WithNulls = """
        {
            "rendered_body": "<h1>Rendered Nulls</h1>",
            "body": "# Markdown Nulls",
            "coediting": false,
            "comments_count": 1,
            "created_at": "2011-12-13T14:15:16+09:00",
            "group": null,
            "id": "b486397e4a0f4f11683e",
            "likes_count": 2,
            "private": true,
            "reactions_count": 3,
            "stocks_count": 4,
            "tags": [],
            "title": "Nulls item title",
            "updated_at": "2014-05-16T17:18:19+00:00",
            "url": "https://qiita.com/nulls_user/items/b486397e4a0f4f11683e",
            "user": {
                "description": null,
                "facebook_id": null,
                "followees_count": 5,
                "followers_count": 6,
                "github_login_name": null,
                "id": "nulls_user",
                "items_count": 7,
                "linkedin_id": null,
                "location": null,
                "name": null,
                "organization": null,
                "permanent_id": 8,
                "profile_image_url": "https://example.com/nulls_user.png",
                "team_only": true,
                "twitter_screen_name": null,
                "website_url": null
            },
            "page_views_count": null,
            "team_membership": null,
            "organization_url_name": null,
            "slide": false,
            "posting_campaign_uuid": null
        }
        """;

    /// <summary>
    /// <see cref="Typical"/> と <see cref="WithNulls"/> を連結した、JSON 配列リテラルです。
    /// </summary>
    internal const string ItemList = $"[{Typical},{WithNulls}]";
}
