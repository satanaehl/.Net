using System;
using Capitals.Core;
using System.Windows.Forms;
using WindowsFormsCapitals.Properties;

namespace WindowsFormsCapitals
{
    public partial class Form1 : Form
    {
        private readonly CapitalsGame One = new CapitalsGame(CapitalCatalog.LoadDefault());

        public Form1()
        {
            InitializeComponent();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            One.StartNewGame();
            btnGame.Enabled = true;
            UpdateQuestion();
            btnGame.Enabled = true;

        }

        private void btnGame_Click(object sender, EventArgs e)
        {
            Answer();
        }

        private void Answer()
        {
            One.TryAnswer(textBox1.Text);
            UpdateQuestion();
        }

        private void UpdateQuestion()
        {
            label1.Text = string.Format(Resources.Form1_Question, One.CurrentQuestion.Country);
            textBox1.Text = String.Empty;
            lblAnswer.Text = string.Format(Resources.Form1_NumberRightAnswersToWin, One.AnswersRemaining);
            lblTry.Text = string.Format(Resources.Form1_NumberOfTries, One.TriesRemaining);

            switch (One.State)
            {
                case GameState.Loser:
                    label2.Text = Resources.Form1_Loser;
                    btnGame.Enabled = false;
                    break;
                case GameState.Winner:
                    label2.Text = Resources.Form1_Winner;
                    btnGame.Enabled = false;
                    lblAnswer.Text = String.Empty;
                    lblTry.Text = String.Empty;
                    label1.Text = String.Empty;
                    break;
                case GameState.Playing:
                    label2.Text = String.Empty;
                    break;
            }
        }
    }
}
