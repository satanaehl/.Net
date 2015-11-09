using System;
using System.Windows.Forms;

namespace WindowsFormsCapitals
{
    public partial class Form1 : Form
    {
        Capitals One = new Capitals();

        public Form1()
        {
            InitializeComponent();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            One.GetCountries();
            One.StartGame();

            UpdateQuestion();
        }

        private void btnGame_Click(object sender, EventArgs e)
        {

            One.Try(textBox1.Text);

            UpdateQuestion();

        }

        private void UpdateQuestion()
        {
            label1.Text = "Загадана страна " + One.GetOneCountry()[0] + ". Как называется её столица?";
            textBox1.Text = string.Empty;
            label2.Text = One.InfoString;
            lblAnswer.Text = "До победы осталось угадать " + One.NumberAnswers + " столиц";
            lblTry.Text = "У Вас осталось " + One.NumberTries + " попыток";

        }
    }
}
