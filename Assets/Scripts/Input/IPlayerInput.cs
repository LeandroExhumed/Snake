namespace LeandroExhumed.SnakeGame.Input
{
    public interface IPlayerInput : IMovementRequester
    {
        char LeftKey { get; }
        char RightKey { get; }
    }
}