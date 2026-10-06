using System.Text.Json.Serialization;

namespace QiitaDotNet.Models;

/// <summary>
/// Qiita の記事です。
/// </summary>
public sealed record Item
{
    /// <summary>HTML 形式の記事本文です。</summary>
    [JsonPropertyName("rendered_body")]
    public required string RenderedBody { get; init; }

    /// <summary>Markdown 形式の記事本文です。</summary>
    [JsonPropertyName("body")]
    public required string Body { get; init; }

    /// <summary>共同編集モードかどうかです。Qiita Team のみで利用可能な機能です。</summary>
    [JsonPropertyName("coediting")]
    public bool Coediting { get; init; }

    /// <summary>コメント数です。</summary>
    [JsonPropertyName("comments_count")]
    public int CommentsCount { get; init; }

    /// <summary>作成日時です。</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>共有先のグループです。Qiita Team 以外では null になります。</summary>
    [JsonPropertyName("group")]
    public Group? Group { get; init; }

    /// <summary>記事 ID です。</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>いいね数です。Qiita のみで利用可能な機能です。</summary>
    [JsonPropertyName("likes_count")]
    public int LikesCount { get; init; }

    /// <summary>限定共有記事かどうかです。Qiita のみで利用可能な機能です。</summary>
    [JsonPropertyName("private")]
    public bool Private { get; init; }

    /// <summary>絵文字リアクション数です。Qiita Team のみで利用可能な機能です。</summary>
    [JsonPropertyName("reactions_count")]
    public int ReactionsCount { get; init; }

    /// <summary>ストック数です。</summary>
    [JsonPropertyName("stocks_count")]
    public int StocksCount { get; init; }

    /// <summary>付けられたタグの一覧です。</summary>
    [JsonPropertyName("tags")]
    public required IReadOnlyList<Tagging> Tags { get; init; }

    /// <summary>記事タイトルです。</summary>
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    /// <summary>更新日時です。</summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>記事の URL です。</summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    /// <summary>投稿者です。</summary>
    [JsonPropertyName("user")]
    public required User User { get; init; }

    /// <summary>閲覧数です。取得できない場合は null になります。</summary>
    [JsonPropertyName("page_views_count")]
    public int? PageViewsCount { get; init; }

    /// <summary>Qiita Team での投稿者のチーム所属情報の要約です。Qiita Team 以外では null になります。</summary>
    [JsonPropertyName("team_membership")]
    public TeamMembershipSummary? TeamMembership { get; init; }

    /// <summary>所属 Organization の url_name です。所属がない場合は null になります。</summary>
    [JsonPropertyName("organization_url_name")]
    public string? OrganizationUrlName { get; init; }

    /// <summary>スライドモードが有効かどうかです。</summary>
    [JsonPropertyName("slide")]
    public bool Slide { get; init; }

    /// <summary>投稿キャンペーンの UUID です。登録されていない場合は null になります。</summary>
    [JsonPropertyName("posting_campaign_uuid")]
    public string? PostingCampaignUuid { get; init; }
}
