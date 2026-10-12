namespace Capitals.Core;

public sealed record Capital(string Country, string City);

public enum GameState
{
    NotStarted,
    Playing,
    Winner,
    Loser
}
