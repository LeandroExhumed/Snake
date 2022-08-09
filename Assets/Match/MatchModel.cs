using LeandroExhumed.SnakeGame.AI;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using LeandroExhumed.SnakeGame.UI.PlayerSlot;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchModel : IMatchModel
    {
        public event System.Action OnInitialized;
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

        private readonly IPlayerSlotModel[] playerSlots;

        private readonly SnakeFactory snakeFactory;
        private readonly BlockFactory blockFactory;
        private readonly ISimulatedInput.Factory simulatedInput;

        public MatchModel (
            IGridModel<INode> grid,
            IPlayerSlotModel[] playerSlots,
            SnakeFactory snakeFactory,
            BlockFactory blockFactory,
            ISimulatedInput.Factory simulatedInput)
        {
            this.grid = grid;
            this.playerSlots = playerSlots;
            this.snakeFactory = snakeFactory;
            this.blockFactory = blockFactory;
            this.simulatedInput = simulatedInput;
        }

        public void Initialize ()
        {
            grid.Initialize();
            OnInitialized?.Invoke();
        }

        public void AddPlayer (InputAction inputAction)
        {
            IPlayerSlotModel slot = playerSlots.FirstOrDefault(x => x.IsAvailable);
            if (slot != null)
            {
                InputFacade input = new(inputAction);
                input.Initialize();
                slot.Enable(input);
                slot.OnSnakeSelected += HandleSelectionConfirmed;
            }
        }

        public void Play (int selectedSnakeID, IMovementRequester input)
        {
            GenerateSnake(selectedSnakeID, input);
            GenerateBlock();
        }

        private void GenerateSnake (int playableSnakeID, IMovementRequester input)
        {
            for (int i = 0; i < snakesPerMatch; i++)
            {
                ISnakeModel snake;
                Vector2Int startDirection;
                IMovementRequester _input;
                if (i % 2 == 0)
                {
                    snake = snakeFactory.Create(playableSnakeID);
                    startDirection = Vector2Int.right;
                    _input = input;
                }
                else
                {
                    snake = snakeFactory.Create(4);
                    startDirection = Vector2Int.left;
                    _input = simulatedInput.Create(snake);
                }
                snake.Initialize(spawnPositions[i], startDirection, _input);
                snake.OnHit += HandleSnakeHit;
            }
        }

        public void GenerateBlock ()
        {
            block = blockFactory.CreateRandomly(1);
            Vector2Int spawnPosition;
            do
            {
                spawnPosition = new(UnityEngine.Random.Range(0, grid.Width), UnityEngine.Random.Range(0, grid.Height));
            } while (grid.GetNode(spawnPosition) != null);
            block.Initialize(spawnPosition);
            block.OnCollected += HandleBlockCollected;
            grid.SetNode(block.Position, block);

            OnBlockGenerated?.Invoke(block.Position);
        }

        private void HandleSelectionConfirmed (int selectedSnakeID, IMovementRequester input)
        {
            Play(selectedSnakeID, input);
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