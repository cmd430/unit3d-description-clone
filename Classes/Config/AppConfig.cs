namespace Unit3dDescriptionClone.Config;

internal sealed record AppConfig(
    string FromTrackerUrl,
    string FromTrackerApiKey,
    bool FromTrackerSupportsFileNameSearch,
    string ToTrackerUrl,
    string ToTrackerApiKey,
    string ToTrackerUsername,
    string ToTrackerPassword,
    string ToTrackerTotpSecret,
    string ImageHostUrl,
    string ImageHostApiKey,
    IReadOnlyDictionary<string, string> KnownImages)
{
    public static AppConfig Load(string path)
    {
        var cfg = IniConfig.Load(path);
        var from = cfg["from_tracker"];
        var to = cfg["to_tracker"];
        var img = cfg["image_host"];
        var knownImages = cfg.TryGetValue("known_images", out var ki)
            ? (IReadOnlyDictionary<string, string>)ki
            : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        return new AppConfig(
            FromTrackerUrl: from["url"],
            FromTrackerApiKey: from["api_key"],
            FromTrackerSupportsFileNameSearch: !from.TryGetValue("supports_file_name_search", out var sfns)
                || sfns.Equals("true", StringComparison.OrdinalIgnoreCase),
            ToTrackerUrl: to["url"],
            ToTrackerApiKey: to["api_key"],
            ToTrackerUsername: to["username"],
            ToTrackerPassword: to["password"],
            ToTrackerTotpSecret: to.GetValueOrDefault("totp_secret", ""),
            ImageHostUrl: img["url"],
            ImageHostApiKey: img["api_key"],
            KnownImages: knownImages);
    }
}
