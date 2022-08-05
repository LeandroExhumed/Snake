using Zenject;

namespace LeandroExhumed.SnakeGame.AI
{
    public class SimulatedInputContainer : MonoInstaller
    {
        public override void InstallBindings ()
        {
            ResolveMVC();
            Container.Bind<PathFinding>().AsSingle();
        }

        private void ResolveMVC ()
        {
            Container.Bind<ISimulatedInput>().To<SimulatedInput>().AsSingle();
            Container.Bind<SimulatedInputController>().AsSingle();
            Container.BindInstance(GetComponent<SimulatedView>()).AsSingle();
        }
    }
}