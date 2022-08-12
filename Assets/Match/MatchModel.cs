﻿using LeandroExhumed.SnakeGame.AI;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using LeandroExhumed.SnakeGame.UI.PlayerSlot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchModel : IMatchModel
    {
        public event Action OnInitialized;
        public event Action<IBlockModel> OnBlockGenerated;
        public event Action<int> OnPlayerLeft;
        public event Action<int, Vector2Int> OnSnakePositionChanged;
        public event Action OnRewind;
        public event Action<int> OnOver;

        private int snakesPerMatch = 2;
        private readonly Vector2Int[] spawnPositions =
        {
            new Vector2Int(3, 1),
            new Vector2Int(27, 29)
        };

        private Dictionary<IBlockModel, MatchPersistentData> persistentData = new();
        private IBlockModel currentRewindResponsible;

        private readonly Dictionary<ISnakeModel, int> players = new();
        private readonly List<ISnakeModel> snakes = new();
        private readonly List<IBlockModel> blocks = new();
        private readonly Dictionary<ISnakeModel, ISimulatedInput> simulatedInputs = new();

        private readonly IGridModel<INode> grid;

        private readonly IPlayerSlotModel[] playerSlots;

        private readonly SnakeFactory snakeFactory;
        private readonly BlockFactory blockFactory;
        private readonly ISimulatedInput.Factory simulatedInputFactory;

        public MatchModel (
            IGridModel<INode> grid,
            IPlayerSlotModel[] playerSlots,
            SnakeFactory snakeFactory,
            BlockFactory blockFactory,
            ISimulatedInput.Factory simulatedInputFactory)
        {
            this.grid = grid;
            this.playerSlots = playerSlots;
            this.snakeFactory = snakeFactory;
            this.blockFactory = blockFactory;
            this.simulatedInputFactory = simulatedInputFactory;
        }

        public void Initialize ()
        {
            grid.Initialize();
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].Initialize(i + 1);
            }
            OnInitialized?.Invoke();
        }

        public void Rewind ()
        {
            Clear();

            MatchPersistentData data = persistentData[currentRewindResponsible];            
            for (int i = 0; i < data.Snakes.Count; i++)
            {
                ISnakeModel snake = snakeFactory.Create(data.Snakes[i].ID);
                ISimulatedInput simulatedInput = simulatedInputFactory.Create();
                simulatedInput.Initialize(snake);
                simulatedInputs.Add(snake, simulatedInput);

                snake.Initialize(data.Snakes[i], simulatedInput);
                snake.OnHit += HandleSnakeHit;

                snakes.Add(snake);
            }

            if (data.Blocks.Count > 0)
            {
                for (int i = 0; i < data.Blocks.Count; i++)
                {
                    GenerateBlock(blockFactory.Create(data.Blocks[i].ID), data.Blocks[i].Position);
                }
            }
            else
            {
                GenerateRandomBlock();
            }
            
            //for (int i = 0; i < persistentData.Players.Count; i++)
            //{
            //    ISnakeModel snake = snakeFactory.Create(persistentData.Players[i].Snake.ID);

            //    Vector2Int startDirection = Vector2Int.right;
            //    InputFacade input = new(
            //        persistentData.Players[i].Input.LeftKey, persistentData.Players[i].Input.RightKey);
            //    input.Initialize();

            //    snake.Initialize(spawnPositions[i], startDirection, input);
            //    snake.OnPositionChanged += HandleSnakePositionChanged;
            //    snake.OnHit += HandleSnakeHit;

            //    snakes.Add(snake);

            //    players.Add(snake, persistentData.Players[i].Number);
            //}
        }

        private void GenerateSnake (int id, Vector2Int position, Vector2Int direction, IMovementRequester input)
        {
            ISnakeModel snake = snakeFactory.Create(id);
            snake.Initialize(position, direction, input);
            snake.OnHit += HandleSnakeHit;

            snakes.Add(snake);
        }

        public void AddPlayer (char leftKey, char rightKey)
        {
            IPlayerSlotModel slot = playerSlots.FirstOrDefault(x => x.State == SlotState.Waiting);
            if (slot != null)
            {
                InputFacade input = new(leftKey, rightKey);
                input.Initialize();
                slot.Enable(input);
                slot.OnSnakeSelected += HandleSelectionConfirmed;
            }
        }

        public void Play (int selectedSnakeID, int playerNumber, IMovementRequester input)
        {
            GenerateSnake(selectedSnakeID, playerNumber, input);
            GenerateRandomBlock();
        }

        private void GenerateSnake (int playableSnakeID, int playerNumber, IMovementRequester input)
        {
            for (int i = 0; i < snakesPerMatch; i++)
            {
                ISnakeModel snake;
                Vector2Int startPosition;
                Vector2Int startDirection;
                IMovementRequester _input;
                if (i % 2 == 0)
                {
                    snake = snakeFactory.Create(playableSnakeID);
                    snake.OnPositionChanged += HandleSnakePositionChanged;
                    startPosition = GetSafeSpawnPosition(new Vector2Int(3, grid.Height - 2));
                    startDirection = Vector2Int.right;
                    _input = input;

                    players.Add(snake, playerNumber);
                }
                else
                {
                    snake = snakeFactory.Create(4);
                    startPosition = GetSafeSpawnPosition(new Vector2Int(grid.Width - 3, grid.Height - 2));
                    startDirection = Vector2Int.left;
                    ISimulatedInput simulatedInput = simulatedInputFactory.Create();
                    simulatedInput.Initialize(snake);
                    simulatedInputs.Add(snake, simulatedInput);
                    _input = simulatedInput;
                }
                snake.Initialize(startPosition, startDirection, _input);
                snake.OnHit += HandleSnakeHit;

                snakes.Add(snake);
            }
        }

        private Vector2Int GetSafeSpawnPosition (Vector2Int idealPosition)
        {
            Vector2Int position;
            int y = idealPosition.y;
            do
            {
                position = new Vector2Int(idealPosition.x, y);
                y -= 2;
            } while (grid.GetNode(position) != null && y > 0);

            return position;
        }

        private void GenerateBlock (IBlockModel block, Vector2Int position)
        {
            block.Initialize(position, true);
            block.OnCollected += HandleBlockCollected;
            grid.SetNode(block.Position, block);

            blocks.Add(block);

            OnBlockGenerated?.Invoke(block);
        }

        private void HandleSelectionConfirmed (int selectedSnakeID, int playerNumber, IMovementRequester input)
        {
            Play(selectedSnakeID, playerNumber, input);
        }

        private void End ()
        {
            Debug.Log("You died!");
        }

        public void Save (IBlockModel timeTravelBlock)
        {
            //for (int i = 0; i < snakes.Count; i++)
            //{
            //    SnakePersistentData snakeData = new();
            //    snakes[i].Save(snakeData);

            //    persistentData.Snakes.Add(snakeData);
            //}

            //foreach (var item in players.ToArray())
            //{
            //    item.Key.Save(snake);
            //    PlayerPersistentData player = new(item.Value, new InputPersistentData('q', 'e'), snake);
            //    persistentData.Players.Add(item.Value, player);
            //}

            MatchPersistentData data = new();
            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].IsEqual(timeTravelBlock))
                {
                    continue;
                }

                BlockPersistentData block = new(blocks[i].ID, blocks[i].Position, blocks[i].HasBenefit);
                data.Blocks.Add(block);
            }
            for (int i = 0; i < snakes.Count; i++)
            {
                SnakePersistentData snakeData = new();
                snakes[i].Save(snakeData);
                data.Snakes.Add(snakeData);
            }

            persistentData.Add(timeTravelBlock, data);
        }

        private void GenerateRandomBlock ()
        {
            Vector2Int spawnPosition;
            do
            {
                spawnPosition = new(UnityEngine.Random.Range(0, grid.Width), UnityEngine.Random.Range(0, grid.Height));
            } while (grid.GetNode(spawnPosition) != null);
            GenerateBlock(blockFactory.Create(4), spawnPosition);
        }

        private void RemoveSnake (ISnakeModel snake)
        {
            if (players.ContainsKey(snake))
            {
                int playerNumber = players[snake];
                playerSlots[playerNumber - 1].Disable();
                players.Remove(snake);

                snake.OnPositionChanged -= HandleSnakePositionChanged;

                OnPlayerLeft?.Invoke(playerNumber);
            }
            else
            {
                simulatedInputs[snake].Destroy();
                simulatedInputs.Remove(snake);
            }

            snake.OnHit -= HandleSnakeHit;
            snakes.Remove(snake);
        }

        private void Clear ()
        {
            grid.Clear();

            for (int i = 0; i < snakes.Count; i++)
            {
                snakes[i].Destroy();
            }
            snakes.Clear();

            foreach (var item in simulatedInputs)
            {
                item.Value.Destroy();
            }
            simulatedInputs.Clear();

            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Destroy();
            }
            blocks.Clear();
        }

        private void HandleSnakeHit (ISnakeModel snake, IBlockModel timeTravelBlock)
        {
            if (timeTravelBlock != null)
            {
                currentRewindResponsible = timeTravelBlock;
                blocks.Remove(timeTravelBlock);
                Rewind();

                return;
            }

            RemoveSnake(snake);

            if (snakes.Count == 1)
            {
                int playerNumber = players.TryGetValue(snake, out int number) ? number : 0;
                OnOver?.Invoke(playerNumber);
                End();
            }
        }

        private void HandleSnakePositionChanged (ISnakeModel snake, Vector2Int position)
        {
            OnSnakePositionChanged?.Invoke(players[snake], position);
        }

        private void HandleBlockCollected (IBlockModel block)
        {
            if (block.ID == 4)
            {
                Save(block);
            }

            foreach (var item in simulatedInputs)
            {
                item.Value.HandleBlockCollected(block);
            }
            grid.SetNode(block.Position, null);
            blocks.Remove(block);
            GenerateRandomBlock();
        }
    }
}