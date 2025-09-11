using System.Text.Json.Serialization;
using Domain.Models;

namespace Domain.FilteredAppliedParsingModel;

public class FilteredRoot
{
    [JsonPropertyName("props")]
    public Props Props { get; set; }
}

public class Props
{
    [JsonPropertyName("pageProps")]
    public PageProps PageProps { get; set; }
}

public class PageProps
{
    [JsonPropertyName("data")]
    public PageData Data { get; set; }
}

public class PageData
{
    [JsonPropertyName("searchAds")]
    public SearchAds SearchAds { get; set; }
}

public class SearchAds
{
    [JsonPropertyName("items")]
    public List<AdvertListItem> Items { get; set; }
}