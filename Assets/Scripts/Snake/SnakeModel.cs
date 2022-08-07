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
        public event Action<IBlockModel> OnBlockAttached;
        public event Action OnHit;

        public Vector2Int Position => Head.Position;
        public Vector2Int Direction { get; private set; }
        public float TimeToMove { get; private set; }

        private IBlockModel Head => attachedBlocks.Peek();

        private readonly Stack<IBlockModel> attachedBlocks = new();

        private float timer = 0f;
        private float speedDecreaseOnLoad = 0.05f;

        private readonly SnakeData data;

        private readonly BlockFactory blockFactory;
        private readonly IGridModel<INode> levelGrid;

        public SnakeModel (SnakeData data, BlockFactory blockFactory, IGridModel<INode> levelGrid)
        {
            this.data = data;
            this.blockFactory = blockFactory;
            this.levelGrid = levelGrid;
        }

        public void Initialize (Vector2Int startPosition, Vector2Int startDirection)
        {
            for (int i = 0; i < data.Size; i++)
            {
                Vector2Int blockPosition = new(
                    startPosition.x - ((data.Size - 1) - i) * startDirection.x,
                    startPosition.y);
                AttachBlock(blockFactory.Create(1), blockPosition);
            }

            Direction = startDirection;
            TimeToMove = data.Speed;
        }

        public void LookTo (int direction)
        {
            if (direction < 0)
            {
                Direction = new Vector2Int(-Direction.y, Direction.x);
            }
            else
            {
                Direction = new Vector2Int(Direction.y, -Direction.x);
            }
        }

        public void Grow (IBlockModel block)
        {
            TimeToMove += speedDecreaseOnLoad;
            AttachBlock(block, Position);
        }

        public void CollectBatteringRam (IBlockModel block)
        {
            Grow(block);
        }

        public void CollectEnginePower (IBlockModel block, float speedAddition)
        {
            Grow(block);
            TimeToMove -= speedAddition;
        }

        public void Tick ()
        {
            if (timer >= TimeToMove)
            {
                Move(Direction);
            }

            timer += Time.deltaTime;
        }

        private void AttachBlock (IBlockModel block, Vector2Int position)
        {
            block.Initialize(position);
            block.OnPositionChanged += HandleAttachedBlockPositionChanged;

            attachedBlocks.Push(block);

            OnBlockAttached?.Invoke(block);
        }

        private void Move (Vector2Int direction)
        {
            Vector2Int lastPosition = Head.Position + direction;
            // TODO: Improve this.
            if (lastPosition.x == levelGrid.Width)
            {
                lastPosition.x = 0;
            }
            else if (lastPosition.x < 0)
            {
                lastPosition.x = levelGrid.Width - 1;
            }
            if (lastPosition.y == levelGrid.Height)
            {
                lastPosition.y = 0;
            }
            else if (lastPosition.y < 0)
            {
                lastPosition.y = levelGrid.Height - 1;
            }

            HandleDestination(lastPosition);

            foreach (IBlockModel item in attachedBlocks.ToList())
            {
                Vector2Int temp = item.Position;
                item.Position = lastPosition;
                lastPosition = temp;
            }

            levelGrid.SetNode(lastPosition, null);

            timer = 0f;

            OnPositionChanged?.Invoke(this, Position);
        }

        private void HandleDestination (Vector2Int value)
        {
            INode targetNode = levelGrid.GetNode(value);
            if (targetNode != null)
            {
                if (targetNode is IBlockModel block)
                {
                    if (block.IsAttached)
                    {
                        if (Head is BatteringRamBlockModel batteringRam && batteringRam.HasBenefit)
                        {
                            Head.RemoveBenefit();
                        }
                        else
                        {
                            OnHit?.Invoke();
                        }
                    }
                    else
                    {
                        block.BeCollected(this);
                    }
                }
                else if (targetNode is ICollectableModel collectable)
                {
                    collectable.BeCollected(this);
                }
            }
        }

        private void HandleAttachedBlockPositionChanged (INode block, Vector2Int value)
        {
            levelGrid.SetNode(value, block);
        }
    }
}