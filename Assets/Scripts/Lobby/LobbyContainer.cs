using Zenject;

namespace LeandroExhumed.SnakeGame.Match
{
    public class LobbyContainer : MonoInstaller
    {
        public override void InstallBindings ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            Container.Bind<ILobbyModel>().To<LobbyModel>().AsSingle();
            Container.Bind<IController>().To<LobbyController>().AsSingle();
            Container.BindInstance(GetComponent<LobbyView>()).AsSingle();
        }
    } 
}