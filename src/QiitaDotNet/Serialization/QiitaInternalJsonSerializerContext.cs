using System.Text.Json;
using System.Text.Json.Serialization;

namespace QiitaDotNet.Serialization;

/// <summary>
/// 公開しない内部 DTO 用の JSON Source Generator コンテキストです。公開モデルは <see cref="QiitaJsonSerializerContext"/> を使います
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never)]
[JsonSerializable(typeof(ErrorResponse))]
internal sealed partial class QiitaInternalJsonSerializerContext : JsonSerializerContext
{
}
