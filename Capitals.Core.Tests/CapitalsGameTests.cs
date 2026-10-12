using System.Globalization;
using Capitals.Core;
using Xunit;

namespace Capitals.Core.Tests;

public class CapitalsGameTests
{
    private static CapitalsGame CreateGame() => new([new Capital("Франция", "Париж")], new Random(1), CultureInfo.InvariantCulture);

    [Fact]
    public void StartAndRestart_ResetCountersAndState()
    {
        var game = CreateGame();
        Assert.Equal(GameState.NotStarted, game.State);
        Assert.False(game.TryAnswer("Париж"));
        game.StartNewGame();
        Assert.Equal(7, game.AnswersRemaining);
        Assert.Equal(5, game.TriesRemaining);
        Assert.Equal("Франция", game.CurrentQuestion!.Country);
        game.TryAnswer("wrong");
        game.StartNewGame();
        Assert.Equal(GameState.Playing, game.State);
        Assert.Equal(5, game.TriesRemaining);
    }

    [Fact]
    public void CorrectAnswerOnFifthTry_IsAccepted()
    {
        var game = CreateGame();
        game.StartNewGame();
        for (int i = 0; i < 4; i++)
            Assert.False(game.TryAnswer("wrong"));
        Assert.Equal(1, game.TriesRemaining);
        Assert.True(game.TryAnswer("  пАРИЖ  "));
        Assert.Equal(GameState.Playing, game.State);
        Assert.Equal(6, game.AnswersRemaining);
        Assert.Equal(5, game.TriesRemaining);
    }

    [Fact]
    public void FiveWrongAnswers_LoseAndFurtherAnswersDoNotChangeState()
    {
        var game = CreateGame();
        game.StartNewGame();
        for (int i = 0; i < 5; i++)
            Assert.False(game.TryAnswer("wrong"));
        Assert.Equal(GameState.Loser, game.State);
        Assert.Equal(0, game.TriesRemaining);
        Assert.False(game.TryAnswer("Париж"));
        Assert.Equal(7, game.AnswersRemaining);
    }

    [Fact]
    public void SevenCorrectAnswers_WinAndGameCanRestart()
    {
        var game = CreateGame();
        game.StartNewGame();
        for (int i = 0; i < 7; i++)
            Assert.True(game.TryAnswer("Париж"));
        Assert.Equal(GameState.Winner, game.State);
        Assert.Equal(0, game.AnswersRemaining);
        Assert.False(game.TryAnswer("Париж"));
        game.StartNewGame();
        Assert.Equal(GameState.Playing, game.State);
        Assert.Equal(7, game.AnswersRemaining);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void EmptyAnswer_ConsumesOneTry(string? answer)
    {
        var game = CreateGame();
        game.StartNewGame();
        Assert.False(game.TryAnswer(answer));
        Assert.Equal(4, game.TriesRemaining);
    }

    [Fact]
    public void EmptyCatalog_IsRejected() => Assert.Throws<ArgumentException>(() => new CapitalsGame([]));

    [Fact]
    public void Parser_TrimsPairsAndSkipsEmptyLines()
    {
        using var reader = new StringReader(" Франция ; Париж \n\n Германия;Берлин");
        Assert.Equal([new Capital("Франция", "Париж"), new Capital("Германия", "Берлин")], CapitalCatalog.Read(reader));
    }

    [Theory]
    [InlineData("Франция")]
    [InlineData("Франция;")]
    [InlineData(";Париж")]
    [InlineData("Франция;Париж;лишнее")]
    public void Parser_RejectsInvalidPairs(string line)
    {
        using var reader = new StringReader(line);
        Assert.Throws<InvalidDataException>(() => CapitalCatalog.Read(reader));
    }

    [Fact]
    public void EmbeddedCatalog_LoadsCyrillicWithoutDependingOnWorkingDirectory()
    {
        var catalog = CapitalCatalog.LoadDefault();
        Assert.True(catalog.Count > 100);
        Assert.Contains(catalog, item => item.City == "Париж");
        Assert.All(catalog, item => Assert.DoesNotContain('\uFFFD', item.City));
    }

    [Theory]
    [InlineData("ru")]
    [InlineData("be")]
    [InlineData("de")]
    [InlineData("fr")]
    [InlineData("en")]
    public void Localizations_ContainGameMessages(string culture)
    {
        foreach (var key in new[] { "Form1_Winner", "Form1_Loser", "Form1_Question", "Form1_NumberOfTries", "Form1_NumberRightAnswersToWin" })
            Assert.False(string.IsNullOrWhiteSpace(QuizText.Get(key, CultureInfo.GetCultureInfo(culture))));
    }
}
