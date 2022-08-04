﻿using LeandroExhumed.SnakeGame.Collectables;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchModel
    {
        private int snakesPerMatch = 2;
        private readonly Vector2Int[] spawnPositions =
        {
            new Vector2Int(3, 1),
            new Vector2Int(27, 29)
        };
        private ICollectableModel block;
        private ISnakeModel[] snakes;

        private readonly IGridModel<INode> grid;

        private readonly ISnakeModel.Factory[] snakeFactories;
        private readonly ICollectableModel.Factory[] collectableFactories;

        public MatchModel (IGridModel<INode> grid, ISnakeModel.Factory[] snakeFactories, ICollectableModel.Factory[] collectableFactories)
        {
            this.grid = grid;
            this.snakeFactories = snakeFactories;
            this.collectableFactories = collectableFactories;
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