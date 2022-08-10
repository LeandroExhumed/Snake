using LeandroExhumed.SnakeGame.Collectables;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeModel : ISnakeModel
    {
        public event Action<IMovementRequester> OnInitialized;
        public event Action<ISnakeModel, Vector2Int> OnPositionChanged;
        public event Action<IBlockModel> OnBlockAttached;
        public event Action<ISnakeModel, bool> OnHit;

        public Vector2Int Position => Head.Position;
        public Vector2Int Direction { get; private set; }
        public float TimeToMove { get; private set; }

        private IBlockModel Head => attachedBlocks.Peek();

        private readonly Stack<IBlockModel> attachedBlocks = new();

        private float timer = 0f;
        private float speedDecreaseOnLoad = 0.05f;
        private bool isAlive = true;
        private bool hasTimeTravel = false;

        private readonly SnakeData data;

        private readonly BlockFactory blockFactory;
        private readonly IGridModel<INode> levelGrid;

        public SnakeModel (SnakeData data, BlockFactory blockFactory, IGridModel<INode> levelGrid)
        {
            this.data = data;
            this.blockFactory = blockFactory;
            this.levelGrid = levelGrid;
        }

        public void Initialize (Vector2Int startPosition, Vector2Int startDirection, IMovementRequester input)
        {
            for (int i = 0; i < data.StartingBlocks.Length; i++)
            {
                Vector2Int blockPosition = new(
                    startPosition.x - ((data.StartingBlocks.Length - 1) - i) * startDirection.x,
                    startPosition.y);
                AttachBlock(blockFactory.Create(data.StartingBlocks[i].ID), blockPosition);
            }

            Direction = startDirection;
            TimeToMove = data.Speed;

            OnInitialized.Invoke(input);
        }

        public void Initialize (SnakePersistentData persistentData, IMovementRequester input)
        {
            for (int i = 0; i < persistentData.Blocks.Length; i++)
            {
                AttachBlock(blockFactory.Create(persistentData.Blocks[i].ID), persistentData.Position);
            }

            Direction = persistentData.Direction;
            TimeToMove = persistentData.TimeToMove;

            OnInitialized.Invoke(input);
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
        }

        public void CollectEnginePower (IBlockModel block, float speedAddition)
        {
            TimeToMove -= speedAddition;
        }

        public void CollectTimeTravel ()
        {
            hasTimeTravel = true;
        }

        public void Tick ()
        {
            if (!isAlive)
            {
                return;
            }

            if (timer >= TimeToMove)
            {
                Move(Direction);
            }

            timer += Time.deltaTime;
        }

        public void Save (SnakePersistentData persistentData)
        {
            persistentData.Position = Position;
            persistentData.Direction = Direction;
            persistentData.TimeToMove = TimeToMove;
            persistentData.HasTimeTravel = hasTimeTravel;

            List<BlockPersistentData> blocks = new ();
            foreach (IBlockModel item in attachedBlocks.ToList())
            {
                blocks.Add(new BlockPersistentData(item.ID, item.Position));
            }
            persistentData.Blocks = blocks.ToArray();
        }

        private void AttachBlock (IBlockModel block, Vector2Int position)
        {
            block.Initialize(position, this);
            block.ApplyEffect();
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
                            isAlive = false;
                            OnHit?.Invoke(this, hasTimeTravel);
                        }
                    }
                    else
                    {
                        block.BeCollected();
                        Grow(block);
                    }
                }
                else if (targetNode is ICollectableModel collectable)
                {
                    collectable.BeCollected();
                }
            }
        }

        private void HandleAttachedBlockPositionChanged (INode block, Vector2Int value)
        {
            levelGrid.SetNode(value, block);
        }
    }
}