using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Snake;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchContainer : MonoInstaller
    {
        [SerializeField]
        private MatchData matchData;

        [SerializeField]
        private GridFacade grid;
        [SerializeField]
        private SnakeFacade[] snakes;

        public override void InstallBindings ()
        {
            Container.Bind<MatchModel>().AsSingle();
            Container.BindInstance(matchData).AsSingle();

            Container.Bind<Input.PlayerInput>().AsSingle();

            Container.Bind<IGridModel<INode>>().FromInstance(grid);

            for (int i = 0; i < snakes.Length; i++)
            {
                Container.BindFactory<ISnakeModel, ISnakeModel.Factory>().FromComponentInNewPrefab(snakes[i])
                    .AsCached();
            }
            // TODO: Change magic number (3).
            for (int i = 0; i < 3; i++)
            {
                string resource = string.Concat("Blocks/", i + 1);
                Container.BindFactory<IBlockModel, IBlockModel.Factory>()
                    .FromComponentInNewPrefabResource(resource).AsCached();
            }
            Container.Bind<BlockFactory>().AsSingle();
        }
    } 
}