using LeandroExhumed.SnakeGame.AI;
using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Input;
using LeandroExhumed.SnakeGame.Snake;
using LeandroExhumed.SnakeGame.UI.PlayerSlot;
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
        private GridFacade levelGrid;

        [SerializeField]
        private PlayerSlotFacade[] playerSlots;

        public override void InstallBindings ()
        {
            ResolveMVC();
            Container.Bind<LobbyModel>().AsSingle();
            Container.BindInstance(matchData).AsSingle();

            Container.Bind<IGridModel<INode>>().FromInstance(levelGrid);
            Container.BindInstance(snakeList.Snakes).AsSingle();
            Container.BindFactory<ISnakeModel, ISimulatedInput, ISimulatedInput.Factory>()
                .FromComponentInNewPrefabResource("SimulatedInput");
            ResolveFactories();

            Container.Bind<IPlayerSlotModel[]>().FromInstance(playerSlots).AsSingle();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IMatchModel>().To<MatchModel>().AsSingle();
            Container.Bind<MatchController>().AsSingle();
        }

        private void ResolveFactories ()
        {
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