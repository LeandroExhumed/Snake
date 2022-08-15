namespace LeandroExhumed.SnakeGame.Input
{
    public interface IPlayerInputModel : IGameInputModel
    {
        char LeftKey { get; }
        char RightKey { get; }
    }
}