using System.Text.Json.Serialization;

namespace CAPI_CORE.Storage;

// Shapes of the JSON files written and read by GameStorage. Property names are in French
// to match the backlog format documented in the README.

internal sealed class FeatureFile
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("titre")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

internal sealed class EstimatedFeatureFile
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("titre")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("difficulte")]
    public int? Difficulty { get; set; }
}

internal sealed class ExportFile
{
    [JsonPropertyName("regle")]
    public string? Rule { get; set; }

    [JsonPropertyName("fonctionnalites")]
    public List<EstimatedFeatureFile>? Features { get; set; }
}

internal sealed class EstimateFile
{
    [JsonPropertyName("id")]
    public string? FeatureId { get; set; }

    [JsonPropertyName("difficulte")]
    public int? Difficulty { get; set; }
}

internal sealed class SaveFile
{
    [JsonPropertyName("regle")]
    public string? Rule { get; set; }

    [JsonPropertyName("joueurs")]
    public List<string>? Players { get; set; }

    [JsonPropertyName("fonctionnalites")]
    public List<FeatureFile>? Features { get; set; }

    [JsonPropertyName("estimations")]
    public List<EstimateFile>? Estimates { get; set; }

    [JsonPropertyName("fonctionnaliteCourante")]
    public string? CurrentFeatureId { get; set; }
}
