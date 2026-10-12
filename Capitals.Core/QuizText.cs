using System.Globalization;
using System.Resources;

namespace Capitals.Core;

public static class QuizText
{
    private static readonly ResourceManager Resources = new("Capitals.Core.Localization.Resources", typeof(QuizText).Assembly);

    public static string Get(string key, CultureInfo? culture = null) =>
        Resources.GetString(key, culture) ?? throw new MissingManifestResourceException($"Missing text: {key}");
}
