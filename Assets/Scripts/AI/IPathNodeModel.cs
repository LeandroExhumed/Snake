using LeandroExhumed.SnakeGame.Grid;

namespace LeandroExhumed.SnakeGame.AI
{
    public interface IPathNodeModel : INodeModel
    {
        int FCost { get; }
        int GCost { get; set; }
        int HCost { get; set; }
        bool IsWalkable { get; }
        IPathNodeModel CameFrom { get; set; }
    }
}