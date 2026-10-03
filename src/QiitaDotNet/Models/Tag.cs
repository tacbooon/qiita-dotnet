using System.Text.Json.Serialization;

namespace QiitaDotNet.Models;

/// <summary>
/// Qiita のタグです。
/// </summary>
public sealed record Tag
{
    /// <summary>フォロワー数です。</summary>
    [JsonPropertyName("followers_count")]
    public int FollowersCount { get; init; }

    /// <summary>タグアイコンの URL です。未設定の場合は null です。</summary>
    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; init; }

    /// <summary>タグ名です。</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>このタグが付けられた記事数です。</summary>
    [JsonPropertyName("items_count")]
    public int ItemsCount { get; init; }
}
