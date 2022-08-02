using Zenject;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class BatteringRamBlockContainer : MonoInstaller
    {
        public override void InstallBindings ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            Container.Bind<ICollectableModel>().To<BatteringRamBlockModel>().AsSingle();
            Container.Bind<CollectableController>().AsSingle();
            Container.BindInstance(GetComponent<CollectableView>()).AsSingle();
        }
    }
}