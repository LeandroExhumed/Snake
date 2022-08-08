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
        private SnakeList snakeList;
        [SerializeField]
        private BlockList blockList;

        [SerializeField]
        private GridFacade grid;

        public override void InstallBindings ()
        {
            Container.Bind<MatchModel>().AsSingle();
            Container.BindInstance(matchData).AsSingle();

            Container.Bind<Input.PlayerInput>().AsSingle();

            Container.Bind<IGridModel<INode>>().FromInstance(grid);

            for (int i = 0; i < snakeList.Snakes.Length; i++)
            {
                string resource = string.Concat("Snakes/", snakeList.Snakes[i].ID);
                Container.BindFactory<ISnakeModel, ISnakeModel.Factory>().FromComponentInNewPrefabResource(resource)
                    .AsCached();
            }
            for (int i = 0; i < blockList.Blocks.Length; i++)
            {
                string resource = string.Concat("Blocks/", blockList.Blocks[i].ID);
                Container.BindFactory<IBlockModel, IBlockModel.Factory>().FromComponentInNewPrefabResource(resource)
                    .AsCached();
            }
            Container.Bind<SnakeFactory>().AsSingle();
            Container.Bind<BlockFactory>().AsSingle();
        }
    } 
}