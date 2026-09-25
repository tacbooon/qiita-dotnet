using System.Text.Json.Serialization;

namespace QiitaDotNet.Models;

/// <summary>
/// Qiita ユーザーです。
/// </summary>
public sealed record User
{
    /// <summary>自己紹介文です。</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Facebook ID です。</summary>
    [JsonPropertyName("facebook_id")]
    public string? FacebookId { get; init; }

    /// <summary>フォロー中のユーザー数です。</summary>
    [JsonPropertyName("followees_count")]
    public int FolloweesCount { get; init; }

    /// <summary>フォロワーの数です。</summary>
    [JsonPropertyName("followers_count")]
    public int FollowersCount { get; init; }

    /// <summary>GitHub のログイン名です。</summary>
    [JsonPropertyName("github_login_name")]
    public string? GithubLoginName { get; init; }

    /// <summary>ユーザー ID です。Qiita の仕様上、この ID は変更可能です。</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>qiita.com 上の投稿記事数です。Qiita Team の記事は含みません。</summary>
    [JsonPropertyName("items_count")]
    public int ItemsCount { get; init; }

    /// <summary>LinkedIn ID です。</summary>
    [JsonPropertyName("linkedin_id")]
    public string? LinkedinId { get; init; }

    /// <summary>居住地です。</summary>
    [JsonPropertyName("location")]
    public string? Location { get; init; }

    /// <summary>表示名です。</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>所属組織名です。</summary>
    [JsonPropertyName("organization")]
    public string? Organization { get; init; }

    /// <summary>永続的なユーザー ID です。<see cref="Id"/> を変更してもこの値は変わりません。</summary>
    [JsonPropertyName("permanent_id")]
    public int PermanentId { get; init; }

    /// <summary>プロフィール画像の URL です。</summary>
    [JsonPropertyName("profile_image_url")]
    public required string ProfileImageUrl { get; init; }

    /// <summary>チーム専用ユーザーとして設定されているかどうかです。</summary>
    [JsonPropertyName("team_only")]
    public bool TeamOnly { get; init; }

    /// <summary>X (旧 Twitter) のスクリーンネームです。</summary>
    [JsonPropertyName("twitter_screen_name")]
    public string? TwitterScreenName { get; init; }

    /// <summary>Web サイトの URL です。</summary>
    [JsonPropertyName("website_url")]
    public string? WebsiteUrl { get; init; }
}
