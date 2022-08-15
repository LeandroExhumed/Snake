using LeandroExhumed.SnakeGame.AI;
using LeandroExhumed.SnakeGame.Block;
using LeandroExhumed.SnakeGame.Snake;
using LeandroExhumed.SnakeGame.UI.PlayerSlot;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchContainer : MonoInstaller
    {
        [SerializeField]
        private BlockList blockList;

        [SerializeField]
        private PlayerSlotFacade[] playerSlots;

        [SerializeField]
        private AIInputFacade aiInputPrefab;

        [Inject]
        private readonly SnakeData[] snakes;

        public override void InstallBindings ()
        {
            ResolveMVC();
            Container.BindInstance(GetComponent<MonoBehaviour>()).AsSingle();
            
            ResolveFactories();
            Container.Bind<IPlayerSlotModel[]>().FromInstance(playerSlots).AsSingle();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IMatchModel>().To<MatchModel>().AsSingle();
            Container.Bind<MatchController>().AsSingle();
            Container.BindInstance(GetComponent<MatchView>()).AsSingle();
        }

        private void ResolveFactories ()
        {
            Container.BindFactory<IAIInputModel, IAIInputModel.Factory>()
                .FromComponentInNewPrefab(aiInputPrefab);
            for (int i = 0; i < snakes.Length; i++)
            {
                string resource = string.Concat("Snakes/", snakes[i].ID);
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