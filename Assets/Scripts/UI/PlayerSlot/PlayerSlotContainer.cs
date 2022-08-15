using Zenject;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public class PlayerSlotContainer : MonoInstaller
    {
        public override void InstallBindings ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IPlayerSlotModel>().To<PlayerSlotModel>().AsSingle();
            Container.Bind<IController>().To<PlayerSlotController>().AsSingle();
            Container.BindInstance(GetComponent<PlayerSlotView>()).AsSingle();
        }
    } 
}