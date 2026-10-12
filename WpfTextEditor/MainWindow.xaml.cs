using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using TextEditor.Core;

namespace WpfTextEditor;

public partial class MainWindow : Window
{
    private readonly TextDocument document = new();
    public MainWindow()
    {
        InitializeComponent();
        UpdateTitle();
    }

    private void UpdateTitle() => Title = "Text Editor — WPF — " +
        (document.FilePath is null ? "new file" : Path.GetFileName(document.FilePath)) +
        (document.HasChanges ? " *" : "");

    private bool Save(bool saveAs = false)
    {
        string? path = document.FilePath;
        if (saveAs || path is null)
        {
            var dialog = new SaveFileDialog { FileName = path ?? "new file.txt", Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*" };
            if (dialog.ShowDialog(this) != true)
                return false;
            path = dialog.FileName;
        }
        try
        {
            document.Save(path);
            UpdateTitle();
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show(this, ex.Message, "Save", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }

    private bool CanContinue() => document.CanContinue(() =>
    {
        var choice = MessageBox.Show(this, "Save changes before continuing? / Сохранить изменения?", "Unsaved changes",
            MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        return choice switch
        {
            MessageBoxResult.Yes => UnsavedChangesChoice.Save,
            MessageBoxResult.No => UnsavedChangesChoice.Discard,
            _ => UnsavedChangesChoice.Cancel
        };
    }, () => Save());

    private void New_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (!CanContinue()) return;
        document.New();
        Editor.Clear();
        ResetUndo();
        UpdateTitle();
    }

    private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var dialog = new OpenFileDialog { Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*" };
        if (dialog.ShowDialog(this) != true || !CanContinue()) return;
        try
        {
            document.Load(dialog.FileName);
            Editor.Text = document.Text;
            ResetUndo();
            UpdateTitle();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show(this, ex.Message, "Open", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Save_Executed(object sender, ExecutedRoutedEventArgs e) => Save();
    private void ResetUndo()
    {
        Editor.IsUndoEnabled = false;
        Editor.IsUndoEnabled = true;
    }
    private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e) => Save(saveAs: true);
    private void Exit_Click(object sender, RoutedEventArgs e) => Close();
    private void Window_Closing(object? sender, CancelEventArgs e) => e.Cancel = !CanContinue();
    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        document.Text = Editor.Text;
        UpdateTitle();
    }
}
