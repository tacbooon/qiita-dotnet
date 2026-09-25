using System.Text.Json;
using System.Text.Json.Serialization;
using QiitaDotNet.Models;

namespace QiitaDotNet.Serialization;

/// <summary>
/// 高速化・トリミング対応・AOT のための JSON Source Generator コンテキストです。
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never)]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(List<User>))]
public sealed partial class QiitaJsonSerializerContext : JsonSerializerContext
{
}
