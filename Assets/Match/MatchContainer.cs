using LeandroExhumed.SnakeGame.Collectables;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchContainer : MonoInstaller
    {
        [SerializeField]
        private GridFacade grid;
        [SerializeField]
        private SnakeFacade[] snakes;

        [SerializeField]
        private CollectableFacade[] collectables;

        public override void InstallBindings ()
        {
            Container.Bind<MatchModel>().AsSingle();

            Container.Bind<Input.PlayerInput>().AsSingle();

            Container.Bind<IGridModel<INode>>().FromInstance(grid);

            for (int i = 0; i < snakes.Length; i++)
            {
                Container.BindFactory<ISnakeModel, ISnakeModel.Factory>().FromComponentInNewPrefab(snakes[i])
                    .AsCached();
            }
            for (int i = 0; i < collectables.Length; i++)
            {
                Container.BindFactory<ICollectableModel, ICollectableModel.Factory>()
                    .FromComponentInNewPrefab(collectables[i]).AsCached();
            }
        }
    } 
}