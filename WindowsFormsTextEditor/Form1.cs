using System;
using System.IO;
using System.Windows.Forms;
using TextEditor.Core;
using WindowsFormsTextEditor.Properties;

namespace WindowsFormsTextEditor
{
    public sealed partial class Form1 : Form
    {
        private readonly TextDocument document = new TextDocument();

        public Form1()
        {
            InitializeComponent();
            txtBox.Dock = DockStyle.Fill;
            txtBox.BringToFront();
            saveFileDialog1.FileName = "new file.txt";
            var saveAs = new ToolStripMenuItem("Save As...") { ShortcutKeys = Keys.Control | Keys.Shift | Keys.S };
            saveAs.Click += (sender, e) => SaveDocument(true);
            fileToolStripMenuItem.DropDownItems.Insert(3, saveAs);
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Text = Resources.Form1_SimpleTextEditor + " — " +
                (document.FilePath == null ? "new file" : Path.GetFileName(document.FilePath)) +
                (document.HasChanges ? " *" : "");
        }

        public void SaveToFile() => SaveDocument(false);

        private bool SaveDocument(bool saveAs)
        {
            string path = document.FilePath;
            if (saveAs || path == null)
            {
                if (saveFileDialog1.ShowDialog(this) != DialogResult.OK)
                    return false;
                path = saveFileDialog1.FileName;
            }
            try
            {
                document.Save(path);
                saveFileDialog1.FileName = path;
                UpdateTitle();
                return true;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show(this, ex.Message, "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool CanContinue() => document.CanContinue(() =>
        {
            var choice = MessageBox.Show(this,
                Resources.Form1_NewToolStripMenuItem_Click_SaveDocYesNoCancel,
                Resources.Form1_NewToolStripMenuItem_Click_SaveDoc,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            return choice == DialogResult.Yes ? UnsavedChangesChoice.Save :
                choice == DialogResult.No ? UnsavedChangesChoice.Discard : UnsavedChangesChoice.Cancel;
        }, () => SaveDocument(false));

        public void NewFile()
        {
            document.New();
            txtBox.Clear();
            saveFileDialog1.FileName = "new file.txt";
            UpdateTitle();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) => SaveToFile();
        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            document.Text = txtBox.Text;
            UpdateTitle();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CanContinue()) NewFile();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) != DialogResult.OK || !CanContinue())
                return;
            try
            {
                document.Load(openFileDialog1.FileName);
                txtBox.Text = document.Text;
                txtBox.ClearUndo();
                saveFileDialog1.FileName = document.FilePath;
                UpdateTitle();
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show(this, ex.Message, "Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => e.Cancel = !CanContinue();
        private void undoToolStripMenuItem_Click(object sender, EventArgs e) => txtBox.Undo();
        private void cutToolStripMenuItem_Click(object sender, EventArgs e) => txtBox.Cut();
        private void copyToolStripMenuItem_Click(object sender, EventArgs e) => txtBox.Copy();
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) => txtBox.Paste();
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) => txtBox.SelectedText = "";
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) => txtBox.SelectAll();
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = txtBox.CanUndo;
            cutToolStripMenuItem.Enabled = copyToolStripMenuItem.Enabled =
                deleteToolStripMenuItem.Enabled = txtBox.SelectionLength > 0;
            pasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
            selectAllToolStripMenuItem.Enabled = txtBox.TextLength > 0;
        }
        private void Form1_Resize(object sender, EventArgs e) { }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutBox1().Show(this);
    }
}
