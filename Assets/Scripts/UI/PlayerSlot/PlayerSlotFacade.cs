using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using System;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public class PlayerSlotFacade : MonoBehaviour, IPlayerSlotModel
    {
        public event Action<IPlayerInput> OnEnabled
        {
            add => model.OnEnabled += value;
            remove => model.OnEnabled -= value;
        }
        public event Action<int[]> OnSnakeShown
        {
            add => model.OnSnakeShown += value;
            remove => model.OnSnakeShown -= value;
        }
        public event Action<int, int, IPlayerInput> OnSnakeSelected
        {
            add => model.OnSnakeSelected += value;
            remove => model.OnSnakeSelected -= value;
        }
        public event Action OnDisabled
        {
            add => model.OnDisabled += value;
            remove => model.OnDisabled -= value;
        }

        public SlotState State => model.State;

        private IPlayerSlotModel model;
        private PlayerSlotController controller;

        [Inject]
        public void Constructor (IPlayerSlotModel model, PlayerSlotController controller)
        {
            this.model = model;
            this.controller = controller;

            controller.Setup();
        }

        public void Initialize (int playerNumber) => model.Initialize(playerNumber);

        public void Enable (IPlayerInput input) => model.Enable(input);

        public void Enable (ISnakeModel snake) => model.Enable(snake);

        public void ShowNextSnake () => model.ShowNextSnake();

        public void ShowPreviousSnake () => model.ShowPreviousSnake();

        public void Confirm () => model.Confirm();

        private void OnDestroy ()
        {
            controller.Dispose();
        }

        public void Disable ()
        {
            model.Disable();
        }
    }
}