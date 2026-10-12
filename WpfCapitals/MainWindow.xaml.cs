using System.Windows;
using Capitals.Core;

namespace WpfCapitals;

public partial class MainWindow : Window
{
    private readonly CapitalsGame game = new(CapitalCatalog.LoadDefault());

    public MainWindow() => InitializeComponent();

    private void Start_Click(object sender, RoutedEventArgs e)
    {
        game.StartNewGame();
        UpdateGame();
    }

    private void Answer_Click(object sender, RoutedEventArgs e)
    {
        game.TryAnswer(AnswerBox.Text);
        UpdateGame();
    }

    private void UpdateGame()
    {
        bool playing = game.State == GameState.Playing;
        AnswerBox.IsEnabled = AnswerButton.IsEnabled = playing;
        AnswerBox.Clear();
        AnswersText.Text = game.State == GameState.Winner ? "" : string.Format(QuizText.Get("Form1_NumberRightAnswersToWin"), game.AnswersRemaining);
        TriesText.Text = game.State == GameState.Winner ? "" : string.Format(QuizText.Get("Form1_NumberOfTries"), game.TriesRemaining);
        QuestionText.Text = game.State == GameState.Winner ? "" : string.Format(QuizText.Get("Form1_Question"), game.CurrentQuestion!.Country);
        StatusText.Text = game.State switch
        {
            GameState.Winner => QuizText.Get("Form1_Winner"),
            GameState.Loser => QuizText.Get("Form1_Loser"),
            _ => ""
        };
        if (playing)
            AnswerBox.Focus();
    }
}
