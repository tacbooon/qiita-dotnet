using System.Text.Json.Serialization;

namespace QiitaDotNet.Models;

/// <summary>
/// Qiita Team 上のグループです。
/// </summary>
public sealed record Group
{
    /// <summary>作成日時です。</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>グループの説明です。</summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>表示用のグループ名です。</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>非公開グループかどうかです。</summary>
    [JsonPropertyName("private")]
    public bool Private { get; init; }

    /// <summary>更新日時です。</summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>チーム内で一意なグループ名です。</summary>
    [JsonPropertyName("url_name")]
    public required string UrlName { get; init; }
}
