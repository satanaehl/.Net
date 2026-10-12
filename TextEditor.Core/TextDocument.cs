using System.Text;

namespace TextEditor.Core;

public enum UnsavedChangesChoice { Save, Discard, Cancel }

/// <summary>Document state and persistence, independent of any window or dialog.</summary>
public sealed class TextDocument
{
    private string savedText = "";
    private Encoding encoding = new UTF8Encoding(false);

    public string Text { get; set; } = "";
    public string? FilePath { get; private set; }
    public bool HasChanges => !string.Equals(Text, savedText, StringComparison.Ordinal);

    public void New()
    {
        Text = savedText = "";
        FilePath = null;
        encoding = new UTF8Encoding(false);
    }

    public void Load(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string fullPath = Path.GetFullPath(path);
        using var reader = new StreamReader(fullPath, new UTF8Encoding(false), detectEncodingFromByteOrderMarks: true);
        string text = reader.ReadToEnd();
        encoding = reader.CurrentEncoding;
        Text = savedText = text;
        FilePath = fullPath;
    }

    public void Save(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string fullPath = Path.GetFullPath(path);
        File.WriteAllText(fullPath, Text, encoding);
        // A failed write must leave both the path and the dirty state intact.
        FilePath = fullPath;
        savedText = Text;
    }

    /// <summary>Call before new/open/close; the UI supplies the choice and save dialog.</summary>
    public bool CanContinue(Func<UnsavedChangesChoice> ask, Func<bool> save)
    {
        ArgumentNullException.ThrowIfNull(ask);
        ArgumentNullException.ThrowIfNull(save);
        if (!HasChanges)
            return true;
        return ask() switch
        {
            UnsavedChangesChoice.Discard => true,
            UnsavedChangesChoice.Save => save() && !HasChanges,
            _ => false
        };
    }
}
