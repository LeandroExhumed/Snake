using Zenject;

namespace LeandroExhumed.SnakeGame.Collectables
{
    public class EnginePowerBlockContainer : MonoInstaller
    {
        public override void InstallBindings ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            Container.Bind<ICollectableModel>().To<EnginePowerBlockModel>().AsSingle();
            Container.Bind<CollectableController>().AsSingle();
            Container.BindInstance(GetComponent<CollectableView>()).AsSingle();
        }
    }
}