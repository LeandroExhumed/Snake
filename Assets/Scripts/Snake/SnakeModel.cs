using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeModel : ISnakeModel
    {
        public event Action<Vector2Int> OnPositionChanged;

        public Vector2Int Position
        {
            get => position;
            set
            {
                position = value;
                OnPositionChanged?.Invoke(value);
            }
        }

        private Vector2Int direction = Vector2Int.right;

        private readonly List<IBodyPartModel> bodyParts = new();

        private float timer = 0f;
        private float speed = 0.5f;
        private float speedDecreaseOnLoad = 0.025f; 

        private readonly IBodyPartModel.Factory bodyPartFactory;

        private Vector2Int position;

        public SnakeModel (IBodyPartModel.Factory bodyPartFactory)
        {
            this.bodyPartFactory = bodyPartFactory;
        }

        public void Initialize (Vector2Int initialPosition, float size)
        {
            Position = initialPosition;

            for (int i = 0; i < size - 1; i++)
            {
                Vector2Int partPosition = i == 0 ? Position : bodyParts[i - 1].Position - Vector2Int.right;
                IBodyPartModel bodyPart = bodyPartFactory.Create();
                bodyPart.Position = partPosition;

                bodyParts.Add(bodyPart);
            }
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

        private void Move (Vector2Int direction)
        {
            Vector2Int lastPosition = bodyParts[0].Position + direction;
            for (int i = 0; i < bodyParts.Count; i++)
            {
                Vector2Int temp = bodyParts[i].Position;
                bodyParts[i].Position = lastPosition;
                lastPosition = temp;
            }

            timer = 0f;
        }
    }
}