using System.Text.Json.Serialization;

namespace QiitaDotNet.Models;

/// <summary>
/// 記事に埋め込まれるチーム所属情報の要約版です。表示名のみを持ちます。
/// </summary>
public sealed record TeamMembershipSummary
{
    /// <summary>チーム内での表示名です。</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
