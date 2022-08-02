using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeModel : ISnakeModel
    {
        public event Action<Vector2Int> OnPositionChanged;

        public Vector2Int Position => bodyParts.Peek().Position;

        private Vector2Int direction = Vector2Int.right;

        private readonly Stack<IBodyPartModel> bodyParts = new();

        private float timer = 0f;
        private float speed;
        private float speedDecreaseOnLoad = 0.025f;

        private readonly SnakeData data;

        private readonly IBodyPartModel.Factory bodyPartFactory;

        public SnakeModel (SnakeData data, IBodyPartModel.Factory bodyPartFactory)
        {
            this.data = data;
            this.bodyPartFactory = bodyPartFactory;

            speed = data.Speed;
        }

        public void Initialize (Vector2Int initialPosition)
        {
            for (int i = 0; i < data.Size; i++)
            {
                Vector2Int partPosition = initialPosition - new Vector2Int((data.Size - i), 0);
                AddBodyPart(partPosition);
            }

            bodyParts.Peek().OnPositionChanged += HandleHeadPositionChanged;
        }

        public void LookTo (Vector2Int direction)
        {
            if (this.direction.x == direction.x || this.direction.y == direction.y)
            {
                return;
            }

            this.direction = direction;

            Move(direction);
        }

        public void Grow ()
        {
            speed -= speedDecreaseOnLoad;
        }

        public void IncreaseSpeed (float speedAddition)
        {
            speed += speedAddition;
        }

        public void ApplyBatteringRamEffect ()
        {
            Debug.Log("Battering ram effect applied.");
        }

        public void Tick ()
        {
            if (timer >= speed)
            {
                Move(direction);
            }

            timer += Time.deltaTime;
        }

        private void AddBodyPart (Vector2Int partPosition)
        {
            IBodyPartModel bodyPart = bodyPartFactory.Create();
            bodyPart.Initialize(partPosition);

            bodyParts.Push(bodyPart);
        }

        private void Move (Vector2Int direction)
        {
            Vector2Int lastPosition = bodyParts.Peek().Position + direction;
            foreach (IBodyPartModel item in bodyParts)
            {
                Vector2Int temp = item.Position;
                item.Position = lastPosition;
                lastPosition = temp;
            }

            timer = 0f;
        }

        private void HandleHeadPositionChanged (Vector2Int value)
        {
            OnPositionChanged?.Invoke(value);
        }
    }
}