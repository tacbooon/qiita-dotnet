using System.Text.Json.Serialization;

namespace QiitaDotNet.Models;

/// <summary>
/// 認証中の Qiita ユーザーです。通常の <see cref="User"/> よりも詳細な情報を含みます。
/// </summary>
public sealed record AuthenticatedUser : User
{
    /// <summary>Qiita にアップロードできる画像の最大月間容量 (バイト) です。</summary>
    [JsonPropertyName("image_monthly_upload_limit")]
    public int ImageMonthlyUploadLimit { get; init; }

    /// <summary>今月 Qiita にアップロードできる画像の残り容量 (バイト) です。</summary>
    [JsonPropertyName("image_monthly_upload_remaining")]
    public int ImageMonthlyUploadRemaining { get; init; }
}
