using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Services;
using System;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Block
{
    public class BlockController : IDisposable
    {
        private readonly IBlockModel model;
        private readonly BlockView view;

        private readonly AudioProvider audioProvider;

        public BlockController (IBlockModel model, BlockView view, AudioProvider audioProvider)
        {
            this.model = model;
            this.view = view;
            this.audioProvider = audioProvider;
        }

        public void Setup ()
        {
            model.OnPositionChanged += HandlePositionChanged;
            model.OnCollected += HandleCollected;
            model.OnAttached += HandleAttached;
            model.OnBenefitRemoved += HandleBenefitRemoved;
            model.OnHit += HandleHit;
            model.OnDestroyed += HandleDestroyed;
        }

        private void HandlePositionChanged (INodeModel model, Vector2Int value)
        {
            view.Position = value;
        }

        private void HandleCollected (IBlockModel obj)
        {
            audioProvider.PlayOneShot(view.CollectedSound);
        }

        private void HandleAttached (Transform owner)
        {
            view.SetParent(owner);
        }

        private void HandleBenefitRemoved ()
        {
            view.SetNoBenefitVisual();
        }

        private void HandleHit ()
        {
            view.PlayExplosionVFX();
        }

        private void HandleDestroyed ()
        {
            view.Destroy();
        }

        public void Dispose ()
        {
            model.OnPositionChanged -= HandlePositionChanged;
            model.OnCollected -= HandleCollected;
            model.OnAttached -= HandleAttached;
            model.OnBenefitRemoved -= HandleBenefitRemoved;
            model.OnHit -= HandleHit;
            model.OnDestroyed -= HandleDestroyed;
        }
    }
}