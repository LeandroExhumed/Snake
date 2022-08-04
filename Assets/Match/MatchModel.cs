using LeandroExhumed.SnakeGame.Collectables;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchModel
    {
        private ICollectableModel block;

        private readonly IGridModel grid;
        private readonly ISnakeModel[] snakes;

        private readonly ICollectableModel.Factory[] collectableFactories;

        public MatchModel (IGridModel grid, ISnakeModel[] snakes, ICollectableModel.Factory[] collectableFactories)
        {
            this.grid = grid;
            this.snakes = snakes;
            this.collectableFactories = collectableFactories;
        }

        public void Initialize ()
        {
            grid.Initialize();
            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i].OnHit += HandleSnakeHit;
            }
            GenerateBlock();
        }

        public void GenerateBlock ()
        {
            block = collectableFactories[Random.Range(0, collectableFactories.Length)].Create();
            Vector2Int spawnPosition = new(Random.Range(0, 30), Random.Range(0, 30));
            block.Initialize(spawnPosition);
            block.OnCollected += HandleBlockCollected;
            grid.SetNode(block.Position.x, block.Position.y, block);
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
            grid.SetNode(block.Position.x, block.Position.y, null);
            GenerateBlock();
        }
    }
}