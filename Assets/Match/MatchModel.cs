using LeandroExhumed.SnakeGame.AI;
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
        public event Action<int, char, char> OnPlayerLeft;
        public event Action<int, Vector2Int> OnSnakePositionChanged;
        public event Action OnRewind;
        public event Action<char, char> OnPlayerReturned;
        public event Action<int> OnOver;

        private int snakesPerMatch = 2;
        private readonly Vector2Int[] spawnPositions =
        {
            new Vector2Int(3, 1),
            new Vector2Int(27, 29)
        };

        private Dictionary<IBlockModel, MatchPersistentData> persistentData = new();
        private IBlockModel currentRewindResponsible;

        private readonly Dictionary<ISnakeModel, Player> players = new();
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
                playerSlots[i].OnSnakeSelected += HandleSelectionConfirmed;
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
                PlayerPersistentData player = data.Players.FirstOrDefault(x => x.Snake == data.Snakes[i].Position);
                if (player != null)
                {
                    IPlayerInput input = new InputFacade(
                        player.Input.LeftKey, player.Input.RightKey);
                    input.Initialize();
                    playerSlots[player.Number - 1].Enable(snake);
                    snake.Initialize(data.Snakes[i], input);
                    snake.OnPositionChanged += HandleSnakePositionChanged;

                    players.Add(snake, new Player(player.Number, input));

                    OnPlayerReturned?.Invoke(input.LeftKey, input.RightKey);
                }
                else
                {
                    ISimulatedInput simulatedInput = simulatedInputFactory.Create();
                    simulatedInput.Initialize(snake);
                    simulatedInputs.Add(snake, simulatedInput);
                    snake.Initialize(data.Snakes[i], simulatedInput);
                }

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
            }
        }

        public void Play (int selectedSnakeID, int playerNumber, IPlayerInput input)
        {
            GenerateSnake(selectedSnakeID, playerNumber, input);
            GenerateRandomBlock();
        }

        private void GenerateSnake (int playableSnakeID, int playerNumber, IPlayerInput input)
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

                    players.Add(snake, new Player(playerNumber, input));
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

        private void HandleSelectionConfirmed (int selectedSnakeID, int playerNumber, IPlayerInput input)
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

            foreach (var item in players)
            {
                InputPersistentData input = new(item.Value.Input.LeftKey, item.Value.Input.RightKey);
                data.Players.Add(new PlayerPersistentData(item.Value.Number, input, item.Key.Position));
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
                Player player = players[snake];
                playerSlots[player.Number - 1].Disable();
                players.Remove(snake);

                snake.OnPositionChanged -= HandleSnakePositionChanged;

                OnPlayerLeft?.Invoke(player.Number, player.Input.LeftKey, player.Input.RightKey);
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

            players.Clear();

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

            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].Disable();
            }
        }

        private void HandleSnakeHit (ISnakeModel snake, IBlockModel timeTravelBlock)
        {
            if (timeTravelBlock != null)
            {
                currentRewindResponsible = timeTravelBlock;
                blocks.Remove(timeTravelBlock);
                OnRewind?.Invoke();

                return;
            }

            RemoveSnake(snake);

            if (snakes.Count == 1)
            {
                int playerNumber = players.TryGetValue(snake, out Player player) ? player.Number : 0;
                OnOver?.Invoke(playerNumber);
                End();
            }
        }

        private void HandleSnakePositionChanged (ISnakeModel snake, Vector2Int position)
        {
            OnSnakePositionChanged?.Invoke(players[snake].Number, position);
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

    public struct Player
    {
        public int Number { get; private set; }
        public IPlayerInput Input { get; private set; }

        public Player (int number, IPlayerInput input)
        {
            Number = number;
            Input = input;
        }
    }
}