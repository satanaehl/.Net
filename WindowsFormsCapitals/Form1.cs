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
            One.GetCountries();
            One.StartGame();
            btnGame.Enabled = true;
            UpdateQuestion();
            btnGame.Enabled = true;
        }

        private void btnGame_Click(object sender, EventArgs e)
        {
            One.Try(textBox1.Text);
            UpdateQuestion();
        }

        private void UpdateQuestion()
        {
            label1.Text = "Загадана страна " + One.GetOneCountry()[0] + ". Как называется её столица?";
            textBox1.Text = String.Empty;
            lblAnswer.Text = "До победы осталось угадать " + One.NumberAnswers + " столиц";
            lblTry.Text = "У Вас осталось " + One.NumberTries + " попыток";

            switch (One.StateOfPlayer)
            {
                case Capitals.State.Loser:
                    label2.Text = Resources.Form1_Loser;
                    btnGame.Enabled = false;
                    break;
                case Capitals.State.Winner:
                    label2.Text = Resources.Form1_Winner;
                    btnGame.Enabled = false;
                    lblAnswer.Text = String.Empty;
                    lblTry.Text = String.Empty;
                    label1.Text = String.Empty;
                    break;
                case Capitals.State.NotWinnerNotLoser:
                    label2.Text = String.Empty;
                    break;
            }
        }
    }
}
