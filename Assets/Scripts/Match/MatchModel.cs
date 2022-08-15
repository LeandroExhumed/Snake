using LeandroExhumed.SnakeGame.AI;
using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using LeandroExhumed.SnakeGame.UI.PlayerSlot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchModel : IMatchModel
    {
        public event Action<IBlockModel> OnBlockGenerated;
        public event Action<int, Vector2Int> OnSnakePositionChanged;
        public event Action<Vector2Int?> OnSnakeHit;
        public event Action<int, char, char> OnPlayerLeft;
        public event Action<char, char> OnPlayerReturned;
        public event Action<int> OnOver;
        public event Action OnRestarted;

        public bool IsRunning { get; private set; }

        private readonly GameData data;
        private readonly IGridModel<INodeModel> levelGrid;

        private const float END_MATCH_DELAY = 4F;

        private bool isOnRewindProcess = false;
        private IBlockModel currentRewindResponsible;

        private readonly Dictionary<ISnakeModel, PlayerValues> players = new();
        private readonly List<ISnakeModel> snakes = new();
        private readonly List<IBlockModel> blocks = new();
        private readonly Dictionary<ISnakeModel, IAIInputModel> aiInputs = new();

        private readonly Dictionary<IBlockModel, MatchPersistentData> persistentData = new();

        private readonly IPlayerSlotModel[] playerSlots;

        private readonly SnakeFactory snakeFactory;
        private readonly BlockFactory blockFactory;
        private readonly IAIInputModel.Factory aiInputFactory;
        
        private readonly MonoBehaviour monoBehaviour;

        public MatchModel (
            GameData data,
            IGridModel<INodeModel> levelGrid,
            IPlayerSlotModel[] playerSlots,
            SnakeFactory snakeFactory,
            BlockFactory blockFactory,
            IAIInputModel.Factory aiInputFactory,
            MonoBehaviour monoBehaviour)
        {
            this.data = data;
            this.levelGrid = levelGrid;
            this.playerSlots = playerSlots;
            this.snakeFactory = snakeFactory;
            this.blockFactory = blockFactory;
            this.aiInputFactory = aiInputFactory;
            this.monoBehaviour = monoBehaviour;
        }

        public void Initialize ()
        {
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].Initialize(i + 1);
                playerSlots[i].OnSnakeSelected += HandleSelectionConfirmed;
            }
        }

        public void Begin ()
        {
            IsRunning = true;
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
                    IPlayerInputModel input = new PlayerInputModel(
                        player.Input.LeftKey, player.Input.RightKey);
                    input.Initialize();
                    playerSlots[player.Number - 1].Enable(snake);
                    snake.Initialize(data.Snakes[i], input);
                    snake.OnPositionChanged += HandleSnakePositionChanged;

                    players.Add(snake, new PlayerValues(player.Number, input));

                    OnPlayerReturned?.Invoke(input.LeftKey, input.RightKey);
                }
                else
                {
                    IAIInputModel simulatedInput = aiInputFactory.Create();
                    simulatedInput.Initialize(snake);
                    aiInputs.Add(snake, simulatedInput);
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

            isOnRewindProcess = false;
            persistentData.Remove(currentRewindResponsible);
        }

        public void Restart ()
        {
            Begin();
            OnRestarted?.Invoke();
        }

        private void GenerateSnake (int id, Vector2Int position, Vector2Int direction, IGameInputModel input)
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
                PlayerInputModel input = new(leftKey, rightKey);
                input.Initialize();
                slot.Enable(input);
            }
        }

        private void GenerateSnake (int playableSnakeID, int playerNumber, IPlayerInputModel input)
        {
            for (int i = 0; i < data.AISnakesPerPlayer + 1; i++)
            {
                ISnakeModel snake;
                Vector2Int startPosition;
                Vector2Int startDirection;
                IGameInputModel _input;
                if (i == 0)
                {
                    snake = snakeFactory.Create(playableSnakeID);
                    snake.OnPositionChanged += HandleSnakePositionChanged;
                    startPosition = GetSafeSpawnPosition(new Vector2Int(3, levelGrid.Height - 2));
                    startDirection = Vector2Int.right;
                    _input = input;

                    players.Add(snake, new PlayerValues(playerNumber, input));
                }
                else
                {
                    snake = snakeFactory.Create(4);
                    startPosition = GetSafeSpawnPosition(new Vector2Int(levelGrid.Width - 3, levelGrid.Height - 2));
                    startDirection = Vector2Int.left;
                    IAIInputModel simulatedInput = aiInputFactory.Create();
                    simulatedInput.Initialize(snake);
                    aiInputs.Add(snake, simulatedInput);
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
            } while (levelGrid.GetNode(position) != null && y > 0);

            return position;
        }

        private void GenerateBlock (IBlockModel block, Vector2Int position)
        {
            block.Initialize(position, true);
            block.OnCollected += HandleBlockCollected;
            levelGrid.SetNode(block.Position, block);

            blocks.Add(block);

            OnBlockGenerated?.Invoke(block);
        }

        private void Play (int selectedSnakeID, int playerNumber, IPlayerInputModel input)
        {
            GenerateSnake(selectedSnakeID, playerNumber, input);
            GenerateRandomBlock();
        }

        private void HandleSelectionConfirmed (int selectedSnakeID, int playerNumber, IPlayerInputModel input)
        {
            Play(selectedSnakeID, playerNumber, input);
        }

        public void Save (IBlockModel timeTravelBlock)
        {
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
                spawnPosition = new(UnityEngine.Random.Range(0, levelGrid.Width), UnityEngine.Random.Range(0, levelGrid.Height));
            } while (levelGrid.GetNode(spawnPosition) != null);
            GenerateBlock(blockFactory.Create(4), spawnPosition);
        }

        private void RemoveSnake (ISnakeModel snake)
        {
            if (players.ContainsKey(snake))
            {
                PlayerValues player = players[snake];
                playerSlots[player.Number - 1].Disable();
                players.Remove(snake);

                snake.OnPositionChanged -= HandleSnakePositionChanged;

                OnPlayerLeft?.Invoke(player.Number, player.Input.LeftKey, player.Input.RightKey);
            }
            else
            {
                aiInputs[snake].Destroy();
                aiInputs.Remove(snake);
            }

            snake.OnHit -= HandleSnakeHit;
            snakes.Remove(snake);
        }

        private void Clear ()
        {
            levelGrid.Clear();

            for (int i = 0; i < snakes.Count; i++)
            {
                snakes[i].Destroy();
            }
            snakes.Clear();

            players.Clear();

            foreach (var item in aiInputs)
            {
                item.Value.Destroy();
            }
            aiInputs.Clear();

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
            if (isOnRewindProcess)
            {
                return;
            }

            OnSnakeHit?.Invoke(timeTravelBlock?.Position);

            if (timeTravelBlock != null)
            {
                isOnRewindProcess = true;
                currentRewindResponsible = timeTravelBlock;
                blocks.Remove(timeTravelBlock);

                return;
            }

            RemoveSnake(snake);

            if (snakes.Count == 1)
            {
                monoBehaviour.StartCoroutine(EndMatchDelayRoutine());
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

            foreach (var item in aiInputs)
            {
                item.Value.HandleBlockCollected(block);
            }
            levelGrid.SetNode(block.Position, null);
            blocks.Remove(block);
            GenerateRandomBlock();
        }

        private IEnumerator EndMatchDelayRoutine ()
        {
            yield return new WaitForSeconds(END_MATCH_DELAY);
            IsRunning = false;

            int playerNumber = players.TryGetValue(snakes[0], out PlayerValues player) ? player.Number : 0;
            OnOver?.Invoke(playerNumber);

            Clear();
        }
    }
}