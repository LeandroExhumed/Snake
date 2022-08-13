namespace LeandroExhumed.SnakeGame.Input
{
    public interface IPlayerInput : IGameInput
    {
        char LeftKey { get; }
        char RightKey { get; }
    }
}