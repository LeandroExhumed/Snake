using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchModel
    {
        public event System.Action<Vector2Int> OnBlockGenerated;

        private int snakesPerMatch = 2;
        private readonly Vector2Int[] spawnPositions =
        {
            new Vector2Int(3, 1),
            new Vector2Int(27, 29)
        };
        private IBlockModel block;
        private ISnakeModel[] snakes;

        private readonly IGridModel<INode> grid;

        private readonly ISnakeModel.Factory[] snakeFactories;
        private readonly BlockFactory blockFactory;

        public MatchModel (IGridModel<INode> grid, ISnakeModel.Factory[] snakeFactories, BlockFactory blockFactory)
        {
            this.grid = grid;
            this.snakeFactories = snakeFactories;
            this.blockFactory = blockFactory;
        }

        public void Initialize ()
        {
            grid.Initialize();
            GenerateSnake();
            GenerateBlock();
        }

        private void GenerateSnake ()
        {
            for (int i = 0; i < snakesPerMatch; i++)
            {
                ISnakeModel snake;
                Vector2Int startDirection;
                if (i % 2 == 0)
                {
                    snake = snakeFactories[0].Create();
                    startDirection = Vector2Int.right;
                }
                else
                {
                    snake = snakeFactories[1].Create();
                    startDirection = Vector2Int.left;
                }
                snake.Initialize(spawnPositions[i], startDirection);
                snake.OnHit += HandleSnakeHit;
            }
        }

        public void GenerateBlock ()
        {
            block = blockFactory.CreateRandomly(1);
            Vector2Int spawnPosition;
            do
            {
                spawnPosition = new(Random.Range(0, grid.Width), Random.Range(0, grid.Height));
            } while (grid.GetNode(spawnPosition) != null);
            block.Initialize(spawnPosition);
            block.OnCollected += HandleBlockCollected;
            grid.SetNode(block.Position, block);

            OnBlockGenerated?.Invoke(block.Position);
        }

        private void HandleSnakeHit ()
        {
            End();
        }

        public void End ()
        {
            Debug.Log("You died!");
        }

        private void HandleBlockCollected ()
        {
            grid.SetNode(block.Position, null);
            GenerateBlock();
        }
    }
}