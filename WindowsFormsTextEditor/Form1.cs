using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsTextEditor.Properties;


//TODO add context menu
//TODO add multithreading

namespace WindowsFormsTextEditor
{
    public sealed partial class Form1 : Form
    {
        private static bool _isChanged = false;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog1.FileName = "new file";
            Text = Resources.Form1_SimpleTextEditor;
            txtBox.Width = ClientSize.Width;
            txtBox.Height = ClientSize.Height;
        }

        public void SaveToFile()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.WriteLine(txtBox.Text);
                }
                saveFileDialog1.FileName = "new file";
                Text = Resources.Form1_SimpleTextEditor + Path.GetFileName(path);
            }
            _isChanged = false;
        }

        public void NewFile()
        {
            txtBox.Clear();
            _isChanged = false;
            Text = Resources.Form1_SimpleTextEditor;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToFile();
        }

        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            _isChanged = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isChanged)
            {
                const string message = "Do you want to save it?";
                const string caption = "Save?";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNoCancel,
                                             MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveToFile();
                        break;
                    case DialogResult.No:
                        NewFile();
                        break;
                    case DialogResult.Cancel: break;
                }
            }
            else
            {
                NewFile();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                using (StreamReader reader = new StreamReader(path, false))
                {
                    txtBox.Text = reader.ReadToEnd();
                }
                Text = Resources.Form1_SimpleTextEditor + Path.GetFileName(path);
                saveFileDialog1.FileName = Path.GetFileName(path);
            }

            _isChanged = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isChanged)
            {
                const string message =
                    "This document is not saved! Do you want to save it before exit?";
                const string caption = "Exit";
                var result = MessageBox.Show(message, caption,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        e.Cancel = true;
                        SaveToFile();
                        break;
                    case DialogResult.No:

                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.Clear();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtBox.SelectAll();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = txtBox.CanUndo;
            cutToolStripMenuItem.Enabled =
                pasteToolStripMenuItem.Enabled =
                deleteToolStripMenuItem.Enabled = txtBox.SelectionLength > 0;
            pasteToolStripMenuItem.Enabled = Clipboard.GetDataObject().
                GetDataPresent(typeof(string)); //acceptable for null clipboard
            selectAllToolStripMenuItem.Enabled = txtBox.Text != String.Empty;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            txtBox.Width = ClientSize.Width;
            txtBox.Height = ClientSize.Height;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.Show();
        }
    }
}






