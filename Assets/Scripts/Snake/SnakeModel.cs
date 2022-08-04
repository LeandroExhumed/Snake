using LeandroExhumed.SnakeGame.Collectables;
using LeandroExhumed.SnakeGame.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeModel : ISnakeModel
    {
        public event Action<ISnakeModel, Vector2Int> OnPositionChanged;
        public event Action OnHit;

        public Vector2Int Position => bodyParts.Peek().Position;

        private Vector2Int direction = Vector2Int.right;

        private readonly Stack<IBodyPartModel> bodyParts = new();

        private float timer = 0f;
        private float movingInterval;
        private float speedDecreaseOnLoad = 0.05f;

        private readonly SnakeData data;

        private readonly IBodyPartModel.Factory bodyPartFactory;
        private readonly IGridModel grid;

        public SnakeModel (SnakeData data, IBodyPartModel.Factory bodyPartFactory, IGridModel grid)
        {
            this.data = data;
            this.bodyPartFactory = bodyPartFactory;
            this.grid = grid;

            movingInterval = data.Speed;
        }

        public void Initialize (Vector2Int initialPosition)
        {
            for (int i = 0; i < data.Size; i++)
            {
                Vector2Int partPosition = initialPosition - new Vector2Int((data.Size - i), 0);
                AddBodyPart(partPosition);
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
            movingInterval += speedDecreaseOnLoad;
            AddBodyPart(Position);
        }

        public void CollectBatteringRam ()
        {
            Grow();
            Debug.Log("Battering ram effect applied.");
        }

        public void CollectEnginePower (float speedAddition)
        {
            Grow();
            movingInterval -= speedAddition;
        }

        public void Tick ()
        {
            if (timer >= movingInterval)
            {
                Move(direction);
            }

            timer += Time.deltaTime;
        }

        private void AddBodyPart (Vector2Int partPosition)
        {
            IBodyPartModel bodyPart = bodyPartFactory.Create();
            bodyPart.Initialize(partPosition);
            bodyPart.OnPositionChanged += HandleHeadPositionChanged;

            bodyParts.Push(bodyPart);
        }

        private void Move (Vector2Int direction)
        {
            Vector2Int lastPosition = bodyParts.Peek().Position + direction;
            if (lastPosition.x == 30)
            {
                lastPosition.x = 0;
            }
            else if (lastPosition.x < 0)
            {
                lastPosition.x = 29;
            }
            if (lastPosition.y == 30)
            {
                lastPosition.y = 0;
            }
            else if (lastPosition.y < 0)
            {
                lastPosition.y = 29;
            }

            HandleDestination(lastPosition);

            foreach (IBodyPartModel item in bodyParts.ToList())
            {
                Vector2Int temp = item.Position;
                item.Position = lastPosition;
                lastPosition = temp;
            }

            grid.SetNode(lastPosition.x, lastPosition.y, null);

            timer = 0f;
        }

        private void HandleDestination (Vector2Int value)
        {
            INode targetNode = grid.GetNode(value.x, value.y);
            if (targetNode != null)
            {
                if (targetNode is IBodyPartModel)
                {
                    OnHit?.Invoke();
                }
                else if (targetNode is ICollectableModel collectable)
                {
                    collectable.BeCollected(this);
                }
            }
        }

        private void HandleHeadPositionChanged (IBodyPartModel bodyPart, Vector2Int value)
        {
            grid.SetNode(value.x, value.y, bodyPart);
        }
    }
}