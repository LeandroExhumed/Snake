using LeandroExhumed.SnakeGame.Collectables;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Input;
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
        private CollectableFacade collectablePrefab;

        public override void InstallBindings ()
        {
            Container.Bind<MatchModel>().AsSingle();

            Container.Bind<Input.PlayerInput>().AsSingle();
            Container.Bind<InputFacade>().AsSingle();

            Container.Bind<IGridModel>().FromInstance(grid);
            Container.Bind<ISnakeModel[]>().FromInstance(snakes);
            Container.BindFactory<ICollectableModel, ICollectableModel.Factory>()
                .FromComponentInNewPrefab(collectablePrefab).AsSingle();
        }
    } 
}