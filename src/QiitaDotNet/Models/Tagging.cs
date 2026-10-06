using System.Text.Json.Serialization;

namespace QiitaDotNet.Models;

/// <summary>
/// 記事とタグの関連です。合わせて <see cref="Tag"/> を参照してください。
/// </summary>
public sealed record Tagging
{
    /// <summary>タグ名です。</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>タグのバージョン一覧です。バージョンが付いていない場合は空配列です。</summary>
    [JsonPropertyName("versions")]
    public required IReadOnlyList<string> Versions { get; init; }
}
