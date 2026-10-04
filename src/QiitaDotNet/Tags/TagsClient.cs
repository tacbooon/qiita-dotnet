using System.Text.Json;
using QiitaDotNet.Models;
using QiitaDotNet.Serialization;

namespace QiitaDotNet.Tags;

/// <summary>
/// <see cref="ITagsClient"/> の既定実装です。
/// </summary>
/// <remarks>
/// <para><see cref="QiitaClient"/> が生成して公開することを想定しています。</para>
/// </remarks>
internal sealed class TagsClient : ITagsClient
{
    private readonly ApiTransport _transport;

    /// <summary>
    /// タグリソースを操作するためのサブクライアントを初期化します。
    /// </summary>
    /// <param name="transport">API 呼び出しに使用するトランスポート。</param>
    internal TagsClient(ApiTransport transport)
    {
        ArgumentNullException.ThrowIfNull(transport);
        _transport = transport;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Tag>> ListTagsAsync(
        int? page = null,
        int? perPage = null,
        TagSort? sort = null,
        CancellationToken cancellationToken = default)
    {
        Pagination.Validate(page, perPage);
        if (sort.HasValue && !Enum.IsDefined(sort.Value))
        {
            throw new ArgumentOutOfRangeException(
                nameof(sort), sort, "sort must be a defined TagSort value.");
        }

        var query = new QueryBuilder()
            .AddIfNotNull("page", page)
            .AddIfNotNull("per_page", perPage)
            .AddIfNotNull("sort", ToQueryValue(sort));

        return await _transport
                .GetJsonAsync(
                    "/tags",
                    query,
                    QiitaJsonSerializerContext.Default.ListTag,
                    cancellationToken)
                .ConfigureAwait(false)
            ?? [];
    }

    /// <inheritdoc />
    public async Task<Tag> GetTagAsync(
        string tagId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tagId);
        if (tagId is "." or "..")
        {
            throw new ArgumentException("tagId must not be '.' or '..'.", nameof(tagId));
        }

        var path = $"/tags/{Uri.EscapeDataString(tagId)}";
        var tag = await _transport
                .GetJsonAsync(
                    path,
                    new QueryBuilder(),
                    QiitaJsonSerializerContext.Default.Tag,
                    cancellationToken)
                .ConfigureAwait(false);

        return tag ?? throw new JsonException("Response body was null.");
    }

    /// <summary>
    /// <see cref="TagSort"/> をクエリパラメータ用の文字列に変換します。
    /// </summary>
    /// <param name="sort">ソート順。null の場合は null を返します。</param>
    /// <returns>クエリパラメータ用の文字列。未指定の場合は null。</returns>
    private static string? ToQueryValue(TagSort? sort) => sort switch
    {
        null => null,
        TagSort.Count => "count",
        TagSort.Name => "name",
        _ => throw new ArgumentOutOfRangeException(
            nameof(sort), sort, "sort must be a defined TagSort value."),
    };
}
