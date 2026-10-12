using System.Globalization;

namespace Capitals.Core;

public sealed class CapitalsGame
{
    public const int AnswersToWin = 7;
    public const int TriesPerQuestion = 5;
    private readonly Capital[] capitals;
    private readonly Random random;
    private readonly StringComparer comparer;

    public CapitalsGame(IEnumerable<Capital> capitals, Random? random = null, CultureInfo? culture = null)
    {
        ArgumentNullException.ThrowIfNull(capitals);
        this.capitals = capitals.ToArray();
        if (this.capitals.Length == 0 || this.capitals.Any(capital => capital is null
                || string.IsNullOrWhiteSpace(capital.Country) || string.IsNullOrWhiteSpace(capital.City)))
            throw new ArgumentException("At least one valid country/capital pair is required.", nameof(capitals));
        this.random = random ?? Random.Shared;
        comparer = StringComparer.Create(culture ?? CultureInfo.CurrentCulture, ignoreCase: true);
    }

    public GameState State { get; private set; } = GameState.NotStarted;
    public int AnswersRemaining { get; private set; }
    public int TriesRemaining { get; private set; }
    public Capital? CurrentQuestion { get; private set; }

    public void StartNewGame()
    {
        AnswersRemaining = AnswersToWin;
        TriesRemaining = TriesPerQuestion;
        State = GameState.Playing;
        NextQuestion();
    }

    public bool TryAnswer(string? answer)
    {
        if (State != GameState.Playing)
            return false;

        bool correct = comparer.Equals(CurrentQuestion!.City, answer?.Trim());
        if (correct)
        {
            AnswersRemaining--;
            TriesRemaining = TriesPerQuestion;
            if (AnswersRemaining == 0)
                State = GameState.Winner;
            else
                NextQuestion();
        }
        else if (--TriesRemaining == 0)
        {
            State = GameState.Loser;
        }
        return correct;
    }

    private void NextQuestion() => CurrentQuestion = capitals[random.Next(capitals.Length)];
}
