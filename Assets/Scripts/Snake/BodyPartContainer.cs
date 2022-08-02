using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class BodyPartContainer : MonoInstaller
    {
        public override void InstallBindings ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IBodyPartModel>().To<BodyPartModel>().AsSingle();
            Container.Bind<BodyPartController>().AsSingle();
            Container.BindInstance(GetComponent<BodyPartView>()).AsSingle();
        }
    }
}