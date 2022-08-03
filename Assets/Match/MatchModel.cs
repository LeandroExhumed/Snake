using LeandroExhumed.SnakeGame.Collectables;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchModel
    {
        private ICollectableModel block;

        private readonly IGridModel grid;
        private readonly ISnakeModel[] snakes;

        private readonly ICollectableModel.Factory collectableFactory;

        public MatchModel (IGridModel grid, ISnakeModel[] snakes, ICollectableModel.Factory collectableFactory)
        {
            this.grid = grid;
            this.snakes = snakes;
            this.collectableFactory = collectableFactory;
        }

        public void Initialize ()
        {
            grid.Initialize();
            GenerateBlock();
        }

        public void GenerateBlock ()
        {
            block = collectableFactory.Create();
            Vector2Int spawnPosition = new(UnityEngine.Random.Range(0, 30), UnityEngine.Random.Range(0, 30));
            block.Initialize(spawnPosition);
            block.OnCollected += HandleBlockCollected;
            grid.SetNode(block.Position.x, block.Position.y, block);

            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i].OnPositionChanged += HandleSnakePositionChanged;
            }
        }

        public void End ()
        {

        }

        private void HandleBlockCollected ()
        {
            grid.SetNode(block.Position.x, block.Position.y, null);
        }

        private void HandleSnakePositionChanged (ISnakeModel snake, Vector2Int value)
        {
            INode targetNode = grid.GetNode(value.x, value.y);
            if (targetNode != null)
            {
                if (targetNode is IBodyPartModel)
                {
                    End();
                }
                else if (targetNode is ICollectableModel collectable)
                {
                    collectable.BeCollected(snake);
                    GenerateBlock();
                }
            }
            
        }
    }
}