using System.Text;
using TextEditor.Core;
using Xunit;

namespace TextEditor.Core.Tests;

public sealed class TextDocumentTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "TextEditor.Tests", Guid.NewGuid().ToString("N"));
    public TextDocumentTests() => Directory.CreateDirectory(directory);
    public void Dispose() => Directory.Delete(directory, recursive: true);
    private string PathFor(string name) => Path.Combine(directory, name);

    [Fact]
    public void CancelledSave_BlocksContinuationAndKeepsChanges()
    {
        var document = new TextDocument { Text = "unsaved" };
        Assert.False(document.CanContinue(() => UnsavedChangesChoice.Save, () => false));
        Assert.True(document.HasChanges);
        Assert.Equal("unsaved", document.Text);
        Assert.Null(document.FilePath);
    }

    [Fact]
    public void CancelledPrompt_DoesNotSaveOrDiscard()
    {
        var document = new TextDocument { Text = "unsaved" };
        Assert.False(document.CanContinue(() => UnsavedChangesChoice.Cancel, () => throw new Exception("Must not save")));
        Assert.True(document.HasChanges);
    }

    [Fact]
    public void SuccessfulSave_AllowsNewOpenOrClose()
    {
        var document = new TextDocument { Text = "Привет\r\nworld" };
        Assert.True(document.CanContinue(() => UnsavedChangesChoice.Save, () =>
        {
            document.Save(PathFor("saved.txt"));
            return true;
        }));
        Assert.False(document.HasChanges);
        Assert.Equal(document.Text, File.ReadAllText(PathFor("saved.txt")));
    }

    [Fact]
    public void Discard_AllowsContinuationButDoesNotMutateUntilNewOrLoad()
    {
        var document = new TextDocument { Text = "unsaved" };
        Assert.True(document.CanContinue(() => UnsavedChangesChoice.Discard, () => throw new Exception("Must not save")));
        Assert.Equal("unsaved", document.Text);
        document.New();
        Assert.Equal("", document.Text);
        Assert.False(document.HasChanges);
        Assert.Null(document.FilePath);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Привет")]
    [InlineData("one\r\ntwo\n")]
    public void RepeatedSaves_PreserveExactTextWithoutAddingNewlines(string text)
    {
        var document = new TextDocument { Text = text };
        var path = PathFor("saved.txt");
        document.Save(path);
        document.Load(path);
        document.Save(path);
        Assert.Equal(text, File.ReadAllText(path));
        Assert.False(File.ReadAllBytes(path).AsSpan().StartsWith(Encoding.UTF8.GetPreamble()));
    }

    [Theory]
    [InlineData("utf-8")]
    [InlineData("utf-16")]
    [InlineData("utf-32")]
    public void LoadAndSave_PreserveUnicodeBom(string encodingName)
    {
        var encoding = Encoding.GetEncoding(encodingName);
        var path = PathFor("original.txt");
        File.WriteAllText(path, "Привет", encoding);
        var document = new TextDocument();
        document.Load(path);
        Assert.Equal("Привет", document.Text);
        document.Text += "!";
        document.Save(path);
        Assert.True(File.ReadAllBytes(path).AsSpan().StartsWith(encoding.GetPreamble()));
        Assert.Equal("Привет!", File.ReadAllText(path));
    }

    [Fact]
    public void FailedSave_PreservesChangesAndPreviousPath()
    {
        var document = new TextDocument { Text = "before" };
        document.Save(PathFor("before.txt"));
        document.Text = "after";
        Assert.Throws<DirectoryNotFoundException>(() => document.Save(PathFor("missing/file.txt")));
        Assert.True(document.HasChanges);
        Assert.Equal(PathFor("before.txt"), document.FilePath);
    }

    [Fact]
    public void FailedLoad_PreservesCurrentDocument()
    {
        var document = new TextDocument { Text = "unsaved" };
        Assert.Throws<FileNotFoundException>(() => document.Load(PathFor("missing.txt")));
        Assert.Equal("unsaved", document.Text);
        Assert.True(document.HasChanges);
    }

    [Fact]
    public void UnchangedDocument_DoesNotPromptAndUndoToSavedTextClearsChanges()
    {
        var document = new TextDocument();
        Assert.True(document.CanContinue(() => throw new Exception("Must not prompt"), () => false));
        document.Text = "changed";
        Assert.True(document.HasChanges);
        document.Text = "";
        Assert.False(document.HasChanges);
    }
}
