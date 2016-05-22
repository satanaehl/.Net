using System;
using System.Windows.Forms;
using WindowsFormsCapitals.Properties;

namespace WindowsFormsCapitals
{
    public partial class Form1 : Form
    {
        private Capitals One = new Capitals();

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
            One.Try(textBox1.Text);
            UpdateQuestion();
        }

        private void UpdateQuestion()
        {
            label1.Text = string.Format(Resources.Form1_Question, One.OneCountry[0]);
            textBox1.Text = String.Empty;
            lblAnswer.Text = string.Format(Resources.Form1_NumberRightAnswersToWin, One.NumberAnswers);
            lblTry.Text = string.Format(Resources.Form1_NumberOfTries, One.NumberTries);

            switch (One.StateOfPlayer)
            {
                case State.Loser:
                    label2.Text = Resources.Form1_Loser;
                    btnGame.Enabled = false;
                    break;
                case State.Winner:
                    label2.Text = Resources.Form1_Winner;
                    btnGame.Enabled = false;
                    lblAnswer.Text = String.Empty;
                    lblTry.Text = String.Empty;
                    label1.Text = String.Empty;
                    break;
                case State.None:
                    label2.Text = String.Empty;
                    break;
            }
        }
    }
}
