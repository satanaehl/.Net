using System.Text;

namespace Capitals.Core;

public static class CapitalCatalog
{
    // Keep the original data files and their Windows-1251 encoding.
    public static IReadOnlyList<Capital> LoadDefault()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var assembly = typeof(CapitalCatalog).Assembly;
        var result = new List<Capital>();
        foreach (var name in assembly.GetManifestResourceNames()
                     .Where(name => name.EndsWith(".txt", StringComparison.Ordinal))
                     .Order(StringComparer.Ordinal))
        {
            using var stream = assembly.GetManifestResourceStream(name)!;
            using var reader = new StreamReader(stream, Encoding.GetEncoding(1251));
            result.AddRange(Read(reader));
        }
        if (result.Count == 0)
            throw new InvalidDataException("The capital catalog is empty.");
        return result.AsReadOnly();
    }

    public static IReadOnlyList<Capital> Read(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        var result = new List<Capital>();
        string? line;
        int lineNumber = 0;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var parts = line.Split(';');
            if (parts.Length != 2 || parts.Any(string.IsNullOrWhiteSpace))
                throw new InvalidDataException($"Invalid country/capital pair on line {lineNumber}.");
            result.Add(new Capital(parts[0].Trim(), parts[1].Trim()));
        }
        return result.AsReadOnly();
    }
}
