using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CAPI_CORE.Rules;

namespace CAPI_CORE.Storage;

/// <summary>
/// Writes and reads the JSON files of a game: the final export of the estimates (US-09)
/// and the save written on a coffee break, reloaded to resume the game (US-10, US-11).
/// The rule chosen for the game is stored in both files.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public static class GameStorage
{
    private const int MinimumPlayerCount = 2;

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // Keeps accented characters readable in the files instead of escaping them.
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// Builds the JSON export of an estimated backlog: the rule of the game and, for each feature, its difficulty.
    /// </summary>
    /// <param name="session">The game whose backlog is fully estimated.</param>
    /// <returns>The content of the export file.</returns>
    /// <exception cref="InvalidOperationException">Thrown when some features are not estimated yet.</exception>
    public static string SerializeExport(GameSession session)
    {
        if (!session.IsBacklogEstimated)
        {
            throw new InvalidOperationException("Le backlog n'est pas entièrement estimé.");
        }

        ExportFile file = new()
        {
            Rule = session.Rule.Name,
            Features = session.Features.Select(feature => new EstimatedFeatureFile
            {
                Id = feature.Id,
                Title = feature.Title,
                Description = feature.Description,
                Difficulty = session.Estimates[feature].Value
            }).ToList()
        };

        return JsonSerializer.Serialize(file, jsonOptions);
    }

    /// <summary>
    /// Writes the JSON export of an estimated backlog to a file.
    /// </summary>
    /// <param name="session">The game whose backlog is fully estimated.</param>
    /// <param name="path">The path of the file to write.</param>
    /// <exception cref="InvalidOperationException">Thrown when some features are not estimated yet.</exception>
    public static void ExportEstimates(GameSession session, string path)
    {
        File.WriteAllText(path, SerializeExport(session));
    }

    /// <summary>
    /// Builds the JSON save of an interrupted game: players, rule, backlog, estimates already validated
    /// and current feature.
    /// </summary>
    /// <param name="session">The game interrupted by a coffee break.</param>
    /// <returns>The content of the save file.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the backlog is already fully estimated.</exception>
    public static string SerializeSave(GameSession session)
    {
        if (session.IsBacklogEstimated)
        {
            throw new InvalidOperationException("Le backlog est entièrement estimé : il n'y a rien à reprendre.");
        }

        SaveFile file = new()
        {
            Rule = session.Rule.Name,
            Players = session.Players.Select(player => player.Name).ToList(),
            Features = session.Features.Select(feature => new FeatureFile
            {
                Id = feature.Id,
                Title = feature.Title,
                Description = feature.Description
            }).ToList(),
            Estimates = session.Features
                .Where(feature => session.Estimates.ContainsKey(feature))
                .Select(feature => new EstimateFile { FeatureId = feature.Id, Difficulty = session.Estimates[feature].Value })
                .ToList(),
            CurrentFeatureId = session.CurrentFeature.Id
        };

        return JsonSerializer.Serialize(file, jsonOptions);
    }

    /// <summary>
    /// Writes the JSON save of an interrupted game to a file.
    /// </summary>
    /// <param name="session">The game interrupted by a coffee break.</param>
    /// <param name="path">The path of the file to write.</param>
    /// <exception cref="InvalidOperationException">Thrown when the backlog is already fully estimated.</exception>
    public static void SaveGame(GameSession session, string path)
    {
        File.WriteAllText(path, SerializeSave(session));
    }

    /// <summary>
    /// Rebuilds a game from the content of a save file. The game resumes at the first round of the current feature.
    /// </summary>
    /// <param name="json">The content of the save file.</param>
    /// <returns>The resumed game.</returns>
    /// <exception cref="InvalidSaveFileException">
    /// Thrown when the content is not valid JSON or does not describe a valid game
    /// (unknown rule, fewer than two players, empty backlog, inconsistent estimates or current feature).
    /// </exception>
    public static GameSession DeserializeSave(string json)
    {
        SaveFile? file;
        try
        {
            file = JsonSerializer.Deserialize<SaveFile>(json, jsonOptions);
        }
        catch (JsonException exception)
        {
            throw new InvalidSaveFileException("Le fichier n'est pas un JSON valide.", exception);
        }

        if (file is null)
        {
            throw new InvalidSaveFileException("Le fichier de sauvegarde est vide.");
        }

        IValidationRule rule = ReadRule(file.Rule);
        List<Player> players = ReadPlayers(file.Players);
        List<Feature> features = ReadFeatures(file.Features);
        List<KeyValuePair<Feature, Card>> estimates = ReadEstimates(file.Estimates, features);

        if (estimates.Count >= features.Count || features[estimates.Count].Id != file.CurrentFeatureId)
        {
            throw new InvalidSaveFileException(
                "La fonctionnalité courante doit être la première fonctionnalité non estimée du backlog.");
        }

        try
        {
            return new GameSession(players, features, rule, estimates);
        }
        catch (ArgumentException exception)
        {
            throw new InvalidSaveFileException(exception.Message, exception);
        }
    }

    /// <summary>
    /// Reads a save file and rebuilds the game it describes. The game resumes at the first round of the current feature.
    /// </summary>
    /// <param name="path">The path of the save file.</param>
    /// <returns>The resumed game.</returns>
    /// <exception cref="InvalidSaveFileException">Thrown when the file cannot be read or does not describe a valid game.</exception>
    public static GameSession LoadGame(string path)
    {
        string json;
        try
        {
            json = File.ReadAllText(path);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            throw new InvalidSaveFileException("Le fichier de sauvegarde ne peut pas être lu.", exception);
        }

        return DeserializeSave(json);
    }

    private static IValidationRule ReadRule(string? name)
    {
        try
        {
            return ValidationRules.FromName(name ?? string.Empty);
        }
        catch (ArgumentException exception)
        {
            throw new InvalidSaveFileException(exception.Message, exception);
        }
    }

    private static List<Player> ReadPlayers(List<string>? names)
    {
        if (names is null || names.Count < MinimumPlayerCount)
        {
            throw new InvalidSaveFileException("La sauvegarde doit contenir au moins deux joueurs.");
        }

        if (names.Any(string.IsNullOrWhiteSpace) || names.Distinct(StringComparer.Ordinal).Count() != names.Count)
        {
            throw new InvalidSaveFileException("Chaque joueur doit avoir un pseudo non vide et unique.");
        }

        return names.Select(name => new Player(name)).ToList();
    }

    private static List<Feature> ReadFeatures(List<FeatureFile>? files)
    {
        if (files is null || files.Count == 0)
        {
            throw new InvalidSaveFileException("La sauvegarde doit contenir au moins une fonctionnalité.");
        }

        if (files.Any(feature => string.IsNullOrWhiteSpace(feature.Id) || string.IsNullOrWhiteSpace(feature.Title)))
        {
            throw new InvalidSaveFileException("Chaque fonctionnalité doit avoir un identifiant et un titre.");
        }

        if (files.Select(feature => feature.Id).Distinct(StringComparer.Ordinal).Count() != files.Count)
        {
            throw new InvalidSaveFileException("Deux fonctionnalités portent le même identifiant.");
        }

        return files.Select(feature => new Feature(feature.Id!, feature.Title!, feature.Description)).ToList();
    }

    private static List<KeyValuePair<Feature, Card>> ReadEstimates(List<EstimateFile>? files, List<Feature> features)
    {
        List<KeyValuePair<Feature, Card>> estimates = new();

        foreach (EstimateFile estimate in files ?? new List<EstimateFile>())
        {
            Feature? feature = features.Cast<Feature?>().FirstOrDefault(candidate => candidate!.Value.Id == estimate.FeatureId);
            if (feature is null || estimate.Difficulty is null)
            {
                throw new InvalidSaveFileException(
                    $"L'estimation « {estimate.FeatureId} » ne correspond à aucune fonctionnalité ou n'a pas de difficulté.");
            }

            int difficulty = estimate.Difficulty.Value;
            estimates.Add(new KeyValuePair<Feature, Card>(feature.Value, new Card(difficulty.ToString(), difficulty)));
        }

        return estimates;
    }
}
